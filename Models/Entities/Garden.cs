using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyGarden_API.Models.Entities
{
    public class Garden : IDisabledEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public List<Plant> Plants { get; set; }

        [ForeignKey("gardenowner")]
        public ApiUser GardenOwner {  get; set; }

        public bool IsDisabled { get; set; }
    }
}
