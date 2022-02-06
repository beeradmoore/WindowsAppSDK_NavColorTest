﻿using Microsoft.UI.Xaml;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace NavTest
{
    // Class inspired by https://stackoverflow.com/a/69604613/1253832
    public class ThemeWatcher
    {
        private const string RegistryThemeKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistryThemeValueName = "AppsUseLightTheme";
        private const string RegistryContrastKeyPath = @"Control Panel\Accessibility\HighContrast";
        private const string RegistryContrastValueName = "LastUpdatedThemeId";

        ManagementEventWatcher _themeWatcher;
        ManagementEventWatcher _contrastWatcher;
        AccessibilitySettings _accessibilitySettings;
        ApplicationTheme _defaultApplicationTheme;

        public enum WindowsTheme
        {
            Default = 0,
            Light = 1,
            Dark = 2,
            HighContrast = 3
        }

        public event EventHandler<ApplicationTheme> ThemeChanged;
        public event EventHandler<bool> ContrastChanged;



        public bool IsWatchingTheme { get; private set; } = false;
        public bool IsWatchingContrast { get; private set; } = false;

        public bool HighContrast {  get {  return _accessibilitySettings.HighContrast; } }


        public ThemeWatcher(ApplicationTheme defaultApplicationTheme = ApplicationTheme.Dark)
        {
            _accessibilitySettings = new AccessibilitySettings();
            _defaultApplicationTheme = defaultApplicationTheme;
        }

        public void Start()
        {
            // Cleanup incase start is called twice.
            Stop();


            var currentUser = WindowsIdentity.GetCurrent();

            var themeQuery = String.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User.Value,
                RegistryThemeKeyPath.Replace(@"\", @"\\"),
                RegistryThemeValueName);

            var contrastQuery = String.Format(
               CultureInfo.InvariantCulture,
               @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
               currentUser.User.Value,
               RegistryContrastKeyPath.Replace(@"\", @"\\"),
               RegistryContrastValueName);


            try
            {
                _themeWatcher = new ManagementEventWatcher(themeQuery);
                _themeWatcher.EventArrived += ThemeWatcher_EventArrived;
                _themeWatcher.Start();
                IsWatchingTheme = true;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"ThemeWatcher Error: {err.Message}");
            }

            try
            {
                _contrastWatcher = new ManagementEventWatcher(contrastQuery);
                _contrastWatcher.EventArrived += ContrastWatcher_EventArrived;
                _contrastWatcher.Start();
                IsWatchingContrast = true;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"ThemeWatcher Error: {err.Message}");
            }

            System.Diagnostics.Debug.WriteLine($"ThemeWatcher Start ({DateTimeOffset.UtcNow.ToUnixTimeSeconds()}): {GetWindowsTheme()}, {HighContrast}");

        }

        public void Stop()
        {
            if (_themeWatcher != null)
            {
                _themeWatcher.EventArrived -= ContrastWatcher_EventArrived;
                _themeWatcher.Stop();
                _themeWatcher = null;
                IsWatchingTheme = false;
            }

            if (_contrastWatcher != null)
            {
                _contrastWatcher.EventArrived -= ContrastWatcher_EventArrived;
                _contrastWatcher.Stop();
                _contrastWatcher = null;
                IsWatchingContrast = false;
            }
        }

        private void ContrastWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ContrastWatcher_EventArrived ({DateTimeOffset.UtcNow.ToUnixTimeSeconds()}): {GetWindowsTheme()}, {HighContrast}");

            ContrastChanged?.Invoke(this, HighContrast);
        }

        private void ThemeWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ThemeWatcher_EventArrived ({DateTimeOffset.UtcNow.ToUnixTimeSeconds()}): {GetWindowsTheme()}, {HighContrast}");

            ThemeChanged?.Invoke(this, GetWindowsApplicationTheme());
        }

        public ApplicationTheme GetWindowsApplicationTheme()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryThemeKeyPath))
                {
                    if (key?.GetValue(RegistryThemeValueName) is int registryValue)
                    {
                        System.Diagnostics.Debug.WriteLine($"RegistryValue: {registryValue}");
                        return registryValue > 0 ? ApplicationTheme.Light : ApplicationTheme.Dark;
                    }

                }
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"GetWindowsTheme Error: {err.Message}");
            }

            return _defaultApplicationTheme;
        }
        

        public WindowsTheme GetWindowsTheme()
        {
            WindowsTheme theme = WindowsTheme.Light;

            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(RegistryThemeKeyPath))
                {
                    if (key?.GetValue(RegistryThemeValueName) is int registryValue)
                    {
                        System.Diagnostics.Debug.WriteLine($"RegistryValue: {registryValue}");
                        theme = registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;

                        return theme;
                    }

                }

                return theme;
            }
            catch (Exception err)
            {
                System.Diagnostics.Debug.WriteLine($"GetWindowsTheme Error: {err.Message}");
                return theme;
            }
        }
    }
}
