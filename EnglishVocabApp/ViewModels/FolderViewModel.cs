using EnglishVocabApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EnglishVocabApp.ViewModels
{
    public class FolderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "lblEnterSomethingFolder")]
        [Display(Name = "lblNameFolder")]
        [StringLength(45, MinimumLength = 2, ErrorMessage = "lblNameLengthFolder")]
        public string Name { get; set; }

        [Required(ErrorMessage = "lblEnterSomethingFolder")]
        [Display(Name = "lblDescriptionFolder")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "lblDescriptionLengthFolder")]
        [Remote("CheckNewDescription", "ModelValidation", AdditionalFields = "Id", ErrorMessage = "lblDescriptionMustBeUnique")]
        public string Description { get; set; }

        [BindNever]
        public string? UserId { get; set; }

        [BindNever]
        [Display(Name = "lblUserNameFolder")]
        public User? User { get; set; }

        [Display(Name = "lblCreatedAtFolder")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "lblUpdatedAtFolder")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "lblIsPrivateFolder")]
        public bool IsPrivate { get; set; }

        public IEnumerable<FoldersUsers>? FoldersUsers { get; set; }

        public IEnumerable<WordsFolders>? WordsFolders { get; set; }

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
