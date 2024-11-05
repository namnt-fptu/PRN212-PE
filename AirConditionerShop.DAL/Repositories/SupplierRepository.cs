using AirConditionerShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.DAL.Repositories
{
    public class SupplierRepository
    {
        private AirConditionerShop2024DbContext _context;//ko new , lúc nào dùng ms new
        //lấy tất cả hàng - cột của table SupplierCompany
        //4 cột , 5 hàng 
        public List<SupplierCompany> GetAll()
        {
            _context = new();
            return _context.SupplierCompanies.ToList();
        }
        public SupplierCompany GetSupplierById(string supplierId) 
        {
            return _context.SupplierCompanies.FirstOrDefault(s => s.SupplierId == supplierId);
        }
        

    }
}
