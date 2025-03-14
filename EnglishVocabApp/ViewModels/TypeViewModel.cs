using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EnglishVocabApp.ViewModels
{
    public class TypeViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "lblName")]
        [Remote("CheckNewTypeName", "ModelValidation", ErrorMessage ="lblTypeNameMustBeUnique")]
        public string Name { get; set; }

        public TypeViewModel() { }

        public TypeViewModel(Models.Type type) 
        {
            Id = type.Id;
            Name = type.Name;
        }
    }
}
