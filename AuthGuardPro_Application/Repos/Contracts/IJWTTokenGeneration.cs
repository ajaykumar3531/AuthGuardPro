using AuthGuardPro_Application.DTO_s.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application.Repos.Contracts
{
    public interface IJWTTokenGeneration
    {
        Task<string> TokenGeneration(TokenRequest request);
    }
}
