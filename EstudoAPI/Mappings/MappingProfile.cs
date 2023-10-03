﻿using AutoMapper;
using EstudoAPI.DTO;
using EstudoAPI.Models;

namespace EstudoAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EditorCategoryDTO, Category>()
                .ReverseMap();
        }
    }
}