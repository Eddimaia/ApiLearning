using AutoMapper;
using EstudoAPI.Models;
using EstudoAPI.ViewModels.Accounts;
using EstudoAPI.ViewModels.Categories;
using EstudoAPI.ViewModels.Posts;
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

            CreateMap<Post, ListPostsViewModel>()
                .ForMember(destino => destino.Category,
                map => map.MapFrom(
                    src => src.Category.Name))
                .ForMember(destino => destino.Author,
                map => map.MapFrom(
                    src => $"{src.Author.Name} ({src.Author.Email})"));
        }
    }
}
