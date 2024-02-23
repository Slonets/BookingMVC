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

            // Використовуємо GetIQueryable для завантаження пов'язаних об'єктів
            await _buildingEntity.GetIQueryable()
                .Include(x => x.ImagesBulding)
                .Include(x => x.ViewOfTheHouse)
                .Include(x => x.TypeOfSale)
                .Include(x => x.UserEntity)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<BuildingDto>(building);
        }
        public async Task<ICollection<BuildingDto>> GetAll()
        {
            var buildings = await _buildingEntity.GetIQueryable()
                .Include(x=>x.ImagesBulding)                 
                .Include(x=>x.ViewOfTheHouse)                
                .Include(x=>x.TypeOfSale)                
                .ToListAsync();

            return _mapper.Map<List<BuildingDto>>(buildings);
        }

        public async Task<ICollection<BuildingDto>> AllFlat()
        {
            var buildings = await _buildingEntity.GetIQueryable()
                .Include(x => x.ImagesBulding)
                .Include(x => x.ViewOfTheHouse)
                .Include(x => x.TypeOfSale)
                .Where(x=>x.ViewOfTheHouse.Name=="Квартира")
                .ToListAsync();

            return _mapper.Map<List<BuildingDto>>(buildings);
        }

        public async Task<ICollection<BuildingDto>> AllHouse()
        {
            var buildings = await _buildingEntity.GetIQueryable()
                .Include(x => x.ImagesBulding)
                .Include(x => x.ViewOfTheHouse)
                .Include(x => x.TypeOfSale)
                .Where(x => x.ViewOfTheHouse.Name == "Будинок")
                .ToListAsync();

            return _mapper.Map<List<BuildingDto>>(buildings);
        }

        public async Task Create(BuildingDto buildingDto)
        {
            // Ініціалізуємо список для зберігання шляхів до зображень
            buildingDto.ImagesBulding = new List<string>();

            foreach (var file in buildingDto.Image)
            {
                    // Зберігаємо кожен файл і додаємо шлях до списку
                    string imagePath = _imageWorker.ImageSave(file);
                    buildingDto.ImagesBulding.Add(imagePath);
            }          

            // Видаляємо поле Image, оскільки тепер ми використовуємо ImagesBulding
            buildingDto.Image = null;

            // Мапимо та зберігаємо об'єкт BuildingEntity
            var building = _mapper.Map<BuildingEntity>(buildingDto);
            await _buildingEntity.InsertAsync(building);
            await _buildingEntity.SaveAsync();
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
