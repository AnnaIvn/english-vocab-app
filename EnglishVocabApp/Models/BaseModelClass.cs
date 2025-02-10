using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnglishVocabApp.Models
{
    public abstract class BaseModelClass: IEquatable<BaseModelClass>
    {
        public int Id { get; set; }

        bool IEquatable<BaseModelClass>.Equals(BaseModelClass? other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Id == other.Id)
            {
                return true;
            }

            return false;
        }
    }
}
