using System;
using System.Windows.Forms;

namespace ColorPicker.Executable
{
    internal static class Program
    {
        [STAThread]
        public static int Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var colorPicker = new UserInterface.ColorPicker();
            Application.Run(colorPicker);


            var selectedColorAsHex =
                $"#{colorPicker.SelectedColor.R:X2}{colorPicker.SelectedColor.G:X2}{colorPicker.SelectedColor.B:X2}";
            Clipboard.SetText(selectedColorAsHex);

            return (colorPicker.SelectedColor.R << 16) | (colorPicker.SelectedColor.G << 8) |
                   colorPicker.SelectedColor.B;
        }
    }
}