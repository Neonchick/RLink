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
using System;

namespace RLink
{
    /// <summary>
    /// Активити для рапознования новой ссылки.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class RecognizeActivity : Activity
    {
        /// <summary>
        /// Кнопка сделать новое фото.
        /// </summary>
        Button cameraButton;

        /// <summary>
        /// Кнопка распознать ссылку.
        /// </summary>
        Button recogniseButton;

        /// <summary>
        /// Кнопка для загрузки изображения.
        /// </summary>
        Button uploadButton;

        /// <summary>
        /// Место для загрузки изображения.
        /// </summary>
        ImageView imageView;

        /// <summary>
        /// Массив байтов для хранения изображения.
        /// </summary>
        static byte[] imageArray;

        /// <summary>
        /// Массив разрешений.
        /// </summary>
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.AccessNetworkState,
            Manifest.Permission.Internet
        };

        /// <summary>
        /// Флаг нажатия кнопки.
        /// </summary>
        bool buttonFlag = false;

        /// <summary>
        /// Обрабодчик создания Активити.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Выбираем соответствующий layout.
            SetContentView(Resource.Layout.activity_main);

            // Находим место для изображения.
            imageView = (ImageView)FindViewById(Resource.Id.imageView);

            imageArray = null;

            // Находим кнопку для нового фото и добавляем обрабодчик ее нажатия. 
            cameraButton = (Button)FindViewById(Resource.Id.cameraButton);
            cameraButton.Click += CameraButton_Click;

            // Находим кнопку для загрузки фото и добавляем обрабодчик ее нажатия. 
            uploadButton = (Button)FindViewById(Resource.Id.uploadButton);
            uploadButton.Click += UploadButton_Click;

            // Находим кнопку для распознования ссылок и добавляем обрабодчик ее нажатия. 
            recogniseButton = (Button)FindViewById(Resource.Id.recogniseButton);
            recogniseButton.Click += RecogniseButton_ClickAsync;

            // Запрашиваем разрешения.
            RequestPermissions(permissionGroup, 0);
        }

        /// <summary>
        /// Обрабодчик нажатия на кнопку для загрузки.
        /// </summary>
        private async void UploadButton_Click(object sender, System.EventArgs e)
        {
            // Две кнопки не должны обрабатыватся одновременно.
            if (buttonFlag)
                return;
            buttonFlag = true;

            try
            {
                await CrossMedia.Current.Initialize();

                // Проверяем возможность загрузить фото.
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    Toast.MakeText(this, "Невозможно загрузить фотографию", ToastLength.Short).Show();
                    buttonFlag = false;
                    return;
                }

                // Загружаем фото.
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 100
                });

                // Проверяем файл на пустоту.
                if (file == null)
                {
                    buttonFlag = false;
                    return;
                }

                // Считываем фото из файла.
                imageArray = System.IO.File.ReadAllBytes(file.Path);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                imageView.SetImageBitmap(bitmap);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary>
        /// Обрабодчик нажатия на кнопку распознать. 
        /// </summary>
        private async void RecogniseButton_ClickAsync(object sender, System.EventArgs e)
        {
            // Две кнопки не должны обрабатыватся одновременно.
            if (buttonFlag)
                return;
            buttonFlag = true;

            try
            {
                // Проверка наличия изображения.
                if (imageArray == null)
                {
                    Toast.MakeText(this, "Фото не выбрано", ToastLength.Short).Show();
                    buttonFlag = false;
                    return;
                }

                // Проверка подключения к интернету.
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    Toast.MakeText(this, "Необходимо подключение к интернету", ToastLength.Short).Show();
                    buttonFlag = false;
                    return;
                }

                // Список распознанных ссылок.
                List<string> recognizedLinks = new List<string>();

                // Запрос к сервисам Майкросовт.
                await MakeRequest(recognizedLinks);

                // Проверяем наличие ссылок.
                if (recognizedLinks.Count == 0)
                    Toast.MakeText(this, "Ничего не распознано", ToastLength.Short).Show();
                else
                {
                    // Вызываем Активити с выбором ссылок.
                    Intent intent = new Intent(this, typeof(ChooseActivity));
                    intent.PutExtra("linkList", recognizedLinks.ToArray());
                    StartActivity(intent);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            buttonFlag = false;
        }

        /// <summary>
        /// Метод для запроса к сервисам Компьютерного зрения.
        /// </summary>
        /// <param name="recognizedLinks">Распознанные ссылки.</param>
        async Task MakeRequest(List<string> recognizedLinks)
        {
            // Создадим запрос.
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Вводим ключ.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9243aac33d7f41f4a0dbe1430bdabd05");

            // Параметры запроса.
            queryString["language"] = "en";
            queryString["detectOrientation"] = "true";
            var uri = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr?" + queryString;

            HttpResponseMessage httpResponseMessage;

            // Изображение для запроса.
            byte[] byteData = imageArray;

            // Делаем запрос.
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                httpResponseMessage = await client.PostAsync(uri, content);
            }
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

            // Проверяем успешность запроса.
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                // Перевод JSON формата.
                Response response = JsonConvert.DeserializeObject<Response>(responseString);
                // Пройдемся по всем регионам, строкам и словам.
                foreach (ResponseLIb.Region region in response.Regions)
                    foreach (Line line in region.Lines)
                    {
                        StringBuilder stringLine = new StringBuilder();
                        foreach (Word word in line.Words)
                        {
                            // Проверяем каждое слово.
                            if (Regex.IsMatch(word.Text, @".*(https?|ftp|www){1,}.*", RegexOptions.IgnoreCase))
                                recognizedLinks.Add(word.Text);
                            stringLine.Append(word.Text);
                        }
                        // Проверяем всю линию.
                        if (Regex.IsMatch(stringLine.ToString(), @".*(https?|ftp|www){1,}.*", RegexOptions.IgnoreCase))
                        {
                            // Строки не должны повторяться.
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
            client.Dispose();
        }

        /// <summary>
        /// Обрабодчик нажатия на кнопку для новой фотографии.
        /// </summary>
        private async void CameraButton_Click(object sender, System.EventArgs e)
        {
            // Только одна кнопка должна обрабатываться.
            if (buttonFlag)
                return;

            buttonFlag = true;

            try
            {
                await CrossMedia.Current.Initialize();

                // Делаем фото.
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 100,
                    Name = "myimage.jpg",
                    Directory = "sample"
                });

                // Проверка файла на пустоту.
                if (file == null)
                {
                    buttonFlag = false;
                    return;
                }

                // Загрузка изображения на экран.
                imageArray = System.IO.File.ReadAllBytes(file.Path);
                Bitmap bitmap = BitmapFactory.DecodeByteArray(imageArray, 0, imageArray.Length);
                imageView.SetImageBitmap(bitmap);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary>
        /// Обрабодчик получения разрешений.
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}