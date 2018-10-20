using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BasisFrameWork.Dependency.Interfaces;

namespace Application.Services
{
    public interface IDemoService:ITransientService
    {
        ValueTask<bool> Set(string value);

        Task<string> Get();
    }
}
