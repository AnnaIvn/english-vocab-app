using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EnglishVocabApp.Models;

namespace EnglishVocabApp.ViewModels
{
    public class WordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "lblNameOfTheWordIsRequired")]
        [StringLength(45, MinimumLength = 1, ErrorMessage = "lblNameLengthMessage")]
        public string Name { get; set; }

        [Required(ErrorMessage = "lblTranscriptOfTheWordIsRequired")]
        [StringLength(50, MinimumLength = 1)]
        public string Transcript { get; set; }

        [Required(ErrorMessage = "lblMeaningOfTheWordIsRequired")]
        [StringLength(200, MinimumLength = 5)]
        public string Meaning { get; set; }
        public string ExamplesString { get; set; }
        public string SynonymsString { get; set; }
        public string AntonymsString { get; set; }

        public List<string> Examples { get; set; } = new List<string>();
        public List<string> Synonyms { get; set; } = new List<string>();
        public List<string> Antonyms { get; set; } = new List<string>();

        [Required(ErrorMessage = "lblTypeOfTheWordIsRequired")]
        public string TypeName { get; set; }

        public WordViewModel() { }

        public WordViewModel(Word word)
        {
            Id = word.Id;
            Name = word.Name;
            Transcript = word.Transcript;
            Meaning = word.Meaning;
            ExamplesString = word.ExamplesString;
            SynonymsString = word.SynonymsString;
            AntonymsString = word.AntonymsString;

            Examples = ExamplesString?.Split(';').Select(s => s.Trim()).ToList() ?? new List<string>();
            Synonyms = SynonymsString?.Split(',').Select(s => s.Trim()).ToList() ?? new List<string>();
            Antonyms = AntonymsString?.Split(',').Select(a => a.Trim()).ToList() ?? new List<string>();

            TypeName = word.Type?.Name;
        }
    }
}
