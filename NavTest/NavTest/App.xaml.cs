using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavTest
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public ElementTheme GlobalElementTheme { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            
            var localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("Theme"))
            {
                GlobalElementTheme = (ElementTheme)Enum.Parse(typeof(ElementTheme), localSettings.Values["Theme"].ToString());
            }
            else
            {
                GlobalElementTheme = ElementTheme.Default;
                localSettings.Values["Theme"] = GlobalElementTheme.ToString();
            }


            /*
            if (GlobalElementTheme == ElementTheme.Light)
            {
                RequestedTheme = ApplicationTheme.Light;
            }
            else if (GlobalElementTheme == ElementTheme.Dark)
            {
                RequestedTheme = ApplicationTheme.Dark;
            }
            */
        }



        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }


        private MainWindow _window;

        public MainWindow MainWindow => _window;
    }
}
