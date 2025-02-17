using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EnglishVocabApp.Models
{
    public class User : IdentityUser
    {
        public DateTime? DateJoined { get; set; }                                 // Дата створення облікового запису

        public List<Folder> CreatedFolders { get; set; } = new List<Folder>();    // Колекція папок, створених користувачем

        public List<Word> LearnedWords { get; set; } = new List<Word>();          // Колекція вивчених слів

        public int TotalWordsLearned => LearnedWords?.Count ?? 0;                 // Загальна кількість вивчених слів   // will work even if it is null
                                                                                  // public string PasswordHash { get; set; } // Хешований пароль  // password is in User, i'll coment this for now
                                                                                  // ? - to be able for these fields to be null, because they aren't implemented right now in any way

        [NotMapped]
        public IEnumerable<FoldersUsers> FoldersUsers { get; set; }               // navigational property for many-to-many relationship between Folder and User
    }
}


