using SQLite;

namespace SQLLib
{
    /// <summary>
    /// Класс элемента базы данных.
    /// </summary>
    [Table("Items")]
    public class DBElem
    {
        /// <summary>
        /// Уникальный номер.
        /// </summary>
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        /// <summary>
        /// Имя.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ссылка.
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Описание.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public DBElem() { }
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя.</param>
        /// <param name="link">Ссылка.</param>
        /// <param name="description">Описание.</param>
        public DBElem(string name, string link, string description)
        {
            Name = name;
            Link = link;
            Description = description;
        }
    }
}