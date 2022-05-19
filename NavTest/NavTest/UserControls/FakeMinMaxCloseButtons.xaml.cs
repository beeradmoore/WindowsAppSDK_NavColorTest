using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavTest.UserControls
{
    public sealed partial class FakeMinMaxCloseButtons : UserControl
    {

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        private const int SW_NORMAL = 1;
        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;
        private const int GWL_STYLE = -16;
        private const long WS_MAXIMIZE = 0x01000000L;
        private const long WS_MINIMIZE = 0x20000000L; 

        public FakeMinMaxCloseButtons()
        {
            this.InitializeComponent();

            // I tried to just use <ContentControl x:Name="MainContentControl" Style="{StaticResource WindowChromeStyle}" /> and hook into the buttons themselves but that did not work.
            // I don't know how to get a reference to the internal objects loaded by a style, eg.
            // var layoutRoot = MainContentControl.FindName("LayoutRoot");

            // I also tried loading the style and getting a refernece that way.            
            /*
            var windowChromeStyle = Resources["WindowChromeStyle"] as Style;
            var settter = windowChromeStyle.Setters[0] as Setter;
            var controlTemplate = settter.Value as ControlTemplate;
            var properties = controlTemplate.TargetType.GetProperties();
            foreach (var property in properties)
            {
                System.Diagnostics.Debug.WriteLine(property.Name);
                if (property.Name == "Content")
                {
                    var val = property.GetValue(null, null);
                }
            }
            */

            /*
            var fakeWindowCaptionButton = Resources["FakeWindowCaptionButton"] as Style;
            foreach (Microsoft.UI.Xaml.Setter setter in fakeWindowCaptionButton.BasedOn.Setters)
            {
                if (setter.Value.GetType() == typeof(Microsoft.UI.Xaml.Controls.ControlTemplate))
                {
                    var controlTemplate = setter.Value as Microsoft.UI.Xaml.Controls.ControlTemplate;

///                    var value = controlTemplate.GetValue(Border.propery);

                    break;
                }
            }
            */


            /*
            MinimizeButton.Click += MinimizeButton_Click;
            MaximizeButton.Click += MaximizeButton_Click;
            CloseButton.Click += CloseButton_Click;
            */
        }

        /*
        void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
            ShowWindow(hWnd, SW_MINIMIZE);
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);

            var style = GetWindowLong(hWnd, GWL_STYLE);
            if ((style & WS_MAXIMIZE) == WS_MAXIMIZE)
            {
                ShowWindow(hWnd, SW_RESTORE);
                VisualStateManager.GoToState(MaximizeButton, "WindowStateNormal", false);
            }
            else
            {
                ShowWindow(hWnd, SW_MAXIMIZE);
                VisualStateManager.GoToState(MaximizeButton, "WindowStateMaximized", false);
            }
        }
        */

        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).MainWindow.Close();
        }
    }
}
