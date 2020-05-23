using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using SQLLib;

namespace RLink
{
    /// <summary>
    /// Активити сохранения новой ссылки.
    /// </summary>
    [Activity(Label = "SaveNewLinkActivity", Theme = "@style/AppTheme")]
    public class SaveNewLinkActivity : Activity
    {
        /// <summary>
        /// Кнопка сохранить.
        /// </summary>
        Button saveButton;

        /// <summary>
        /// Тесктовое поле имени.
        /// </summary>
        EditText name;
        /// <summary>
        /// Тесктовое поле ссылки.
        /// </summary>
        EditText link;
        /// <summary>
        /// Тесктовое поле описания.
        /// </summary>
        EditText description;

        /// <summary type="void" dos="protected">
        /// Обрабодчик создания Активити.
        /// </summary>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Выбираем соответствующий layout.
            SetContentView(Resource.Layout.save_new_link);

            // Находим кнопку сохранения и добовляем обрабодчик нажатия.
            saveButton = FindViewById<Button>(Resource.Id.saveNewLinkButton);
            saveButton.Click += SaveButton_Click;

            // Назодим все текстовые поля.
            link = FindViewById<EditText>(Resource.Id.saveLinkEditText);
            name = FindViewById<EditText>(Resource.Id.nameSaveLinkEditText);
            description = FindViewById<EditText>(Resource.Id.saveDescriptionEditText);

            // Присваем текстовому полю ссылки распознанную ссылку.
            link.Text = Intent.GetStringExtra("saveNewLink");
        }

        /// <summary type="void" dos="private">
        /// Обрабодчик нажатия на кнопку сохранить.
        /// </summary>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // Кнопка должна быть нажата лиш раз.
            saveButton.Clickable = false;
            // Заменяем пустое название на "Без названия".
            if (name.Text == "")
                name.Text = "Без названия";
            try
            {
                // Находим путь к базе данных.
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "links.db3");
                // Подключимся к бд.
                var db = new SQLiteConnection(dbPath);
                // Создадим таблицу.
                db.CreateTable<DBElem>();
                // Созддадим новый элемент и добавим новый элемент.
                DBElem linkDB = new DBElem(name.Text, link.Text, description.Text);
                db.Insert(linkDB);
                db.Dispose();
                // Завершим активити.
                this.Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
            saveButton.Clickable = true;
        }
    }
}