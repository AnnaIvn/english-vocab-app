using EnglishVocabApp.Models;

namespace EnglishVocabApp.ViewModels
{
    public class FolderViewModel
    {
        public int Id { get; set; }              
        public string Name { get; set; }         
        public string Description { get; set; }  
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
        public bool IsPrivate { get; set; }

        public IEnumerable<FoldersUsers> FoldersUsers { get; set; }
        public IEnumerable<WordsFolders> WordsFolders { get; set; }

        public FolderViewModel() { }

        public FolderViewModel(Models.Folder folder)
        {
            Id = folder.Id;
            Name = folder.Name;
            Description = folder.Description;
            UserId = folder.UserId;
            User = folder.User;
            CreatedAt = folder.CreatedAt;
            UpdatedAt = folder.UpdatedAt;
            IsPrivate = folder.IsPrivate;
        }
    }
}
