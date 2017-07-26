using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Aves.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Aves
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IdentifyTab : ContentPage
    {
        public IdentifyTab()
        {
            InitializeComponent();
        }

        private async void BtnCamera_OnClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No camera", "Aves needs access to the camera in order to work", "Close");
                return;
            }

            // click animation
            await stcCamera.ScaleTo(1.1, 100, Easing.CubicIn);
            await stcCamera.ScaleTo(1, 100, Easing.Linear);

            MediaFile image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "AvesCurrentImage",
                Name = $"{DateTime.Now}"
            });

            NavigateToPrediction(image);
        }

        private async void BtnGallery_OnClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("No gallery access", "Aves needs access to the gallery in order to work", "Close");
                return;
            }

            // click animation
            await stckGallery.ScaleTo(1.1, 100, Easing.CubicIn);
            await stckGallery.ScaleTo(1, 100, Easing.Linear);

            MediaFile image = await CrossMedia.Current.PickPhotoAsync();

            NavigateToPrediction(image);
        }

        private async void NavigateToPrediction(MediaFile image)
        {
            if (image == null)
                return;

            await Navigation.PushModalAsync(new PredictionTab(image), true);
        }
    }
}