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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly db_aad141_exe201Context _exe201Context;

        public CategoryRepository(db_aad141_exe201Context exe201Context)
        {
            _exe201Context = exe201Context;
        }

        public async Task<bool> Create(Category entity)
        {
            try
            {
                await _exe201Context.Categories.AddAsync(entity);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(Category entity)
        {
            try
            {
                _exe201Context.Categories.Remove(entity);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Category>> GetAll()
        {
            try
            {
                return await _exe201Context.Categories
                    .Where(c => c.Status == true)
                    .Include(c => c.Products)
                    .Include(c => c.EcologicalCharacteristics)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Category> GetById(int id)
        {
            try
            {
                return await _exe201Context.Categories
                    .Where(c => c.Status == true)
                    .Include(c => c.Products)
                    .Include(c => c.EcologicalCharacteristics)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(Category entity)
        {
            try
            {
                _exe201Context.Categories.Update(entity);
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
