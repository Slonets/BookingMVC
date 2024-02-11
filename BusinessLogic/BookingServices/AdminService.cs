using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class AdminService : IAdmin
    {
        private readonly IRepository<UserEntity> _userEntity;
        private readonly IMapper _mapper;
        public readonly IImageWorker _imageWorker;
        public AdminService(IRepository<UserEntity> userEntity, IMapper mapper, IImageWorker imageWorker)
        {
            _userEntity=userEntity;
            _mapper=mapper;
            _imageWorker=imageWorker;
        }
        public async Task<UserDto> GetId(int id)
        {
            var user = await _userEntity.GetByIDAsync(id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = _userEntity.GetAsync().Result.ToList();

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task Edit(UserDto userDto)
        {
            var value = _mapper.Map<UserEntity>(userDto);

            var user = await _userEntity.GetByIDAsync(value.Id);

            await _userEntity.UpdateAsync(user);
            await _userEntity.SaveAsync();
        }
        public async Task Delete(int id)
        {
            var seach = await _userEntity.GetByIDAsync(id);
            _imageWorker.RemoveImage(seach.Image);
            await _userEntity.DeleteAsync(seach);
            await _userEntity.SaveAsync();
        }

           
    }
}
