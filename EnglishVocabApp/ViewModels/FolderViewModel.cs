using EnglishVocabApp.Models;
using System.ComponentModel.DataAnnotations;

namespace EnglishVocabApp.ViewModels
{
    public class FolderViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "lblNameFolder")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "lblDescriptionFolder")]
        public string Description { get; set; }
        public string UserId { get; set; }
        [Required]
        [Display(Name = "lblUserNameFolder")]
        public User User { get; set; }
        [Required]
        [Display(Name = "lblCreatedAtFolder")]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Display(Name = "lblUpdatedAtFolder")]
        public DateTime UpdatedAt { get; set; }
        [Required]
        [Display(Name = "lblIsPrivateFolder")]
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
