namespace EnglishVocabApp.Models
{
    public class FoldersUsers
    {
        public int FolderId { get; set; }
        public string UserId { get; set; }      // string, because IdentityUser that is inherited by User has Id of type string


        public Folder Folder { get; set; }      // navigational properties
        public User User { get; set; }
    }
}
