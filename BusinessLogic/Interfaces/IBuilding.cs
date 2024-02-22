using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IBuilding
    {
        Task<BuildingDto> GetId(int id);
        Task<ICollection<BuildingDto>> GetAll();
        Task<ICollection<BuildingDto>> AllFlat();
        Task<ICollection<BuildingDto>> AllHouse();        
        Task Create(BuildingDto buildingDto);
        Task Edit(BuildingDto buildingDto);
        Task Delete(int id);
    }
}
