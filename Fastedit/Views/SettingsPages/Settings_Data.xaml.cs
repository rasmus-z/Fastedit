﻿using Fastedit.Dialogs;
using Fastedit.Settings;
using Fastedit.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Fastedit.Views.SettingsPages
{
    public sealed partial class Settings_Data : Page
    {
        public Settings_Data()
        {
            this.InitializeComponent();
        }

        private void InitSize()
        {
            temporaryFileSizeDisplay.Text = TemporaryFilesHandler.GetSize();
            recyclebinFilesizeDisplay.Text = RecycleBinDialog.GetSize();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitSize();
            base.OnNavigatedTo(e);
        }

        //Import/Export settings
        private async void ExportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (await SettingsImportExport.Export())
                InfoMessages.SettingsExportSucceed();
            else
                InfoMessages.SettingsExportFailed();
        }
        private async void ImportSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (await SettingsImportExport.Import())
                InfoMessages.SettingsImportSucceed();
            else
                InfoMessages.SettingsImportFailed();
        }

        //Clear recylcebin
        private async void ClearRecyclebin_Click(object sender, RoutedEventArgs e)
        {
            if (await RecycleBinDialog.ClearRecycleBin() == ClearRecycleBinResult.Success)
                InfoMessages.RecyclebinClearSucceed();

            InitSize();
        }
        private async void ClearTemporaryFiles_Click(object sender, RoutedEventArgs e)
        {
            if (await TemporaryFilesHandler.Clear())
                InfoMessages.ClearTemporaryFilesSucceed();
            else
                InfoMessages.ClearTemporaryFilesFailed();

            InitSize();
        }
    }
}
