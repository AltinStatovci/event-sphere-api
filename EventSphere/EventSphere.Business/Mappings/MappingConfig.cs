using Mapster;
using EventSphere.Domain.Entities;
using EventSphere.Domain.DTOs.User;

namespace EventSphere.Business.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserDTO>();
            config.NewConfig<CreateUserDTO, User>()
                .Ignore(dest => dest.Password)
                .Ignore(dest => dest.Salt)
                .Ignore(dest => dest.Role);
            config.NewConfig<User, CreateUserDTO>().TwoWays();
            config.NewConfig<User, UpdateUserDTO>().TwoWays();

            config.NewConfig<UpdateUserDTO, User>()
                .Ignore(dest => dest.Password)
                .Ignore(dest => dest.Salt);
        }
    }

}
