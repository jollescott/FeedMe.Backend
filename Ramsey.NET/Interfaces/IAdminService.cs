using Ramsey.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsey.NET.Interfaces
{
    public interface IAdminService
    {
        AdminUser Authenticate(string username, string password);
        IEnumerable<AdminUser> GetAll();
        AdminUser GetById(int id);
        Task<AdminUser> CreateAsync(AdminUser user, string password);
        Task UpdateAsync(AdminUser user, string password = null);
        Task DeleteAsync(int id);
    }
}
