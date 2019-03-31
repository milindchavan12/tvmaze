using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMaze.Persistence.Models
{
    /// <summary>
    /// Detail for a Show
    /// </summary>
    public class Show
    {
        public Show()
        {
            Cast = new List<Cast>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets the list for this show's cast.
        /// </summary>
        public List<Cast> Cast { get; set; }
    }
}