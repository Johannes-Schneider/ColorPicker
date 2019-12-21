using System;
using System.Drawing;
using System.Windows.Forms;
using ColorPicker.Executable.PInvoke;
using ColorPicker.Executable.Properties;

namespace ColorPicker.Executable.UserInterface
{
    public partial class ColorPicker : Form
    {
        private static readonly Size SizeOneByOne = new Size(1, 1);

        private static readonly float MinMagnificationFactor = 1.0f;
        private static readonly float MaxMagnificationFactor = 10.0f;

        private float _magnificationFactor = 1.0f;

        public float MagnificationFactor
        {
            get => _magnificationFactor;
            set
            {
                if (value == MagnificationFactor)
                {
                    return;
                }

                _magnificationFactor = Math.Max(MinMagnificationFactor, Math.Min(MaxMagnificationFactor, value));

                if (!IsRunning)
                {
                    return;
                }

                var transform = Magnification.Transform.Magnify(MagnificationFactor);
                Magnification.MagSetWindowTransform(_magnificationControlHandle, ref transform);
            }
        }

        public bool IsRunning { get; private set; }

        public Color SelectedColor { get; private set; }

        private Timer UpdateTimer { get; } = new Timer();

        private Bitmap SelectedPixel { get; } = new Bitmap(SizeOneByOne.Width, SizeOneByOne.Height);

        private Graphics Screenshot { get; }

        private IntPtr _magnificationControlHandle = IntPtr.Zero;

        public ColorPicker()
        {
            InitializeComponent();
            Icon = Resources.ColorPickerIcon;

            MouseWheel += OnMouseWheel;
            MouseDown += OnMouseDown;
            PreviewKeyDown += OnPreviewKeyDown;

            Screenshot = Graphics.FromImage(SelectedPixel);
        }

        private void OnPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var movement = new Point(0, 0);

            switch (e.KeyCode)
            {
                case Keys.Left:
                {
                    movement.X = -1;
                    break;
                }
                case Keys.Right:
                {
                    movement.X = 1;
                    break;
                }
                case Keys.Up:
                {
                    movement.Y = -1;
                    break;
                }
                case Keys.Down:
                {
                    movement.Y = 1;
                    break;
                }
                case Keys.Space: // fallthrough
                case Keys.Enter:
                {
                    // confirm selection
                    IsRunning = false;
                    Close();
                    return;
                }
                default:
                {
                    return;
                }
            }

            User32.GetCursorPos(out var cursorPosition);
            User32.SetCursorPos(cursorPosition.X + movement.X, cursorPosition.Y + movement.Y);
        }

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
            MagnifierHost.Cursor = Cursors.Cross;

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
                clientRect.Left,
                clientRect.Top,
                clientRect.Right - clientRect.Left,
                clientRect.Bottom - clientRect.Top,
                MagnifierHost.Handle,
                IntPtr.Zero,
                applicationHandle,
                IntPtr.Zero);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                return;
            }

            User32.GetCursorPos(out var cursorPosition);
            var locationX = cursorPosition.X - Width / 2;
            var locationY = cursorPosition.Y - Height / 2;

            locationX = Math.Max(SystemInformation.VirtualScreen.Left,
                Math.Min(SystemInformation.VirtualScreen.Right - Width, locationX));
            locationY = Math.Max(SystemInformation.VirtualScreen.Top,
                Math.Min(SystemInformation.VirtualScreen.Bottom - Height, locationY));

            Location = new Point(locationX, locationY);

            var sourceWidth = (int) (MagnifierHost.Size.Width / MagnificationFactor);
            var sourceHeight = (int) (MagnifierHost.Size.Height / MagnificationFactor);
            var sourceLeft = cursorPosition.X - sourceWidth / 2;
            var sourceTop = cursorPosition.Y - sourceHeight / 2;

            var magnificationSourceRectangle = new Win32.Rectangle(
                sourceLeft,
                sourceTop,
                sourceLeft + sourceWidth,
                sourceTop + sourceHeight
            );

            Magnification.MagSetWindowSource(_magnificationControlHandle, magnificationSourceRectangle);

            Screenshot.CopyFromScreen(cursorPosition.X, cursorPosition.Y, 0, 0, SizeOneByOne);
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