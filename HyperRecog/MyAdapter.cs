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
using SQLLib;

namespace HyperRecog
{
    class MyAdapter: BaseAdapter<DBElem>
    {
        private List<DBElem> list;

        private Context context;

        public MyAdapter(Context context, List<DBElem> list)
        {
            this.list = list;
            this.context = context;
        }

        public override int Count => list.Count;

        public override DBElem this[int position] => list[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.list_view, null, false);
                
                TextView name = view.FindViewById<TextView>(Resource.Id.nameTextView);
                name.Text = list[position].Name;

                TextView link = view.FindViewById<TextView>(Resource.Id.linkTextView);
                link.Text = list[position].Link;
            }
            return view;
        }

        public override long GetItemId(int position) => position;
    }
}