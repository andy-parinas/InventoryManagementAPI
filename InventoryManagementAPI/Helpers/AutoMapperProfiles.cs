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
                .ForMember(d => d.Location, o => o.MapFrom(i => i.Location.Name))
                .ForMember(d => d.Status, o => o.MapFrom(i => i.Status.Status));

            CreateMap<Inventory, InventoryDetailDto>()
                .ForMember(d => d.Product, o => o.MapFrom(i => i.Product.Name))
                .ForMember(d => d.ProductId, o => o.MapFrom(i => i.Product.Id))
                .ForMember(d => d.Location, o => o.MapFrom(i => i.Location.Name))
                .ForMember(d => d.LocationId, o => o.MapFrom(i => i.Location.Id))
                .ForMember(d => d.Status, o => o.MapFrom(i => i.Status.Status))
                .ForMember(d => d.StatusId, o => o.MapFrom(i => i.Status.Id));

            CreateMap<InventoryTransaction, TransactionListDto>()
                .ForMember(d => d.TransactionType, o => o.MapFrom(t => t.TransactionType.Name));

            CreateMap<InventoryTransaction, TransactionDetailDto>()
               //.ForMember(d => d.TransactionType, o => o.MapFrom(t => t.TransactionType.Name))
               .ForMember(d => d.TransactionTypeId, o => o.MapFrom(t => t.TransactionType.Id));

            CreateMap<InventoryStatus, InventoryStatusListDto>();

        }
    }
}
