using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aves.Models;
using Microsoft.WindowsAzure.MobileServices;

namespace Aves
{
    class AzureManager
    {
        private static AzureManager _instance;
        private MobileServiceClient _client;
        private IMobileServiceTable<SearchHistoryModel> searchHistoryTable;

        private AzureManager()
        {
            _client = new MobileServiceClient(@"http://avesnz.azurewebsites.net");
            searchHistoryTable = _client.GetTable<SearchHistoryModel>();
        }

        public MobileServiceClient AzureClient => _client;
        public static AzureManager AzureManagerInstance => _instance ?? (_instance = new AzureManager());

        public async Task<List<SearchHistoryModel>> GetSearchHistory()
        {
            return await searchHistoryTable.ToListAsync();
        }

        public async Task SubmitSearchHistory(SearchHistoryModel searchHistory)
        {
            await searchHistoryTable.InsertAsync(searchHistory);
        }
    }
}
