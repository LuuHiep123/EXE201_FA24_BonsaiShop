using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public interface  IEcologicalCharacteristicRepository
    {
        public Task<bool> Create(EcologicalCharacteristic entity);
        public Task<bool> Update(EcologicalCharacteristic entity);
        public Task<bool> Delete(EcologicalCharacteristic entity);
        public Task<List<EcologicalCharacteristic>> GetAll();
        public Task<EcologicalCharacteristic> GetById(int id);
    }
}
