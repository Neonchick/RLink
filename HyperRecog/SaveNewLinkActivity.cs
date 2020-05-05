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

namespace HyperRecog
{
    [Activity(Label = "SaveNewLinkActivity", Theme = "@style/AppTheme")]
    public class SaveNewLinkActivity : Activity
    {
        Button saveButton;

        EditText name;
        EditText link;
        EditText description;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.save_new_link);

            saveButton = FindViewById<Button>(Resource.Id.saveNewLinkButton);
            saveButton.Click += SaveButton_Click;

            link = FindViewById<EditText>(Resource.Id.saveLinkEditText);
            name = FindViewById<EditText>(Resource.Id.nameSaveLinkEditText);
            description = FindViewById<EditText>(Resource.Id.saveDescriptionEditText);

            link.Text = Intent.GetStringExtra("saveNewLink");
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            saveButton.Clickable = false;
            if (name.Text == "")
                name.Text = "Без названия";
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            DBElem linkDB = new DBElem(name.Text, link.Text, description.Text);
            db.Insert(linkDB);
            this.Finish();
            saveButton.Clickable = true;
        }
    }
}