using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Exceptions;
using WebApp.Models;
using WebApp.Security;

namespace WebApp.Services
{
    public class AccountService
    {
        private readonly IConfiguration _configuration;
        private readonly WebAppDbContext _dbContext;
        
        public AccountService(IConfiguration configuration, WebAppDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        /// <summary>
        /// ログインする
        /// </summary>
        /// <param name="login">Eメールまたはニックネーム</param>
        /// <param name="password">パスワード</param>
        /// <returns>JWTトーケン</returns>
        /// <exception cref="ServiceException"></exception>
        public string Login(string login, string password)
        {
            var account = _dbContext.Accounts
                .Include(a => a.Roles)
                .Include("Roles.Role")
                .FirstOrDefault(a => (a.NickName == login || a.Email == login) &&
                                     a.Password == Encryptor.Md5Hash(password));

            if (account == null)
                throw new ServiceException("アカウントが存在しません。");
            
            if (!account.Active)
                throw new ServiceException("無効なアカウント。");
            
            //JWTトーケンを作る
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.NickName),
                new Claim(JwtRegisteredClaimNames.Jti, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email)
            };

            foreach (var role in account.Roles)
                claims.Add(new Claim("role", Enum.GetName(typeof(Role), role.Role.Name)));
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtToken:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                _configuration["JwtToken:Issuer"],
                _configuration["JwtToken:Audiance"],
                claims,
                expires:DateTime.Now.AddMinutes(10),
                signingCredentials:creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        /// <summary>
        /// 新しいアカウント登録されます
        /// </summary>
        /// <param name="nickname">ニックネーム</param>
        /// <param name="email">Eメール</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Account Register(string nickname, string email, string password)
        {
            if (_dbContext.Accounts.Any(p => p.Email == email))
                throw new ServiceException("Eメールがすでに登録されています。");

            if (_dbContext.Accounts.Any(p => p.NickName == nickname))
                throw new ServiceException("ニックネームがすでに登録されています。");
            
            var account = new Account
            {
                CreationDate = DateTime.UtcNow,
                Active = true,
                NickName = nickname,
                Email = email,
                Password = Encryptor.Md5Hash(password),
                Roles = new List<AccountRole>()
            };
            
            //デフォルトロール
            AddRole(account, Role.User);
            
            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();
            return account;
        }

        /// <summary>
        /// ロールを追加する
        /// </summary>
        /// <param name="account">ユーザー</param>
        /// <param name="role">ロール</param>
        /// <exception cref="ServiceException"></exception>
        public void AddRole(Account account, Role role)
        {
            var roleToAdd = _dbContext.Roles.FirstOrDefault(r => r.Name == role);
            
            if (roleToAdd == null)
                throw new ServiceException("ロールは存在しません。");

            if (account.Roles.Any(r => r.Role.Name == role))
                throw new ServiceException("アカウントはロールがすでに登録されいます。");
            
            account.Roles.Add(new AccountRole {Account = account, Role = roleToAdd});
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// アカウントをリストする
        /// </summary>
        public IEnumerable<AccountModel> List
        {
            get
            {
                var accountsModel = new List<AccountModel>();
                var accounts = _dbContext.Accounts
                    .Include(a => a.Roles)
                    .Include("Roles.Role")
                    .ToList();

                foreach (var account in accounts)
                    accountsModel.Add(AccountModel.From(account));

                return accountsModel;
            }
        }

        public AccountModel Get(long id)
        {
            var account = _dbContext.Accounts
                .Include(a => a.Roles)
                .Include("Roles.Role")
                .FirstOrDefault(a => a.Id == id);
            
            if (account == null)
                throw new ServiceException("アカウントが存在しません。");
            
            return AccountModel.From(account);
        }
        
        public AccountModel Current(ClaimsPrincipal user)
        {
            var claimJti = user.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Jti);
            var account =  _dbContext.Accounts
                .Include(a => a.Roles)
                .Include("Roles.Role")
                .FirstOrDefault(a => a.Id == Convert.ToInt32(claimJti.Value));
            
            if (account == null)
                throw new ServiceException("アカウントが存在しません。");
            
            return AccountModel.From(account);
        }
    }
}