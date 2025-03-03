namespace EnglishVocabApp.ViewModels
{
    public class TypeViewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }

        public TypeViewModel() { }

        public TypeViewModel(Models.Type type) 
        {
            Id = type.Id;
            Name = type.Name;
        }

    }
}
