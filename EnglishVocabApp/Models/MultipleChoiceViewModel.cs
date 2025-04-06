using System.Collections.Generic;

namespace EnglishVocabApp.ViewModels
{
    public class MultipleChoiceViewModel
    {
        public int FolderId { get; set; }
        public int WordId { get; set; }
        public string WordText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }
}