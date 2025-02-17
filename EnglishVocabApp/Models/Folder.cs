using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishVocabApp.Models
{
    public class Folder
    {
        public int Id { get; set; }              // Ідентифікатор папки
        public string Name { get; set; }         // Назва папки
        public string Description { get; set; }  // Опис папки або категорія
        public string UserId { get; set; }       // Ідентифікатор користувача, якому належить папка, string because User's Id is string
        public DateTime CreatedAt { get; set; }  // Дата створення папки
        public DateTime UpdatedAt { get; set; }  // Дата останнього оновлення
        public bool IsPrivate { get; set; }      // Чи є папка приватною


        public User User { get; set; }                                 // navigational property
        [NotMapped]                                                    // 
        public IEnumerable<WordsFolders> WordsFolders { get; set; }    // navigational property for many-to-many relationship between Word and Folder
        [NotMapped]
        public IEnumerable<FoldersUsers> FoldersUsers { get; set; }    // navigational property for many-to-many relationship between Folder and User
    }
}
