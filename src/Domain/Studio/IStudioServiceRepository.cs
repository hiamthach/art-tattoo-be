using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Studio;

namespace art_tattoo_be.src.Domain.Studio
{
    public interface IStudioServiceRepository
    {   
        IEnumerable<StudioService> GetAll();
        StudioService GetById(int id);
        int CreateStudioService(StudioService studioService);
        int DeleteStudioService(int id);
    }
}