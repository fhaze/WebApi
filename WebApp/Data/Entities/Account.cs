using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Data.Entities
{
    public class Account
    {
        [Key]
        public long Id { get; set; }
        
        public string Email { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public List<AccountRole> Roles { get; set; }
    }
}