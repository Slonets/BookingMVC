using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Area { get; set; }
        public string Address { get; set; }
        public ViewOfTheHouseDto ViewOfTheHouse { get; set; }
        public int? NumberOfRooms { get; set; }
        public TypeOfSaleDto TypeOfSale { get; set; }
        public int Price { get; set; }
        public UserDto UserEntity { get; set; }       
        public List<IFormFile> Image { get; set; }
        public List<ImagesBuldingDto> ImagesBulding { get; set; } 
    }
}
