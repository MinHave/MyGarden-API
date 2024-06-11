using MyGarden_API.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyGarden_API.Models
{
    public class AuthRefreshToken
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string UserId { get; set; }

        public ApiUser User { get; set; }

        public bool Enabled { get; set; }
    }
}