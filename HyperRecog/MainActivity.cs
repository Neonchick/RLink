using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android;
using Plugin.Media;
using Android.Graphics;
using System.Collections.Generic;
using Android.Content;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net.Http.Headers;
using Nancy.Helpers;
using Newtonsoft.Json;
using ResponseLIb;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Essentials;

namespace HyperRecog
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {
        Button cameraButton;
        Button recogniseButton;
        Button uploadButton;

        ImageView imageView;
        static byte[] imageArray;

        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.AccessNetworkState
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            imageView = (ImageView)FindViewById(Resource.Id.imageView);

            cameraButton = (Button)FindViewById(Resource.Id.cameraButton);
            cameraButton.Click += CameraButton_Click;

            uploadButton = (Button)FindViewById(Resource.Id.uploadButton);
            uploadButton.Click += UploadButton_Click;

            recogniseButton = (Button)FindViewById(Resource.Id.recogniseButton);
            recogniseButton.Click += RecogniseButton_ClickAsync;

            RequestPermissions(permissionGroup, 0);
        }

        private async void UploadButton_Click(object sender, System.EventArgs e)
        {
            uploadButton.Clickable = false;

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                Toast.MakeText(this, "Невозможно загрузить фотографию", ToastLength.Short).Show();
                uploadButton.Clickable = true;
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 100
            });

            if (file == null)
            {
                uploadButton.Clickable = true;
                return;
            }

            imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            imageView.SetImageBitmap(bitmap);

            uploadButton.Clickable = true;
        }

        private async void RecogniseButton_ClickAsync(object sender, System.EventArgs e)
        {
            recogniseButton.Clickable = false;

            if (imageArray == null)
            {
                Toast.MakeText(this, "Фото не выбрано", ToastLength.Short).Show();
                recogniseButton.Clickable = true;
                return;
            }

            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                Toast.MakeText(this, "Необходимо подключение к интернету", ToastLength.Short).Show();
                recogniseButton.Clickable = true;
                return;
            }

            List<string> recognizedLinks = new List<string>();

            await MakeRequest(recognizedLinks);

            if (recognizedLinks.Count == 0)
                Toast.MakeText(this, "Ничего не распознаннно", ToastLength.Short).Show();
            else 
            {
                Intent intent = new Intent(this, typeof(ChooseActivity));
                intent.PutExtra("linkList", recognizedLinks.ToArray());
                StartActivity(intent);
            }

            recogniseButton.Clickable = true;
        }

        async Task MakeRequest(List<string> recognizedLinks)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9243aac33d7f41f4a0dbe1430bdabd05");

            // Request parameters
            queryString["language"] = "en";
            queryString["detectOrientation"] = "true";
            var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr?" + queryString;

            HttpResponseMessage httpResponseMessage;

            // Request body
            byte[] byteData = imageArray;

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                httpResponseMessage = await client.PostAsync(uri, content);
            }
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            Toast.MakeText(this, "Тута", ToastLength.Short).Show();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Response response = JsonConvert.DeserializeObject<Response>(responseString);
                foreach (ResponseLIb.Region region in response.Regions)
                    foreach (Line line in region.Lines)
                    {
                        StringBuilder stringLine = new StringBuilder();
                        foreach (Word word in line.Words)
                        {
                            if (Regex.IsMatch(word.Text, @".*(https?|ftp|www){1,}.*", RegexOptions.IgnoreCase))
                                recognizedLinks.Add(word.Text);
                            stringLine.Append(word.Text);
                        }
                        if (Regex.IsMatch(stringLine.ToString(), @".*(https?|ftp|www){1,}.*", RegexOptions.IgnoreCase))
                        {
                            bool f = true;
                            foreach (var elem in recognizedLinks)
                                if (stringLine.ToString() == elem)
                                    f = false;
                            if (f)
                                recognizedLinks.Add(stringLine.ToString());
                        }
                    }
            }
            else Toast.MakeText(this, "Неудачный запрос", ToastLength.Short).Show();
        }

        private async void CameraButton_Click(object sender, System.EventArgs e)
        {
            cameraButton.Clickable = false;
            await CrossMedia.Current.Initialize();

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                CompressionQuality = 100,
                Name = "myimage.jpg",
                Directory = "sample"
            });

            if (file == null)
            {
                cameraButton.Clickable = true;
                return;
            }

            imageArray = System.IO.File.ReadAllBytes(file.Path);
            Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
            imageView.SetImageBitmap(bitmap);
            cameraButton.Clickable = true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}