namespace EnglishVocabApp.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime? DateJoined { get; set; }
        public int TotalWordsLearned { get; set; }

        public UserViewModel() { }

        public UserViewModel(Models.User user)
        {
            Id = user.Id;
            UserName = user.UserName;
            DateJoined = user.DateJoined;
            TotalWordsLearned = user.TotalWordsLearned ?? 0;
        }
    }
}
