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
using SQLite;

namespace SQLLib
{
    [Table("Items")]
    public class DBElem
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public DBElem() { }

        public DBElem(string name, string link, string description)
        {
            Name = name;
            Link = link;
            Description = description;
        }
    }
}