using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebApp.Data.Entities
{
    public class AccountRole
    {
        [Key]
        public long Id { get; set; }
        
        public Account Account { get; set; }
        public RoleModel Role { get; set; }
    }
}