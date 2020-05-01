using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using SQLite;
using System.IO;
using SQLLib;

namespace HyperRecog
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class ResultActivity : Activity
    {
        EditText link;
        EditText test;

        Button copyButton;
        Button goToLinkButton;
        Button shareButton;
        Button saveButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.result);

            link = FindViewById<EditText>(Resource.Id.link);
            link.Text = Intent.GetStringExtra("recognizedLink");

            copyButton = FindViewById<Button>(Resource.Id.copyButton);
            copyButton.Click += CopyButton_Click;

            goToLinkButton = FindViewById<Button>(Resource.Id.goToLinkButton);
            goToLinkButton.Click += GoToLinkButton_Click;

            shareButton = FindViewById<Button>(Resource.Id.shareButton);
            shareButton.Click += ShareButton_Click;

            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += SaveButton_Click;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            DBElem linkDB = new DBElem("test", link.Text);
            db.Insert(linkDB);   
        }

        private async void ShareButton_Click(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = link.Text,
                Title = "Поделиться ссылкой"
            });
        }

        private void GoToLinkButton_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(link.Text));
            StartActivity(intent);
        }

        private async void CopyButton_Click(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(link.Text);
            Toast.MakeText(this, "Ссылка скопирована", ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}