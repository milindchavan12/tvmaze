using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvMaze.Persistence.Models
{
    /// <summary>
    /// A cast member.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Gets or sets the member identifier.
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
        /// Gets or sets the birthdate.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Gets the cast of this Show.
        /// </summary>
        public List<Cast> Cast { get; } = new List<Cast>();

        public override string ToString()
        {
            return $"ID: {Id} - {Name}, {Birthdate}";
        }
    }
}