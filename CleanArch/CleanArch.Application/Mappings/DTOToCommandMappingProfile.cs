using AutoMapper;
using CleanArch.Application.DTOs;
using CleanArch.Application.Products.Commands;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.Mappings
{
    public class DTOToCommandMappingProfile : Profile
    {
        public DTOToCommandMappingProfile()
        {
            CreateMap<ProductDTO, ProductCreateCommand>().ReverseMap();
            CreateMap<ProductDTO, ProductUpdateCommand>().ReverseMap();
        }
    }
}
