using System;
using System.Collections.Generic;
using System.IO;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using SQLite;
using SQLLib;

namespace RLink
{
    /// <summary>
    /// Активити сохраненных ссылок.
    /// </summary>
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class SavedActivity : Activity
    {
        /// <summary>
        /// Кнопка распознать новую ссылку.
        /// </summary>
        Button newLinkButton;

        /// <summary>
        /// Список сохраненных ссылок для отображения.
        /// </summary>
        ListView savedListView;

        /// <summary>
        /// Список сохраненных ссылок из базы данных.
        /// </summary>
        List<DBElem> savedList;

        /// <summary>
        /// Массив разрешений.
        /// </summary>
        readonly string[] permissionGroup =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.AccessNetworkState
        };

        /// <summary type="void" dos="protected">
        /// Обрабодчик создания Активити.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Выбираем соответствующий layout.
            SetContentView(Resource.Layout.saved);

            // Находим кнопку для распознования новой ссылки и добовляем обрабодчик нажатия.
            newLinkButton = FindViewById<Button>(Resource.Id.newLinkButton);
            newLinkButton.Click += NewLinkButton_Click;

            // Находим список ссылок и добовляем обрабодчик нажатия на элемент списка.
            savedListView = FindViewById<ListView>(Resource.Id.savedListView);
            savedListView.ItemClick += SavedListView_ItemClick;

            // Запрашиваем разрешения.
            RequestPermissions(permissionGroup, 0);
        }

        /// <summary type="void" dos="protected">
        /// Метод вызывающийся когда Активити становится активной.
        /// </summary>
        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                // Определяем путь к файлу с базой данных.
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
                // Подключаемся к базе даныых.
                var db = new SQLiteConnection(dbPath);
                // Создаем таблицу, если ее нет.
                db.CreateTable<DBElem>();
                // Сохраняем таблицу в список.
                savedList = db.Table<DBElem>().ToList();
                // Разворачиваем список.
                savedList.Reverse();
                db.Dispose();
                // Создаем свой адаптер и присваем его списку, так как у нас кастомный ListView.
                MyAdapter adapter = new MyAdapter(this, savedList);
                savedListView.Adapter = adapter;

                // Если сохраненных ссылок нет, то переходим к распознованию новой ссылки.
                if (savedList.Count == 0)
                {
                    Intent intent = new Intent(this, typeof(RecognizeActivity));
                    StartActivity(intent);
                }
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на элемент списка.
        /// </summary>
        private void SavedListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            try
            {
                // Создаем новую Активити.
                Intent intent = new Intent(this, typeof(ChangeActivity));

                // Получаем элемент списка.
                var item = savedList[e.Position];

                // Передаем информацию новой Активити.
                intent.PutExtra("loadId", item.Id);
                intent.PutExtra("loadName", item.Name);
                intent.PutExtra("loadLink", item.Link);
                intent.PutExtra("loadDescription", item.Description);

                // Запускаем новую Активити.
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку для распознования новой ссылки.
        /// </summary>
        private void NewLinkButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Вызываем новую Активити для распознования новой ссылки.
                Intent intent = new Intent(this, typeof(RecognizeActivity));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }

        /// <summary type="void" dos="public">
        /// Обрабодчик получения разрешений.
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            // Запрос разрешений.
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}