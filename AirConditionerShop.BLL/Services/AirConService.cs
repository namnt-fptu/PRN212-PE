using AirConditionerShop.DAL.Models;
using AirConditionerShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.BLL.Services
{
    public class AirConService
    {
        //thằng này bị GUI gọi và nó có data trong ram 
        // nó đi nhờ thằng repo lấy data giúp
        private AirConRepository _repository = new();

       public List<AirConditioner> GetAllAirConditioners() 
        {
            
            return _repository.GetAll();
        }

        public void DeleteAirCon(AirConditioner airConditioner)
        {
            _repository.Delete(airConditioner);
        }
        public void UpdateAirCon(AirConditioner airConditioner)
        {
            _repository.Update(airConditioner);
        }
        public void CreateAirCon(AirConditioner airConditioner)
        {
            _repository.Create(airConditioner);
        }

    }
}
