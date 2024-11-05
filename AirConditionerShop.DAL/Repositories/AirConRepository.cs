using AirConditionerShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirConditionerShop.DAL.Repositories
{
    public class AirConRepository
    {
        //THẰNG NÀY CHƠI TRỰC TIẾP VỚI DB CONTEXT , NHỜ NÓ CRUD TABLE MÁY LẠNH 
        //NẾU A NHỜ B , A PHẢI KHAI BÁO BIẾN CỦA B 
        private AirConditionerShop2024DbContext _context;//lúc nào xài mới new
        //CRUD MÁY LẠNH , nhờ qua context
        public List<AirConditioner> GetAll() 
        {
            _context = new AirConditionerShop2024DbContext();
            //navigation property path : Supplier
            return _context.AirConditioners.Include("Supplier").ToList();
            //join 2 bảng qua navigation path , biến đối tượng supplierCompany
            //vì 1 máy lành thuộc về 1 nhà cung cấp 
            //trả về Db set , giống list nhưng ta phải convert sang list 
        }
        //HÀM NHẬN VÀO 1 OBJECT CẦN XÓA ~ obj : 1 vùng new AirCon
        //Trong biến obj muốn xóa đưa vào hàm , thì phía hậu trường của dbcontext , nó
        //obj.GetID() và nó viết câu lệnh DELETE WHERE TRÊN TABLE THẬT
        public void Delete(AirConditioner obj)
        {
            _context = new();//mỗi lần xài là  một lần new cái context
            _context.AirConditioners.Remove(obj);//xóa trong ram 
            _context.SaveChanges();//chính thức xóa trong db

        }
        public void Create(AirConditioner obj)
        {
            _context = new();//mỗi lần xài là  một lần new cái context
            _context.AirConditioners.Add(obj);//tạo trong ram 
            _context.SaveChanges();//chính thức tạo mới trong db

        }
        public void Update(AirConditioner obj)
        {
            _context = new();//mỗi lần xài là  một lần new cái context
            _context.AirConditioners.Update(obj);//update trong ram 
            _context.SaveChanges();//chính thức update trong db

        }

        public AirConditioner GetAirConById(int id)
        {
           
            return _context.AirConditioners.FirstOrDefault(a => a.AirConditionerId == id);
        }
    }
}
