using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using SQLLib;
using Xamarin.Essentials;

namespace HyperRecog
{
    [Activity(Label = "LoadActivity", Theme = "@style/AppTheme")]
    public class LoadActivity : Activity
    {
        int id;

        EditText name;
        EditText link;
        EditText description;

        Button copyButton;
        Button goToLinkButton;
        Button shareButton;
        Button saveButton;
        Button deleteButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.load_link);

            id = Intent.GetIntExtra("loadId", 0);

            link = FindViewById<EditText>(Resource.Id.loadLinkEditText);
            link.Text = Intent.GetStringExtra("loadLink");

            name = FindViewById<EditText>(Resource.Id.loadNameEditText);
            name.Text = Intent.GetStringExtra("loadName");

            description = FindViewById<EditText>(Resource.Id.loadDescriptionEditText);
            description.Text = Intent.GetStringExtra("loadDescription");

            copyButton = FindViewById<Button>(Resource.Id.loadCopyButton);
            copyButton.Click += CopyButton_Click;

            goToLinkButton = FindViewById<Button>(Resource.Id.loadGoToLinkButton);
            goToLinkButton.Click += GoToLinkButton_Click;

            shareButton = FindViewById<Button>(Resource.Id.loadShareButton);
            shareButton.Click += ShareButton_Click;

            saveButton = FindViewById<Button>(Resource.Id.loadSaveButton);
            saveButton.Click += SaveButton_Click;

            deleteButton = FindViewById<Button>(Resource.Id.loadDeleteButton);
            deleteButton.Click += DeleteButton_Click;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Clickable = false;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            db.Delete(id, db.GetMapping<DBElem>());
            this.Finish();

            deleteButton.Clickable = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveButton.Clickable = false;

            if (name.Text == "")
                name.Text = "Без названия";

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            db.Delete(id, db.GetMapping<DBElem>());
            DBElem linkDB = new DBElem(name.Text, link.Text, description.Text);
            db.Insert(linkDB);
            this.Finish();

            saveButton.Clickable = true;
        }

        private async void ShareButton_Click(object sender, EventArgs e)
        {
            shareButton.Clickable = false;

            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = link.Text,
                Title = "Поделиться ссылкой"
            });

            shareButton.Clickable = true;
        }

        private void GoToLinkButton_Click(object sender, EventArgs e)
        {
            goToLinkButton.Clickable = false;

            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(link.Text));
            StartActivity(intent);

            goToLinkButton.Clickable = true;
        }

        private async void CopyButton_Click(object sender, EventArgs e)
        {
            copyButton.Clickable = false;

            await Clipboard.SetTextAsync(link.Text);
            Toast.MakeText(this, "Ссылка скопирована", ToastLength.Short).Show();

            copyButton.Clickable = true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}