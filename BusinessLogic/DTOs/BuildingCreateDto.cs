using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.DTOs
{
    public class BuildingCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Area { get; set; }
        public string Address { get; set; }
        public int ViewOfTheHouseId { get; set; }
        public int? NumberOfRooms { get; set; }
        public int TypeOfSaleId { get; set; }
        public int Price { get; set; }
        public int UserEntityId { get; set; }
        public List<IFormFile> Images { get; set; }
        public string ViewOfTheHouse { get; set; }
        public SelectList ViewOfTheHouseList { get; set; }
        public SelectList TypeOfSaleList { get; set; }
        public SelectList UserList { get; set; }
    }
}
