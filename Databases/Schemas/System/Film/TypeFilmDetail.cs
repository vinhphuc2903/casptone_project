using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;

namespace CapstoneProject.Databases.Schemas.System.Film
{
    [Table("TypeFilmDetail")]
    public partial class TypeFilmDetail : ITable
    {
        /// <summary>
        /// Mã film
        /// </summary>
        public int FilmId { get; set; }
        /// <summary>
        /// Mã thể loại
        /// </summary>
        public int TypeFilmId { get; set; }
        
        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual Films? Films { get; set; }

        public virtual TypeFilm? TypeFilms { get; set; }
    }
}

