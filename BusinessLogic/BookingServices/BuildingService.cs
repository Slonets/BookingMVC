using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class BuildingService : IBuilding
    {
        private readonly IRepository<BuildingEntity> _buildingEntity;
        private readonly IRepository<ImagesBulding> _imagesBuldingEntity;
        private readonly IMapper _mapper;
        public readonly IImageWorker _imageWorker;
        private readonly IConfiguration _configuration;
        

        public BuildingService(IRepository<BuildingEntity> buildingEntity, IRepository<ImagesBulding> imagesBuldingEntity, IMapper mapper, IImageWorker imageWorker, IConfiguration configuration)
        {
            _buildingEntity = buildingEntity;
            _mapper = mapper;
            _imageWorker = imageWorker;
            _configuration = configuration;
            _imagesBuldingEntity = imagesBuldingEntity;           
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

        public async Task Create(BuildingCreateDto create)
        {
            var newBulding = _mapper.Map<BuildingEntity>(create);

            await _buildingEntity.InsertAsync(newBulding);
            await _buildingEntity.SaveAsync();

            foreach (var image in create.Images)
            {
                _imagesBuldingEntity.InsertAsync(
                    new ImagesBulding
                    {
                        Path = _imageWorker.ImageSave(image),
                        BuildingEntityId = create.Id
                    });
            }
            _imagesBuldingEntity.SaveAsync();
        }

        public async Task Edit(BuildingDto buildingDto)
        {
            //отримати обєкт з серверу за цим Id, який раніше зберігався
            //var OldObject = await _buildingEntity.GetByIDAsync(buildingDto.Id);                  

            //if (OldObject != null)
            //{
            //    if(buildingDto.Image!=null)
            //    {
            //        //видаляю старі фото
            //        foreach (var image in OldObject.ImagesBulding)
            //        {
            //            _imageWorker.RemoveImage(image.Path);
            //        }

            //        //buildingDto.ImagesBulding.Add(_imageWorker.ImageSave(buildingDto.Image));
            //    }
                         

            //    await _buildingEntity.UpdateAsync(OldObject);
            //    await _buildingEntity.SaveAsync();
            //}
        }

        public async Task Delete(int id)
        {
            throw new NotImplementedException();
        }        
    }
}
