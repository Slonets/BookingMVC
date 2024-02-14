using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class BuildingEntity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [ForeignKey("ViewOfTheHouse")]
        public int ViewOfTheHouseId { get; set; }
        public ViewOfTheHouse ViewOfTheHouse { get; set; }
        [ForeignKey("TypeOfSale")]
        public int TypeOfSaleId { get; set; }
        public TypeOfSale TypeOfSale { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("UserEntity")]
        public int UserEntityId { get; set; }
        public UserEntity UserEntity { get; set; }
        public ICollection<ImagesBulding> ImagesBulding { get; set; }
    }
}
