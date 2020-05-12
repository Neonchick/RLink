using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using SQLite;
using SQLLib;
using Xamarin.Essentials;

namespace RLink
{
    /// <summary>
    /// Активити изменения сохраненнной ссылки.
    /// </summary>
    [Activity(Label = "LoadActivity", Theme = "@style/AppTheme")]
    public class ChangeActivity : Activity
    {
        // ID ссылки.
        int id;

        /// <summary>
        /// Текстовое поле имени.
        /// </summary>
        EditText name;
        /// <summary>
        /// Текстовое поле ссылки.
        /// </summary>
        EditText link;
        /// <summary>
        /// Текстовое поле описания.
        /// </summary>
        EditText description;

        /// <summary>
        /// Кнопка копировать.
        /// </summary>
        Button copyButton;
        /// <summary>
        /// Кнопка перейти.
        /// </summary>
        Button goToLinkButton;
        /// <summary>
        /// Кнопка поделится.
        /// </summary>
        Button shareButton;
        /// <summary>
        /// Кнопка сохранить.
        /// </summary>
        Button saveButton;
        /// <summary>
        /// Кнопка удалить.
        /// </summary>
        Button deleteButton;

        /// <summary>
        /// Флаг нажатия кнопки.
        /// </summary>
        bool buttonFlag = false;

        /// <summary>
        /// Обрабодчик создания активити.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Найдим подходящий layout.
            SetContentView(Resource.Layout.load_link);

            // Получим id.
            id = Intent.GetIntExtra("loadId", 0);

            // Найдем и присвоем значение текстовому полю ссылки.
            link = FindViewById<EditText>(Resource.Id.loadLinkEditText);
            link.Text = Intent.GetStringExtra("loadLink");

            // Найдем и присвоем значение текстовому полю имени.
            name = FindViewById<EditText>(Resource.Id.loadNameEditText);
            name.Text = Intent.GetStringExtra("loadName");

            // Найдем и присвоем значение текстовому полю описания.
            description = FindViewById<EditText>(Resource.Id.loadDescriptionEditText);
            description.Text = Intent.GetStringExtra("loadDescription");

            // Найдем кнопку скопировать и добавим обрабодчик нажатия.
            copyButton = FindViewById<Button>(Resource.Id.loadCopyButton);
            copyButton.Click += CopyButton_Click;

            // Найдем кнопку перейти и добавим обрабодчик нажатия.
            goToLinkButton = FindViewById<Button>(Resource.Id.loadGoToLinkButton);
            goToLinkButton.Click += GoToLinkButton_Click;

            // Найдем кнопку поделиться и добавим обрабодчик нажатия.
            shareButton = FindViewById<Button>(Resource.Id.loadShareButton);
            shareButton.Click += ShareButton_Click;

            // Найдем кнопку сохранить и добавим обрабодчик нажатия.
            saveButton = FindViewById<Button>(Resource.Id.loadSaveButton);
            saveButton.Click += SaveButton_Click;

            // Найдем кнопку удалить и добавим обрабодчик нажатия.
            deleteButton = FindViewById<Button>(Resource.Id.loadDeleteButton);
            deleteButton.Click += DeleteButton_Click;
        }

        /// <summary>
        /// Обрабодчик нажатия на кнопку удалить.
        /// </summary>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // Только одна кнопка может быть нажата.
            if (buttonFlag)
                return;

            buttonFlag = true;

            try
            {
                // Находим путь к базе данных.
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
                // Подключение к бд.
                var db = new SQLiteConnection(dbPath);
                // Создаем таблицу.
                db.CreateTable<DBElem>();
                // Удаляем элемент.
                db.Delete(id, db.GetMapping<DBElem>());
                // Уничтожаем объект.
                db.Dispose();
                // Заканчиваем активити.
                this.Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary>
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
                // Если текстовое поле имени пустое заменим его на "Без названия"
                if (name.Text == "")
                    name.Text = "Без названия";

                // Находим путь к базе данных.
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
                // Полключимся к базе данных.
                var db = new SQLiteConnection(dbPath);
                // Создадим базу данных.
                db.CreateTable<DBElem>();
                // Удалим эдемент.
                db.Delete(id, db.GetMapping<DBElem>());
                // Создадим новый элемент.
                DBElem linkDB = new DBElem(name.Text, link.Text, description.Text);
                // Вставим его в базу данных.
                db.Insert(linkDB);
                // Уничтожаем объект.
                db.Dispose();
                // Завершаем ативити.
                this.Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }

            buttonFlag = false;
        }

        /// <summary>
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

        /// <summary>
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

        /// <summary>
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