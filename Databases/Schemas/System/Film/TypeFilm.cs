using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases;

namespace CapstoneProject.Databases.Schemas.System.Film
{
    [Table("TypeFilm")]
    public partial class TypeFilm : TableHaveIdInt, ITable
    {
		public TypeFilm()
		{
		}
        /// <summary>
        /// Tên thể loại film
        /// </summary>
        public string Name { get; set; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

    }
}

