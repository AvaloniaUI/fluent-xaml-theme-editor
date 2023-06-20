﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using FluentEditor.ControlPalette.Model;
using FluentEditor.Model;
using FluentEditor.OuterNav;
using FluentEditorShared;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace FluentEditor
{
    sealed partial class App : Application
    {
        public static FilePickerFileType JsonFileType { get; } = new("JSON")
        {
            Patterns = new[] { "*.json" },
            MimeTypes = new[] { "application/json" },
            AppleUniformTypeIdentifiers = new[] { "public.json" }
        };
        
        public static OuterNavPage NavPage { get; private set; }
        
        public App()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            base.OnFrameworkInitializationCompleted();
            
            await SetupDependencies();
            
            NavPage = new OuterNavPage(_outerNavViewModel);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime classicDesktopStyleApplicationLifetime)
            {
                var window = new Window
                {
                    Content = NavPage
                };
                classicDesktopStyleApplicationLifetime.MainWindow = window;
                classicDesktopStyleApplicationLifetime.Exit += (sender, args) =>
                {
                    _ = SuspendApp(_mainNavModel, _paletteModel);
                };
                window.Show();
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewApplicationLifetime)
            {
                singleViewApplicationLifetime.MainView = NavPage;
            }
        }

        private async Task SuspendApp(IMainNavModel mainModel, IControlPaletteModel controlPaletteModel)
        {
            await controlPaletteModel.HandleAppSuspend();
            await mainModel.HandleAppSuspend();
        }

        private async Task SetupDependencies()
        {
            var stringProvider = new StringProvider();
            var exportProvider = new ControlPaletteExportProvider();

            var paletteModel = new ControlPaletteModel();
            await paletteModel.InitializeData(stringProvider, stringProvider.GetString("ControlPaletteDataPath"));

            var navModel = new MainNavModel(stringProvider);
            await navModel.InitializeData(stringProvider.GetString("MainDataPath"), paletteModel, exportProvider);

            _stringProvider = stringProvider;
            _exportProvider = exportProvider;

            _paletteModel = paletteModel;

            _mainNavModel = navModel;
            _outerNavViewModel = new OuterNavViewModel(_mainNavModel.NavItems, _mainNavModel.DefaultNavItem);
        }

        private StringProvider _stringProvider;
        private ControlPaletteExportProvider _exportProvider;
        private IMainNavModel _mainNavModel;
        private IControlPaletteModel _paletteModel;

        private OuterNavViewModel _outerNavViewModel;
    }
}
