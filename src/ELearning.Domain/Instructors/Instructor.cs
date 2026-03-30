using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ELearning.Domain.Shared;

namespace ELearning.Domain.Instructors;
public sealed class Instructor : BaseEntity
{
    private Instructor() { }
    public Instructor(
        string id,
        Bio bio,
        Rating rating) : base(id)
    {
        Bio = bio;
        Rating = rating;
    }
    public Bio Bio { get; private set; }
    public Rating Rating { get; private set; }

    public void UpdateBio(Bio bio)
    {
        if (bio is not null && !string.IsNullOrWhiteSpace(bio.Value))
        {
            Bio = bio;
            return;
        }

        throw new ApplicationException("Bio can't be null or empty");
    }

}
