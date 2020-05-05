using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android;
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

        List<DBElem> savedList;

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
            SetContentView(Resource.Layout.saved);

            newLinkButton = FindViewById<Button>(Resource.Id.newLinkButton);
            newLinkButton.Click += NewLinkButton_Click;

            savedListView = FindViewById<ListView>(Resource.Id.savedListView);
            savedListView.ItemClick += SavedListView_ItemClick;

            RequestPermissions(permissionGroup, 0);
        }

        protected override void OnResume()
        {
            base.OnResume();
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<DBElem>();
            savedList = db.Table<DBElem>().ToList();
            savedList.Reverse();
            MyAdapter adapter = new MyAdapter(this, savedList);
            savedListView.Adapter = adapter;
            if (savedList.Count == 0)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
        }

        private void SavedListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(LoadActivity));
            var item = savedList[e.Position];
            intent.PutExtra("loadId", item.Id);
            intent.PutExtra("loadName", item.Name);
            intent.PutExtra("loadLink", item.Link);
            intent.PutExtra("loadDescription", item.Description);
            StartActivity(intent);
        }

        private void NewLinkButton_Click(object sender, EventArgs e)
        {
            newLinkButton.Clickable = false;

            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);

            newLinkButton.Clickable = true;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}