using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CapstoneProject.Databases.Schemas.System;

namespace CapstoneProject.Databases.Schemas.System.Users
{
    [Table("UserTokens")]
    public partial class UserToken : TableHaveIdInt, ITable
    {
        public int UserId { set; get; }

        [StringLength(500)]
        public string? JwtToken { set; get; }

        [StringLength(500)]
        public string? RefreshToken { set; get; }

        [StringLength(500)]
        public string? ValidateToken { set; get; }

        [StringLength(30)]
        public string? LatOfMap { get; set; }

        [StringLength(30)]
        public string? LongOfMap { get; set; }

        [StringLength(200)]
        public string? Browser { get; set; }

        [StringLength(100)]
        public string? PlayerId { get; set; }

        [StringLength(50)]
        public string? Serial { get; set; }

        public int LoginFrom { set; get; }

        public DateTimeOffset? Timeout { set; get; }

        public DateTimeOffset CreatedAt { set; get; }

        public int CreatedBy { set; get; }

        [StringLength(50)]
        public string? CreatedIp { set; get; }

        public DateTimeOffset? UpdatedAt { set; get; }

        public int? UpdatedBy { set; get; }

        [StringLength(50)]
        public string? UpdatedIp { set; get; }

        public bool DelFlag { set; get; }

        public virtual User User { set; get; }
    }
}
