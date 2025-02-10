public class Folder
{
    public int Id { get; set; } // Ідентифікатор папки
    public string Name { get; set; } // Назва папки
    public string Description { get; set; } // Опис папки або категорія
    public int UserId { get; set; } // Ідентифікатор користувача, якому належить папка
    public DateTime CreatedAt { get; set; } // Дата створення папки
    public DateTime UpdatedAt { get; set; } // Дата останнього оновлення
    public bool IsPrivate { get; set; } // Чи є папка приватною

    // Посилання на власника папки
    public User User { get; set; }
}
