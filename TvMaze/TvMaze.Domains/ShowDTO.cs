using System.Collections.Generic;

namespace TvMaze.Domains.DTO
{
    public class ShowDto
    {
	    public ShowDto()
	    {
		    Cast = new List<PersonDto>();
	    }

		public int Id { get; set; }

        public string Name { get; set; }

        public List<PersonDto> Cast { get; set; }

        public override string ToString()
        {
            return $"Name: {Name} - ID: {Id}";
        }
    }
}