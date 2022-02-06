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
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NavTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ThemePickerPage : Page
    {
        private bool _startingUp = true;
        public ThemePickerPage()
        {
            this.InitializeComponent();


            var selectedTheme = ApplicationData.Current.LocalSettings.Values["Theme"].ToString();
            if (selectedTheme == "Light")
            {
                LightRadioButton.IsChecked = true;
            }
            else if (selectedTheme == "Dark")
            {
                DarkRadioButton.IsChecked = true;
            }
            else
            {
                SystemRadioButton.IsChecked = true;
            }

            _startingUp = false;
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (_startingUp)
            {
                return;
            }

            if  (sender is RadioButton radioButton && radioButton.Tag is String radioButtonTag)
            {
                var newTheme = radioButtonTag switch
                {
                    "Dark" => ElementTheme.Dark,
                    "Light" => ElementTheme.Light,
                    _ => ElementTheme.Default,
                };

                ApplicationData.Current.LocalSettings.Values["Theme"] = newTheme.ToString();
                ((App)Application.Current)?.MainWindow?.UpdateColors(newTheme);
            }
        }
    }
}
