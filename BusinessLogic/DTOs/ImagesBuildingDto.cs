﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class ImagesBuildingDto
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int BuildingEntityId { get; set; }
    }
}
