﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Aves.Models
{
    class SearchHistoryModel
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Date")]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "Bird")]
        public string Bird { get; set; }

        [JsonProperty(PropertyName = "Longitude")]
        public float Longitude { get; set; }
        [JsonProperty(PropertyName = "Latitude")]
        public float Latitude { get; set; }
    }
}
