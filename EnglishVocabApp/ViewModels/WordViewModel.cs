using System.Collections.Generic;
using System.Linq;
using EnglishVocabApp.Models;

namespace EnglishVocabApp.ViewModels
{
    public class WordViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Transcript { get; set; }
        public string Meaning { get; set; }
        public string ExamplesString { get; set; }
        public string SynonymsString { get; set; }
        public string AntonymsString { get; set; }

        public List<string> Examples { get; set; } = new List<string>();
        public List<string> Synonyms { get; set; } = new List<string>();
        public List<string> Antonyms { get; set; } = new List<string>();

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
