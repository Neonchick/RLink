using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;

namespace RLink
{
    /// <summary>
    /// Активити выбора ссылки.
    /// </summary>
    [Activity(Label = "ChooseActivity", Theme = "@style/AppTheme")]
    public class ChooseActivity : Activity
    {
        /// <summary>
        /// Отображение списка ссылок.
        /// </summary>
        ListView linkListView;

        /// <summary>
        /// Список ссылок.
        /// </summary>
        List<string> linkList;

        /// <summary>
        /// Обрабодчик создания активити.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Находим подходящий layout.
            SetContentView(Resource.Layout.choose);
            // Находим список.
            linkList = Intent.GetStringArrayExtra("linkList").ToList<string>();
            linkListView = (ListView)FindViewById(Resource.Id.linkListView);
            // Создаем свой адаптер.
            MyAdapterLittle adapter = new MyAdapterLittle(this, linkList);
            // Присваем адаптер списку и добовляем обрабодчик нажатия на элемент.
            linkListView.Adapter = adapter;
            linkListView.ItemClick += LinkListView_ItemClick;
        }

        /// <summary>
        /// Обрабодчик нажатия на элемент списка.
        /// </summary>
        private void LinkListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                // Запускаем новую активити с выбранной ссылкой. 
                Intent intent = new Intent(this, typeof(ResultActivity));
                intent.PutExtra("recognizedLink", linkList[e.Position]); ;
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
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