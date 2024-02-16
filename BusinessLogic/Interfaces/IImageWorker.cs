using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IImageWorker
    {
        string ImageSave(IFormFile image);
        string ImageSave(string url);
        void RemoveImage(string name);
    }
}
