public class User:BaseModelClass
{
    public int UserId { get; set; } // Унікальний ідентифікатор користувача
    public string Username { get; set; } // Ім’я користувача
    public string Email { get; set; } // Адреса електронної пошти
    public string PasswordHash { get; set; } // Хешований пароль
    public DateTime DateJoined { get; set; } // Дата створення облікового запису

    // Колекція папок, створених користувачем
    public List<Folder> CreatedFolders { get; set; } = new List<Folder>();

    // Колекція вивчених слів
    public List<Word> LearnedWords { get; set; } = new List<Word>();

    // Загальна кількість вивчених слів
    public int TotalWordsLearned => LearnedWords.Count;
}


