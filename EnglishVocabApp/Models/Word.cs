using System.ComponentModel.DataAnnotations.Schema;

namespace EnglishVocabApp.Models
{
    public class Word : BaseModelClass
    {
        public string Name { get; set; }
        
        public int TypeId {  get; set; }           // type of word: noun, adjective
        public string Transcript { get; set; }     // one string for all transcripts: uk - pronunciation1, us - pronunciation2

        //public List<(string Meaning, string Example)> MeaningsAndExamples { get; set; }       // meaning of the word - example of use in sentence

        public List <string> Synonyms { get; set; }
        public List <string> Antonyms { get; set; }


        public Type Type { get; set; }                                   // navigational property for one-to-many relationship between Type and Word
        [NotMapped]
        public IEnumerable<WordsFolders> WordsFolders { get; set; } = new List<WordsFolders>();      // navigational property for many-to-many relationship between Word and Folder
    }
}
