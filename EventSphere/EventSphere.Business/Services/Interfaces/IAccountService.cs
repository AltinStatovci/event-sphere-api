using EventSphere.Domain.DTOs.User;
using EventSphere.Domain.Entities;
﻿using EventSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventSphere.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDTO> AddUserAsync(CreateUserDTO createUserDto);
        Task<string> AuthenticateAsync(LoginDTO loginDto);

    }
}
