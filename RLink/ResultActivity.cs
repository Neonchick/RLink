using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Xamarin.Essentials;

namespace RLink
{
    /// <summary>
    /// Активити результата распознования.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ResultActivity : Activity
    {
        /// <summary>
        /// Текствое поле ссылки.
        /// </summary>
        EditText link;

        /// <summary>
        /// Кнопка копирования.
        /// </summary>
        Button copyButton;
        /// <summary>
        /// Кнопка перехода.
        /// </summary>
        Button goToLinkButton;
        /// <summary>
        /// Кнопка поделиться.
        /// </summary>
        Button shareButton;
        /// <summary>
        /// Кнопка сохранения.
        /// </summary>
        Button saveButton;

        /// <summary>
        /// Флаг нажатия кнопки.
        /// </summary>
        bool buttonFlag = false;

        /// <summary type="void" dos="protected">
        /// Обрабодчик создания актививти.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Находим layout.
            SetContentView(Resource.Layout.result);

            // Находим тествое поле ссылки и присваеваем ему значение.
            link = FindViewById<EditText>(Resource.Id.link);
            link.Text = Intent.GetStringExtra("recognizedLink");

            // Находим кнопку копирования и добовляем обрабодчик нажатия.
            copyButton = FindViewById<Button>(Resource.Id.copyButton);
            copyButton.Click += CopyButton_Click;

            // Находим кнопку перейти и добовляем обрабодчик нажатия.
            goToLinkButton = FindViewById<Button>(Resource.Id.goToLinkButton);
            goToLinkButton.Click += GoToLinkButton_Click;

            // Находим кнопку кподелится и добовляем обрабодчик нажатия.
            shareButton = FindViewById<Button>(Resource.Id.shareButton);
            shareButton.Click += ShareButton_Click;

            // Находим кнопку сохранения и добовляем обрабодчик нажатия.
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += SaveButton_Click;
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку сохранить.
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Только одна кнопка может быть нажата.
            if (buttonFlag)
                return;

            buttonFlag = true;

            try
            {
                // Вызываем активити сохранеия ссылки.
                Intent intent = new Intent(this, typeof(SaveNewLinkActivity));
                intent.PutExtra("saveNewLink", link.Text);
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку поделиться.
        /// </summary>
        private async void ShareButton_Click(object sender, EventArgs e)
        {
            // Только одна кнопка может быть нажата.
            if (buttonFlag)
                return;
            buttonFlag = true;

            try
            {
                // Поделиться ссылкой.
                await Share.RequestAsync(new ShareTextRequest
                {
                    Uri = link.Text,
                    Title = "Поделиться ссылкой"
                });
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку перейти.
        /// </summary>
        private void GoToLinkButton_Click(object sender, EventArgs e)
        {
            // Только одна кнопка может быть нажата.
            if (buttonFlag)
                return;
            buttonFlag = true;

            try
            {
                // Вызываем активити для перехода по ссылке.
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(link.Text));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку скопировать.
        /// </summary>
        private async void CopyButton_Click(object sender, EventArgs e)
        {
            // Только одна кнопка может быть нажата.
            if (buttonFlag)
                return;
            buttonFlag = true;

            try
            {
                // Копируем текст в буфер обмена.
                await Clipboard.SetTextAsync(link.Text);
                Toast.MakeText(this, "Ссылка скопирована", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary type="void" dos="public">
        /// Обрабодчик получения разрешений.
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}