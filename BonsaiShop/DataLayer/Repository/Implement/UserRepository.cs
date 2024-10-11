using DataLayer.DBContext;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository.Implement
{
    public class UserRepository : IUserRepositoty
    {
        private readonly db_aad141_exe201Context _exe201Context;

        public UserRepository(db_aad141_exe201Context exe201Context)
        {
            _exe201Context = exe201Context;
        }

        public async Task<bool> CreateUser(User user)
        {
            try
            {
                await _exe201Context.Users.AddAsync(user);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUser(User user)
        {
            try
            {
                _exe201Context.Users.Remove(user);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<User>> GetAllUser()
        {
            try
            {
                return await _exe201Context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
               throw ex;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await _exe201Context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            try
            {
                return await _exe201Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUser(User user)
        {
            try
            {
                _exe201Context.Users.Update(user);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
