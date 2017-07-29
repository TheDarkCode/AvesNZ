using Aves.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aves
{
    class AzureData
    {
        private static AzureData _instance;
        private MobileServiceClient _client;
        private IMobileServiceTable<SearchHistoryModel> searchHistoryTable;

        public AzureData()
        {
            _client = new MobileServiceClient(@"http://avesnz.azurewebsites.net");
            searchHistoryTable = _client.GetTable<SearchHistoryModel>();
        }

        public MobileServiceClient AzureClient => _client;
        public static AzureData AzureDataInstance => _instance ?? (_instance = new AzureData());

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
