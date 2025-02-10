namespace EnglishVocabApp.Models
{
    public class WordsFolders
    {
        public int WordId { get; set; }
        public int FolderId { get; set; }


        public Word Word { get; set; }          // navigational properties
        public Folder Folder { get; set; }
    }
}