using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Data.Entities;
using WebApp.Security;

namespace WebApp.Models
{
    public class AccountModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<string> Roles { get; set; }
        
        public static AccountModel From(Account account)
        {
            return new AccountModel
            {
                Id = account.Id,
                CreationDate = account.CreationDate,
                Email = account.Email,
                NickName = account.NickName,
                Roles = account.Roles.Select(r => Enum.GetName(typeof(Role), r.Role.Name))
            };
        }
    }
}