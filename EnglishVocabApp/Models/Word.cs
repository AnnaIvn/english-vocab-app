namespace EnglishVocabApp.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        // a new class must be created named Type
        public int TypeId {  get; set; }           // type of word: noun, adjective
        public string Transcript { get; set; }     // one string for all transcripts: uk - pronunciation1, us - pronunciation2

        public List<(string Meaning, string Example)> MeaningsAndExamples { get; set; }       // meaning of the word - example of use in sentence

        public List <string> Synonyms { get; set; }
        public List <string> Antonyms { get; set; }

        public Type Type { get; set; }        // navigational property
    }
}
