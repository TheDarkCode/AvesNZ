using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Aves.Models;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aves
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryTab : ContentPage
    {

        public HistoryTab()
        {
            InitializeComponent();
            GetSearchHistory();
        }

        private async void GetSearchHistory()
        {
            List<SearchHistoryModel> searchHistory = await AzureManager.AzureManagerInstance.GetSearchHistory();
            HistoryListView.ItemsSource = searchHistory;
            HistoryListView.IsRefreshing = false;
        }

        private void HistoryListView_OnRefreshing(object sender, EventArgs e)
        {
            GetSearchHistory();
        }

        private async void BtnMap_OnClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            var allItems = await AzureManager.AzureManagerInstance.GetSearchHistory();
            SearchHistoryModel selectedItem = allItems.SingleOrDefault(s => s.Id == btn.CommandParameter.ToString());

            Device.OpenUri(new Uri($"geo:0,0?q={selectedItem.Latitude} {selectedItem.Longitude}"));
        }
    }
}