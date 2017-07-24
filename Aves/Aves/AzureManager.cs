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
            this._client = new MobileServiceClient(@"http://avesnz.azurewebsites.net");
            this.searchHistoryTable = this._client.GetTable<SearchHistoryModel>();
        }

        public MobileServiceClient AzureClient => _client;

        public static AzureManager AzureManagerInstance => _instance ?? (_instance = new AzureManager());

        public async Task<List<SearchHistoryModel>> GetSearchHistory()
        {
            return await this.searchHistoryTable.ToListAsync();
        }
    }
}
