using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace RLink
{
    /// <summary>
    /// Адаптер списка распознанных ссылок.
    /// </summary>
    class MyAdapterLittle : BaseAdapter<string>
    {
        /// <summary>
        /// Список элементов.
        /// </summary>
        private readonly List<string> list;

        /// <summary>
        /// Контекст.
        /// </summary>
        private readonly Context context;

        /// <summary>
        /// Коснтрукор.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="list">Список элементов.</param>
        public MyAdapterLittle(Context context, List<string> list)
        {
            this.list = list;
            this.context = context;
        }

        /// <summary>
        /// Возвращет колличество элементов.
        /// </summary>
        public override int Count => list.Count;

        /// <summary>
        /// Индексатор.
        /// </summary>
        /// <param name="position">Номкр нужной позиции.</param>
        /// <returns>Элемент с этим номером.</returns>
        public override string this[int position] => list[position];

        /// <summary>
        /// Метод для получения отбображения элемента списка.
        /// </summary>
        /// <param name="position">Позиция</param>
        /// <param name="convertView">Отображение.</param>
        /// <param name="parent">Родитель.</param>
        /// <returns>Отображение элемента.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Заполним отображение если оно пусто.
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.list_view_little, null, false);

                TextView link = view.FindViewById<TextView>(Resource.Id.chooseTextView);
                link.Text = list[position];
            }
            return view;
        }

        /// <summary>
        /// Метод для получение позиции.
        /// </summary>
        /// <param name="position">Нужная позиция.</param>
        /// <returns>Эта же позиция.</returns>
        public override long GetItemId(int position) => position;
    }
}