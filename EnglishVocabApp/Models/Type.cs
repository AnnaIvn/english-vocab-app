using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishVocabApp.Models
{
    public class Type : BaseModelClass
    {
        public string Name { get; set; }
        public ICollection<Word> Words { get; set; } = new List<Word>();
    }
}
