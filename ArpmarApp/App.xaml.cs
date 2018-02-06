using System.Windows;

namespace ArpmarApp
{
    public partial class App : Application
    {
        public static void ShowMessageBox(string text) 
            => MessageBox.Show(text, "arpMAR");
    }
}
