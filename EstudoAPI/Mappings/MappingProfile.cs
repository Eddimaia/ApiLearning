using AutoMapper;
using EstudoAPI.Models;
using EstudoAPI.ViewModels.Accounts;
using EstudoAPI.ViewModels.Categories;
using EstudoAPI.ViewModels.Roles;

namespace EstudoAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EditorCategoryViewModel, Category>()
                .ReverseMap();

            CreateMap<EditorRoleViewModel, Role>()
                .ReverseMap();

            CreateMap<RegisterViewModel, User>()
                .ForMember(destino => destino.Slug,
                map => map.MapFrom(
                    src => src.Email
                    .Replace("@", "-")
                    .Replace(".", "-")));
        }
    }
}
