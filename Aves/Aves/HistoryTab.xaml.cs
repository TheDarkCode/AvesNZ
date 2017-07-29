using Aves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<SearchHistoryModel> searchHistory = await AzureData.AzureDataInstance.GetSearchHistory();
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

            //TODO: more efficient way of retrieving coordinates.
            var allItems = await AzureData.AzureDataInstance.GetSearchHistory();
            SearchHistoryModel selectedItem = allItems.SingleOrDefault(s => s.Id == btn.CommandParameter.ToString());

            //TODO: Add iOS & WinMobile platform URI's
            Device.OpenUri(new Uri($"geo:0,0?q={selectedItem.Latitude} {selectedItem.Longitude}"));
        }
    }
}