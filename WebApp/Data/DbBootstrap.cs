using System;
using WebApp.Security;
using WebApp.Services;

namespace WebApp.Data
{
    public static class DbBootstrap
    {
        public static void Init(WebAppDbContext dbContext, AccountService accountService)
        {
            foreach (var role in Enum.GetNames(typeof(Role)))
                dbContext.Roles.Add(new Entities.RoleModel{Name = Enum.Parse<Role>(role)});
            dbContext.SaveChanges();
            
            //スーパーユーザーを作る
            var admin = accountService.Register("admin", "admin@localhost", "123456");
            accountService.AddRole(admin, Role.Admin);
            
            //devユーザーを作る
            accountService.Register("dev", "dev@localhost", "123456");
        }
    }
}