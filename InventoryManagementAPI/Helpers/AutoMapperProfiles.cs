using AutoMapper;
using InventoryManagementAPI.Dto;
using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductListDto>()
                .ForMember(d => d.Category, o => o.MapFrom(p => p.ProductCategory.Name));

            CreateMap<Product, ProductDetailDto>();

            CreateMap<ProductCategory, ProductCategoryInfoDto>();

            CreateMap<Inventory, InventoryListDto>()
                .ForMember(d => d.Product, o => o.MapFrom(i => i.Product.Name))
                .ForMember(d => d.Location, o => o.MapFrom(i => i.Location.Name));

            CreateMap<Inventory, InventoryDetailDto>()
                .ForMember(d => d.Product, o => o.MapFrom(i => i.Product))
                .ForMember(d => d.Location, o => o.MapFrom(i => i.Location));

        }
    }
}
