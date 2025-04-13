using AutoMapper;
using backend.Core.Dto.CareDto;
using backend.Core.Dto.UserDto;
using backend.Core.Models;

namespace backend.Core.AutoMapper
{
    public class AutoMapperConfigProfile: Profile
    {
        public AutoMapperConfigProfile()
        {
            CreateMap<Care, GetCare>();
            CreateMap<CreateCare, Care>();           
        }
    }
}
