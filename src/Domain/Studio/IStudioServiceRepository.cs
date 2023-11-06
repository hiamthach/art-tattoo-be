using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Domain.Media;
using art_tattoo_be.Domain.Studio;
using art_tattoo_be.src.Application.DTOs.StudioService;

namespace art_tattoo_be.src.Domain.Studio
{
    public interface IStudioServiceRepository
    {
        IEnumerable<StudioService> GetAll();
        StudioService GetById(Guid id);
        int CreateStudioService(StudioService studioService);
        int DeleteStudioService(Guid id);
        StudioServiceList GetStudioServicePages(GetStudioServiceQuery req);
        int UpddateStudioService(StudioService studioService, IEnumerable<Media> mediaList);
    }
}