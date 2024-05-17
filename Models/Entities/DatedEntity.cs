namespace MyGarden_API.Models.Entities
{
    public class DatedEntity : IDatedEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }
    }
}
