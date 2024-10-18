using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface ICategoryRepository
    {
        public Task<bool> Create(Category entity);
        public Task<bool> Update(Category entity);
        public Task<bool> Delete(Category entity);
        public Task<List<Category>> GetAll();
        public Task<Category> GetById(int id);
    }
}
