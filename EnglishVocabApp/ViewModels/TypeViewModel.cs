using System.ComponentModel.DataAnnotations;

namespace EnglishVocabApp.ViewModels
{
    public class TypeViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "lblName")]
        public string Name { get; set; }

        public TypeViewModel() { }

        public TypeViewModel(Models.Type type) 
        {
            Id = type.Id;
            Name = type.Name;
        }
    }
}
