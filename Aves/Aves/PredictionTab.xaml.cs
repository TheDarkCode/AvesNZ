﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aves.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aves
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionTab : ContentPage
    {
        private List<String> _predictions;

        public PredictionTab(MediaFile image)
        {
            InitializeComponent();
            PerformTasks(image);
        }

        private async void PerformTasks(MediaFile image)
        {
            // old, causes issues with threads...
            //Task.Run(() => this.MakePrediction(image)).ContinueWith(ShowResults(_predictions, image));
            await MakePrediction(image);
            ShowResults(_predictions, image);
        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakePrediction(MediaFile image)
        {
            var client = new HttpClient { DefaultRequestHeaders = { { "Prediction-Key", "0e0a735690e14bccafd0ac94acd572e5" } } };

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v1.0/Prediction/3f3b150a-e19c-4acb-ae03-5d7078f5c288/image?iterationId=ec6c19e5-4f8e-4385-87c5-3754f8ce4102";

            byte[] byteData = GetImageAsByteArray(image);

            List<string> prediction = null;

            using (var content = new ByteArrayContent(byteData))
            {

                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    EvaluationModel responseModel = JsonConvert.DeserializeObject<EvaluationModel>(responseString);

                    prediction = responseModel.Predictions.Where(p => p.Probability > 0.7).Select(p => p.Tag).ToList();
                }
                else
                {
                    await DisplayAlert("Error", "Something went wrong, please try again\n" + response.StatusCode, "OK");
                }

                _predictions = prediction;
            }
        }

        private void ShowResults(List<string> prediction, MediaFile image)
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

        private async void StartOver_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}