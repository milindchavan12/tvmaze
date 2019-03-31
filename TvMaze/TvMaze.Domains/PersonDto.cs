using System;
using System.Collections.Generic;

namespace TvMaze.Domains.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthdate { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} - {Name}, {Birthdate}";
        }
    }

    public class PersonDtoEqualityComparer : IEqualityComparer<PersonDto>
    {
        public bool Equals(PersonDto x, PersonDto y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(PersonDto obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}