using System;
using System.Collections.Generic;
using Aves.Models;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aves
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryTab : ContentPage
    {
        //private MobileServiceClient client = AzureManager.AzureManagerInstance.AzureClient;

        public HistoryTab()
        {
            InitializeComponent();
        }

        private async void Handle_ClickedAsync(object sender, EventArgs e)
        {
            List<SearchHistoryModel> searchHistory = await AzureManager.AzureManagerInstance.GetSearchHistory();
            lvHistory.ItemsSource = searchHistory;
        }
    }
}