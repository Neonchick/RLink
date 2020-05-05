using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HyperRecog
{
    [Activity(Label = "ChooseActivity", Theme = "@style/AppTheme")]
    public class ChooseActivity : Activity
    {
        ListView linkListView;

        List<string> linkList;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.choose);

            linkList = Intent.GetStringArrayExtra("linkList").ToList<string>();
            linkListView = (ListView)FindViewById(Resource.Id.linkListView);
            MyAdapterLittle adapter = new MyAdapterLittle(this, linkList);
            linkListView.Adapter = adapter;
            linkListView.ItemClick += LinkListView_ItemClick;
        }

        private void LinkListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(ResultActivity));
            intent.PutExtra("recognizedLink", linkList[e.Position]); ;
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}