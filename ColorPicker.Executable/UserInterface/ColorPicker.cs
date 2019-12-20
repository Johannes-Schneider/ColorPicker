using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PInvoke;

namespace ColorPicker.Executable.UserInterface
{
    public partial class ColorPicker : Form
    {
        private static readonly Size SizeOneByOne = new Size(1, 1);


        private IntPtr _magnificationControlHandle = IntPtr.Zero;

        private float _magnificationFactor = 1.0f;

        public ColorPicker()
        {
            InitializeComponent();

            MouseWheel += OnMouseWheel;
            MouseDown += OnMouseDown;

            Screenshot = Graphics.FromImage(SelectedPixel);
        }

        public float MagnificationFactor
        {
            get => _magnificationFactor;
            set
            {
                if (value == MagnificationFactor)
                {
                    return;
                }

                _magnificationFactor = Math.Max(1.0f, value);

                if (!IsRunning) return;

                Magnification.MagSetWindowTransform(_magnificationControlHandle,
                    new Magnification.MAGTRANSFORM
                        {[0, 0] = MagnificationFactor, [1, 1] = MagnificationFactor, [2, 2] = 1.0f});
            }
        }

        public bool IsRunning { get; private set; }

        public Color SelectedColor { get; private set; }

        private Timer UpdateTimer { get; } = new Timer();

        private Bitmap SelectedPixel { get; } = new Bitmap(SizeOneByOne.Width, SizeOneByOne.Height);

        private Graphics Screenshot { get; }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            IsRunning = false;
            Close();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                MagnificationFactor += 0.25f;
            }
            else
            {
                MagnificationFactor -= 0.25f;
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            IsRunning = Magnification.MagInitialize();

            _magnificationControlHandle = CreateMagnificationControl();
            MagnificationFactor = 2.0f;

            UpdateTimer.Interval = 10;
            UpdateTimer.Tick += UpdateTimer_Tick;
            UpdateTimer.Start();
        }

        private IntPtr CreateMagnificationControl()
        {
            var applicationHandle = Kernel32.GetModuleHandle(string.Empty);
            User32.GetClientRect(MagnifierHost.Handle, out var clientRect);
            return User32.CreateWindowEx(
                0u,
                "Magnifier",
                string.Empty,
                User32.WindowStyles.WS_CHILD | User32.WindowStyles.WS_VISIBLE,
                clientRect.left,
                clientRect.top,
                clientRect.right - clientRect.left,
                clientRect.bottom - clientRect.top,
                MagnifierHost.Handle,
                IntPtr.Zero,
                applicationHandle.DangerousGetHandle(),
                IntPtr.Zero);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                return;
            }

            var cursorPosition = User32.GetCursorPos();
            var locationX = cursorPosition.x - Width / 2;
            var locationY = cursorPosition.y - Height / 2;

            locationX = Math.Max(SystemInformation.VirtualScreen.Left,
                Math.Min(SystemInformation.VirtualScreen.Right - Width, locationX));
            locationY = Math.Max(SystemInformation.VirtualScreen.Top,
                Math.Min(SystemInformation.VirtualScreen.Bottom - Height, locationY));

            Location = new Point(locationX, locationY);

            var sourceWidth = (int) (MagnifierHost.Size.Width / MagnificationFactor);
            var sourceHeight = (int) (MagnifierHost.Size.Height / MagnificationFactor);
            var sourceLeft = cursorPosition.x - sourceWidth / 2;
            var sourceTop = cursorPosition.y - sourceHeight / 2;

            var magnificationSourceRectangle = new RECT
            {
                left = sourceLeft,
                top = sourceTop,
                right = sourceLeft + sourceWidth,
                bottom = sourceTop + sourceHeight
            };

            MagSetWindowSource(_magnificationControlHandle, magnificationSourceRectangle);

            Screenshot.CopyFromScreen(cursorPosition.x, cursorPosition.y, 0, 0, SizeOneByOne);
            SelectedColor = SelectedPixel.GetPixel(0, 0);

            SelectedColorDisplay.BackColor = SelectedColor;
            SelectedColorLabel.BackColor = SelectedColor;
            SelectedColorLabel.Text = $"#{SelectedColor.R:X2}{SelectedColor.G:X2}{SelectedColor.B:X2}";

            var foreColor = 0;
            var average = (SelectedColor.R + SelectedColor.G + SelectedColor.B) / 3;
            if (average < 128) foreColor = 255;

            var contrastColor = Color.FromArgb(foreColor, foreColor, foreColor);

            BackColor = contrastColor;
            SelectedColorLabel.ForeColor = contrastColor;
        }

        [DllImport("Magnification.dll", CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool MagSetWindowSource(IntPtr hWnd, RECT rect);

        protected override void WndProc(ref Message m)
        {
            const uint WM_MOUSEACTIVATE = 0x0021;
            if (m.Msg != WM_MOUSEACTIVATE)
            {
                base.WndProc(ref m);
                return;
            }

            IsRunning = false;
            Close();
        }

        ~ColorPicker()
        {
            IsRunning = false;
            UpdateTimer.Stop();
            Screenshot.Dispose();
            SelectedPixel.Dispose();
            Magnification.MagUninitialize();
        }
    }
}