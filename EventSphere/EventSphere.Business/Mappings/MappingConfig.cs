using Mapster;
using EventSphere.Domain.Entities;
using EventSphere.Domain.DTOs;

namespace EventSphere.Business.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<User, UserDTO>();
            config.NewConfig<User, CreateUserDTO>().TwoWays();
            config.NewConfig<User, UpdateUserDTO>().TwoWays();
        }
    }

}
