using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using art_tattoo_be.Application.DTOs.Pagination;

namespace art_tattoo_be.src.Application.DTOs.StudioService
{
    public class GetStudioServiceQuery : PaginationReq
    {
        public Guid StudioId { get; set; }
        public string? SearchKeyword { get; set; } = null!;
    }
}