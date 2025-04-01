using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EnglishVocabApp.Controllers;
using EnglishVocabApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishVocabApp.ViewModels
{
    public class WordViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "lblNameOfTheWordIsRequired")]
        [StringLength(45, MinimumLength = 1, ErrorMessage = "lblNameLengthMessage")]
        [RegularExpression(@"^[a-zA-Z0-9\s.,!?'"":;_()-]+$", ErrorMessage = "Only English letters, numbers, and common punctuation are allowed.")]

        public string Name { get; set; }

        [Required(ErrorMessage = "lblTranscriptOfTheWordIsRequired")]
        [StringLength(50, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z0-9\s.,!?'"":;_()\-ˈˌːɑæɔəɪʊʌθðŋ]+$", ErrorMessage = "Only English letters, numbers, common punctuation, and transcription symbols are allowed.")]
        public string Transcript { get; set; }

        [Required(ErrorMessage = "lblMeaningOfTheWordIsRequired")]
        [StringLength(200, MinimumLength = 5)]
        [Remote("CheckNewMeaning", "ModelValidation", AdditionalFields = "Id", ErrorMessage = "lblMeaningMustBeUnique")]
        [RegularExpression(@"^[a-zA-Z0-9\s.,!?'"":;_()-]+$", ErrorMessage = "Only English letters, numbers, and common punctuation are allowed.")]

        public string Meaning { get; set; }

        public string ExamplesString { get; set; }
        public string SynonymsString { get; set; }
        public string AntonymsString { get; set; }


        [Remote("Validate", "ModelValidation")]
        //[RequiredList(ErrorMessage = "At least one example is required.")]
        public List<string> Examples { get; set; } = new List<string>();

        [Remote("Validate", "ModelValidation")]
        public List<string> Synonyms { get; set; } = new List<string>();

        [Remote("Validate", "ModelValidation")]
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

                        // Set default phrases if lists are empty
            if (!Examples.Any())
            {
                Examples.Add("no examples available.");
            }

            if (!Synonyms.Any())
            {
                Synonyms.Add("no synonyms available.");
            }

            if (!Antonyms.Any())
            {
                Antonyms.Add("no antonyms available.");
            }

            TypeName = word.Type?.Name;
        }
    }
}
