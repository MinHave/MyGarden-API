namespace MyGarden_API.ViewModels
{
    public class UserViewModel : SimpleUserViewModel
    {
        public DateTimeOffset Created { get; set; }
        
        public DateTimeOffset Updated { get; set; }
    }
}
