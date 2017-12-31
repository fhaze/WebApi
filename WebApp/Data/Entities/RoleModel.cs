using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Security;

namespace WebApp.Data.Entities
{
    [Table("Role")]
    public class RoleModel
    {
        [Key]
        public long Id { get; set; }
        
        public Role Name { get; set; }
    }
}