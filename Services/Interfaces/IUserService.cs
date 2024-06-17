using MyGarden_API.ViewModels;

namespace MyGarden_API.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserViewModel>> GetUsers();

        public Task<UserViewModel> GetUserById(string userId);

        public Task<bool> ToggleUserDisabled(string userId);
    }
}
