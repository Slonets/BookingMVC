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
        public string ViewOfTheHouse { get; set; }
        public int? NumberOfRooms { get; set; }
        public string TypeOfSale { get; set; }
        public int Price { get; set; }
        public string UserEntity { get; set; }
        public string Image { get; set; } 
    }
}
