using RensourceDomain.Models.Request;
using RensourceDomain.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Interfaces
{
    public interface IUserRepo
    {
        Task<GenericResponse> UserLoginAsync(UserLoginRequest loginReq);
        Task<GenericResponse> ForgotPasswordAsync(PasswordResetRequest request);
        Task<GenericResponse> ChangePasswordAsync(ChangePasswordRequest request);
    }
}
