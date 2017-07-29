using Aves.Models;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aves
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionTab : ContentPage
    {
        private List<String> _predictions;
        private MediaFile _image;

        public PredictionTab(MediaFile image)
        {
            InitializeComponent();
            PerformTasks(image);
        }

        private async void PerformTasks(MediaFile image)
        {
            _image = image;
            // old, causes issues with threads...
            //Task.Run(() => this.MakePrediction(image)).ContinueWith(ShowResults(_predictions, image));
            await MakePrediction(_image);
            ShowResults(_predictions, _image);
        }

        private byte[] ImageToByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePrediction(MediaFile image)
        {
            var client = new HttpClient { DefaultRequestHeaders = { { "Prediction-Key", "0e0a735690e14bccafd0ac94acd572e5" } } };

            const string apiUrl = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/3f3b150a-e19c-4acb-ae03-5d7078f5c288/image?iterationId=ec6c19e5-4f8e-4385-87c5-3754f8ce4102";

            var dataInBytes = ImageToByteArray(image);

            List<string> prediction = null;

            using (var content = new ByteArrayContent(dataInBytes))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseContent = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    prediction = responseContent.Predictions.Where(p => p.Probability > 0.7).Select(p => p.Tag).ToList();
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, please try again\n" + response.StatusCode, "OK");
                    return;
                }

                _predictions = prediction;
            }
        }

        private async void ShowResults(List<string> prediction, MediaFile image)
        {
            imgPhoto.Source = image.Path;

            switch (prediction.Count)
            {
                case 0:
                    lblIntro.Text = "Sorry, there's no match.";
                    lblPredictionLabel.Text = "Maybe try with another image?";
                    break;
                case 1:
                    lblIntro.Text = "We think it\'s a";
                    lblPredictionLabel.Text = prediction[0];

                    // save searched item
                    await SubmitSearchHistory(prediction);
                    break;
                default:
                    StringBuilder result = new StringBuilder();

                    foreach (string item in prediction)
                    {
                        result.AppendLine(item + " ");
                    }
                    lblIntro.Text = "It could be one of these:";
                    lblPredictionLabel.Text = result.ToString();
                    break;
            }

            //reveal result and image
            stckLoading.IsVisible = false;
            stckResult.IsVisible =
            stckImage.IsVisible = true;
        }

        // save search to history
        private static async Task SubmitSearchHistory(List<string> prediction)
        {
            //get location
            var locatorInstance = CrossGeolocator.Current;
            locatorInstance.DesiredAccuracy = 40;

            var userPosition = await locatorInstance.GetPositionAsync(TimeSpan.FromSeconds(5));

            await AzureData.AzureDataInstance.SubmitSearchHistory(new SearchHistoryModel()
            {
                Bird = prediction[0],
                Date = DateTime.Now,
                Longitude = (float)userPosition.Longitude,
                Latitude = (float)userPosition.Latitude
            });
        }

        private async void StartOver_OnClicked(object sender, EventArgs e)
        {
            _image.Dispose();
            await Navigation.PopModalAsync();
        }

        private async void BtnSaveImage_OnClicked(object sender, EventArgs e)
        {
            //TODO: Save image to gallery. At the moment image remains in the Android/Aves/Temp folder
            await Navigation.PopModalAsync();
        }
    }
}