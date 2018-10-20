using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BasisFrameWork.Dependency.Interfaces;

namespace Application.Demonstration.Redis
{
    public interface IRedisDemoService:ITransientService
    {
        Task<bool> Set(string value);
        Task<string> Get();
    }
}
