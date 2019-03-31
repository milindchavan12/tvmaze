namespace TvMaze.Persistence.Models
{
    /// <summary>
    /// Represents the many-to-many relationship between <see cref="Show"/>s and <see cref="Person"/>s.
    /// </summary>
    public class Cast
    {
        /// <summary>
        /// Gets or sets the show identifier.
        /// </summary>
        public int ShowId { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Show"/>.
        /// </summary>
        public Show Show { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Person"/> casted in this <see cref="Show"/>.
        /// </summary>
        public int CastPersonId { get; set; }

        /// <summary>
        /// Gets or sets the cast member.
        /// </summary>
        public Person Person { get; set; }
    }
}
