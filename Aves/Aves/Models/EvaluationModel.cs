using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aves.Models
{
    public class EvaluationModel
    {
        public string Id { get; set; }
        public List<PredictionModel> Predictions { get; set; }
    }
}
