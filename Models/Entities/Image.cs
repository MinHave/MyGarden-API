using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyGarden_API.Models.Entities
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid id { get; set; }

        public string ImageLocation { get; set; }

        public DateTimeOffset lastUpdated { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public DateTime imageTaken { get; set; }

        public string fileName { get; set; }
    }
}
