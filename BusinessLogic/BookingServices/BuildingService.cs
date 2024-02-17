using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class BuildingService : IBuilding
    {
        private readonly IRepository<BuildingEntity> _buildingEntity;
        private readonly IMapper _mapper;
        public readonly IImageWorker _imageWorker;

        public BuildingService(IRepository<BuildingEntity> buildingEntity, IMapper mapper, IImageWorker imageWorker)
        {
            _buildingEntity = buildingEntity;
            _mapper = mapper;
            _imageWorker = imageWorker;
        }

        public async Task<BuildingDto> GetId(int id)
        {
            var building = await _buildingEntity.GetByIDAsync(id);

            return _mapper.Map<BuildingDto>(building);
        }
        public async Task<ICollection<BuildingDto>> GetAll()
        {
            var buildings = await _buildingEntity.GetAsync();
            
            return _mapper.Map<List<BuildingDto>>(buildings);
        }

        public async Task Create(BuildingDto buildingDto)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(BuildingDto buildingDto)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }        
    }
}
