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
    public class EcologicalCharacteristicRepository : IEcologicalCharacteristicRepository
    {
        private readonly db_aad141_exe201Context _exe201Context;

        public EcologicalCharacteristicRepository(db_aad141_exe201Context exe201Context)
        {
            _exe201Context = exe201Context;
        }

        public async Task<bool> Create(EcologicalCharacteristic entity)
        {
            try
            {
                await _exe201Context.EcologicalCharacteristics.AddAsync(entity);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Delete(EcologicalCharacteristic entity)
        {
            try
            {
                _exe201Context.EcologicalCharacteristics.Remove(entity);
                await _exe201Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<EcologicalCharacteristic>> GetAll()
        {
            try
            {
                return await _exe201Context.EcologicalCharacteristics
                    .Where(ec => ec.Status == true)
                    .Include(ec => ec.Categories)
                        .ThenInclude(c => c.Products)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EcologicalCharacteristic> GetById(int id)
        {
            try
            {
                return await _exe201Context.EcologicalCharacteristics
                    .Include(ec => ec.Categories)
                        .ThenInclude(c => c.Products)
                    .Where(ec => ec.Status == true)
                    .FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Update(EcologicalCharacteristic entity)
        {
            try
            {
                _exe201Context.EcologicalCharacteristics.Update(entity);
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
