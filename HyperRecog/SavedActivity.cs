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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SavedActivity : Activity
    {
        Button newLinkButton;

        ListView savedListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.saved);

            newLinkButton = FindViewById<Button>(Resource.Id.newLinkButton);
            newLinkButton.Click += NewLinkButton_Click;

            savedListView = FindViewById<ListView>(Resource.Id.savedListView);
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            List<DBElem> savedList = db.Table<DBElem>().ToList();
            MyAdapter adapter = new MyAdapter(this, savedList);
            savedListView.Adapter = adapter;
            savedListView.ItemClick += SavedListView_ItemClick;

            if (savedList.Count == 0)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }

        private void SavedListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            
        }

        private void NewLinkButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}