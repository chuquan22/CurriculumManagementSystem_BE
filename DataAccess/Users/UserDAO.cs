using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Users
{
    public class UserDAO
    {
        public readonly CMSDbContext _cmsDbContext = new CMSDbContext();
        public User Login(string email, string password)
        {
            UserLoginRequest request = new UserLoginRequest();
            request.email = email;
            request.password = password;
            User response = new User();
            try
            {
                response = _cmsDbContext.User.Where(u => u.user_email.Equals(email) && u.user_password.Equals(password)).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public List<User> GetAllUser()
        {
            var listUser = _cmsDbContext.User.Include(x => x.Role).ToList();
            return listUser;
        }

        public User GetUserById(int id)
        {
            var user = _cmsDbContext.User.Include(x => x.Role).FirstOrDefault(x => x.user_id == id);
            return user;
        }

        public List<User> PaginationUser(int page, int limit, string? txtSearch)
        {
            IQueryable<User> query = _cmsDbContext.User;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.user_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.user_email.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listUser = query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();
            return listUser;
        }

        public int GetTotalUser(string? txtSearch)
        {
            IQueryable<User> query = _cmsDbContext.User;

            if (!string.IsNullOrEmpty(txtSearch))
            {
                query = query.Where(x => x.user_name.ToLower().Contains(txtSearch.ToLower().Trim()) || x.user_email.ToLower().Contains(txtSearch.ToLower().Trim()));
            }

            var listUser = query
                .ToList();
            return listUser.Count;
        }

        public bool CheckUserDuplicate(string email)
        {
            return (_cmsDbContext.User?.Any(x => x.user_email.Equals(email))).GetValueOrDefault();
        }

        public string CreateUser(User user)
        {
            try
            {
                _cmsDbContext.User.Add(user);
                _cmsDbContext.SaveChanges();
                return Result.createSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string UpdateUser(User user)
        {
            try
            {
                _cmsDbContext.User.Update(user);
                _cmsDbContext.SaveChanges();
                return Result.updateSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }

        public string DeleteUser(User user)
        {
            try
            {
                _cmsDbContext.User.Remove(user);
                _cmsDbContext.SaveChanges();
                return Result.deleteSuccessfull.ToString();
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message;
            }
        }
    }
}
