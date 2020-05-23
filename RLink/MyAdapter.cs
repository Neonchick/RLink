using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using SQLLib;

namespace RLink
{
    /// <summary>
    /// Адаптер сохраненных ссылок.
    /// </summary>
    class MyAdapter : BaseAdapter<DBElem>
    {
        /// <summary>
        /// Список элементов.
        /// </summary>
        private readonly List<DBElem> list;

        /// <summary>
        /// Контекст.
        /// </summary>
        private readonly Context context;

        /// <summary type="void" dos="public">
        /// Коснтрукор.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="list">Список элементов.</param>
        public MyAdapter(Context context, List<DBElem> list)
        {
            this.list = list;
            this.context = context;
        }

        /// <summary type="int" dos="public">
        /// Возвращет колличество элементов.
        /// </summary>
        public override int Count => list.Count;

        /// <summary>
        /// Индексатор.
        /// </summary>
        /// <param name="position">Номкр нужной позиции.</param>
        /// <returns>Элемент с этим номером.</returns>
        public override DBElem this[int position] => list[position];

        /// <summary type="View" dos="public">
        /// Метод для получения отбображения элемента списка.
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="convertView">Отображение.</param>
        /// <param name="parent">Родитель.</param>
        /// <returns>Отображение элемента.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Заполним отображение, если оно пусто.
            View view = LayoutInflater.From(context).Inflate(Resource.Layout.list_view, null, false);

            TextView name = view.FindViewById<TextView>(Resource.Id.nameTextView);
            name.Text = list[position].Name;

            TextView link = view.FindViewById<TextView>(Resource.Id.linkTextView);
            link.Text = list[position].Link;

            return view;
        }

        /// <summary type="long" dos="public">
        /// Метод для получение позиции.
        /// </summary>
        /// <param name="position">Нужная позиция.</param>
        /// <returns>Эта же позиция.</returns>
        public override long GetItemId(int position) => position;
    }
}