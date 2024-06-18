using AutoMapper;
using MyGarden_API.Models.Entities;
using MyGarden_API.Repositories.Interfaces;
using MyGarden_API.Repositories;
using MyGarden_API.Services.Interfaces;
using MyGarden_API.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MyGarden_API.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryDesignPattern<ApiUser> _designPattern;
        private readonly IBaseService<ApiUser> _baseService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;

        public UserService(IRepositoryDesignPattern<ApiUser> designPattern, IMapper mapper, IBaseService<ApiUser> baseService, UserManager<ApiUser> userManager)
        {
            _designPattern = designPattern;
            _mapper = mapper;
            _baseService = baseService;
            _userManager = userManager;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {

            var result = await _designPattern.GetAll<ApiUser>(
               disabledCondition => disabledCondition.IsDisabled == false,
                false
                );



            return _mapper.Map<List<UserViewModel>>(result);
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            ApiUser user = await _designPattern.GetByCondition<ApiUser>(
                condition => condition.Id == userId,
                disabledCondition => true,
                false
            );

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<bool> ToggleUserDisabled(string userId)
        {
            ApiUser user = await _designPattern.GetByCondition<ApiUser>(
                condition => condition.Id == userId,
                disabledCondition => false,
                false
            );

            return await _baseService.ToggleDisabled(user);
        }
    }
}
