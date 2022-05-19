using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NavTest.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavTest
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        bool _isCustomizationSupported;
        ThemeWatcher _themeWatcher;


        public MainWindow()
        {
            _isCustomizationSupported = AppWindowTitleBar.IsCustomizationSupported();

            this.InitializeComponent();


            _themeWatcher = new ThemeWatcher();
            _themeWatcher.ThemeChanged += ThemeWatcher_ThemeChanged;
            _themeWatcher.Start();


            if (_isCustomizationSupported)
            {                
                var appWindow = GetAppWindowForCurrentWindow();
                var appWindowTitleBar = appWindow.TitleBar;
                appWindowTitleBar.ExtendsContentIntoTitleBar = true;
                appWindowTitleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;

                /*
                appWindowTitleBar.SetDragRectangles(new Windows.Graphics.RectInt32[]
                {
                    new Windows.Graphics.RectInt32(0, 0, 1, 1),
                });
                */
            }
            else
            {
                RootGrid.RowDefinitions[0].Height = new GridLength(28);

                ExtendsContentIntoTitleBar = true;
                SetTitleBar(AppTitleBar);


                var appWindow = GetAppWindowForCurrentWindow();
                appWindow.Title = "NavTest";


                //var fakeMinMaxCloseButtons = new FakeMinMaxCloseButtons();
                //TitlebarGrid.Children.Add(fakeMinMaxCloseButtons);\
            }

            UpdateColors(((App)Application.Current).GlobalElementTheme);

            PageContent.Navigate(typeof(ThemePickerPage));
        }


        void UpdateColorsLight()
        {
            RootGrid.RequestedTheme = ElementTheme.Light;


            var app = ((App)Application.Current);
            var theme = app.Resources.MergedDictionaries[1].ThemeDictionaries["Light"] as ResourceDictionary;


            if (_isCustomizationSupported)
            {
                var appWindow = GetAppWindowForCurrentWindow();
                var appWindowTitleBar = appWindow.TitleBar;

                
                appWindowTitleBar.ButtonBackgroundColor = (Color)theme["ButtonBackgroundColor"];
                appWindowTitleBar.ButtonForegroundColor = (Color)theme["ButtonForegroundColor"];
                appWindowTitleBar.ButtonHoverBackgroundColor = (Color)theme["ButtonHoverBackgroundColor"];
                appWindowTitleBar.ButtonHoverForegroundColor = (Color)theme["ButtonHoverForegroundColor"];
                appWindowTitleBar.ButtonInactiveBackgroundColor = (Color)theme["ButtonInactiveBackgroundColor"];
                appWindowTitleBar.ButtonInactiveForegroundColor = (Color)theme["ButtonInactiveForegroundColor"];
                appWindowTitleBar.ButtonPressedBackgroundColor = (Color)theme["ButtonPressedBackgroundColor"];
                appWindowTitleBar.ButtonPressedForegroundColor = (Color)theme["ButtonPressedForegroundColor"];
                
            }
            else
            {
                var res = Application.Current.Resources;

                // Removes the tint on title bar
                res["WindowCaptionBackground"] = Colors.Transparent;
                // Sets the tint of the forground of the buttons
                res["WindowCaptionForeground"] = theme["TitleBarForground"];
                res["WindowCaptionButtonBackgroundPointerOver"] = theme["TitleBarHover"];
                //TitleBarBackground
                //TitleBarHover

                RepaintCurrentWindow();
            }
        }

        void UpdateColorsDark()
        {
            RootGrid.RequestedTheme = ElementTheme.Dark;

            var app = ((App)Application.Current);
            var theme = app.Resources.MergedDictionaries[1].ThemeDictionaries["Dark"] as ResourceDictionary;


            if (_isCustomizationSupported)
            {
                var appWindow = GetAppWindowForCurrentWindow();
                var appWindowTitleBar = appWindow.TitleBar;

                
                appWindowTitleBar.ButtonBackgroundColor = (Color)theme["ButtonBackgroundColor"];
                appWindowTitleBar.ButtonForegroundColor = (Color)theme["ButtonForegroundColor"];
                appWindowTitleBar.ButtonHoverBackgroundColor = (Color)theme["ButtonHoverBackgroundColor"];
                appWindowTitleBar.ButtonHoverForegroundColor = (Color)theme["ButtonHoverForegroundColor"];
                appWindowTitleBar.ButtonInactiveBackgroundColor = (Color)theme["ButtonInactiveBackgroundColor"];
                appWindowTitleBar.ButtonInactiveForegroundColor = (Color)theme["ButtonInactiveForegroundColor"];
                appWindowTitleBar.ButtonPressedBackgroundColor = (Color)theme["ButtonPressedBackgroundColor"];
                appWindowTitleBar.ButtonPressedForegroundColor = (Color)theme["ButtonPressedForegroundColor"];
                

                /*
                BackgroundColor
                ForegroundColor
                InactiveBackgroundColor
                InactiveForegroundColor                 
                */
            }
            else
            {
                var res = Microsoft.UI.Xaml.Application.Current.Resources;

                // Removes the tint on title bar
                res["WindowCaptionBackground"] = Colors.Transparent;
                // Sets the tint of the forground of the buttons
                res["WindowCaptionForeground"] = theme["TitleBarForground"];
                res["WindowCaptionButtonBackgroundPointerOver"] = theme["TitleBarHover"];
                //TitleBarBackground
                //TitleBarHover

                RepaintCurrentWindow();
            }


        }


        void ThemeWatcher_ThemeChanged(object sender, ApplicationTheme e)
        {
            var globalTheme = ((App)Application.Current).GlobalElementTheme;

            if (globalTheme == ElementTheme.Default)
            {
                var osApplicationTheme = _themeWatcher.GetWindowsApplicationTheme();
                if (osApplicationTheme == ApplicationTheme.Light)
                {
                    UpdateColorsLight();
                }
                else if (osApplicationTheme == ApplicationTheme.Dark)
                {
                    UpdateColorsDark();
                }
            }
        }


        internal void UpdateColors(ElementTheme theme)
        {
            ((App)Application.Current).GlobalElementTheme = theme;

            if (theme == ElementTheme.Light)
            {
                UpdateColorsLight();
            }
            else if (theme == ElementTheme.Dark)
            {
                UpdateColorsDark();
            }
            else
            {
                var osApplicationTheme = _themeWatcher.GetWindowsApplicationTheme();
                if (osApplicationTheme == ApplicationTheme.Light)
                {
                    UpdateColorsLight();
                }
                else if (osApplicationTheme == ApplicationTheme.Dark)
                {
                    UpdateColorsDark();
                }
            }          
        }

        // to trigger repaint tracking task id 38044406
        void RepaintCurrentWindow()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

            var activeWindow = Win32.GetActiveWindow();
            if (hWnd == activeWindow)
            {
                Win32.SendMessage(hWnd, Win32.WM_ACTIVATE, Win32.WA_INACTIVE, IntPtr.Zero);
                Win32.SendMessage(hWnd, Win32.WM_ACTIVATE, Win32.WA_ACTIVE, IntPtr.Zero);
            }
            else
            {
                Win32.SendMessage(hWnd, Win32.WM_ACTIVATE, Win32.WA_ACTIVE, IntPtr.Zero);
                Win32.SendMessage(hWnd, Win32.WM_ACTIVATE, Win32.WA_INACTIVE, IntPtr.Zero);
            }
        }


        /*
        void RootGrid_ActualThemeChanged(FrameworkElement sender, object args)
        {
            Console.WriteLine(sender.ActualTheme);

            if (((App)Application.Current).GlobalElementTheme == ElementTheme.Default)
            {
                if (sender.ActualTheme == ElementTheme.Light)
                {
                    UpdateColorsLight();
                }
                else if (sender.ActualTheme == ElementTheme.Dark)
                {
                    UpdateColorsDark();
                }
            }
        }
        */

        AppWindow GetAppWindowForCurrentWindow()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }
    }
}
