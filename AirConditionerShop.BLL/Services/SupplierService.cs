using AirConditionerShop.DAL.Models;
using AirConditionerShop.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.BLL.Services
{
    public class SupplierService
    {
        private SupplierRepository _repository = new();
        //GUI gọi service ---- repo ---- dbContext
        public List<SupplierCompany> GetAllSuppliers()
        {
            return _repository.GetAll();
        }
        public SupplierCompany GetSupplierCompanyById(string id) 
        {
            return _repository.GetSupplierById(id);
        }
    }
}
