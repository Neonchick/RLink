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
    class MyAdapterLittle : BaseAdapter<string>
    {
        private List<string> list;

        private Context context;

        public MyAdapterLittle(Context context, List<string> list)
        {
            this.list = list;
            this.context = context;
        }

        public override int Count => list.Count;

        public override string this[int position] => list[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(context).Inflate(Resource.Layout.list_view_little, null, false);

                TextView link = view.FindViewById<TextView>(Resource.Id.chooseTextView);
                link.Text = list[position];
            }
            return view;
        }

        public override long GetItemId(int position) => position;
    }
}