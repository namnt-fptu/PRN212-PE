using AirConditionerShop.BLL.Services;
using AirConditionerShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirConditionerShop_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for DetailWindow.xaml
    /// </summary>
    public partial class DetailWindow : Window
    {
        private AirConService _airConService = new();
        
        private SupplierService _supplierService = new();


        //MÀN HÌNH DETAIL CÓ 2 MODE : MODE EDIT , CREATE 
        //MODE CREATE : LOAD MÀN HÌNH TRẮNG , CÓ THÊM COMBO CÓ SẴN 5 NHÀ CUNG CẤP
        // MÌNH CHỌN DEFAULT NHÀ CUNG CẤP ĐẦU TIÊN
        //MODE EDIT : LÀ MÀN HÌNH SẼ ĐC FILL ĐẦY INFO VÀO CÁC Ô TEXT 
        //INFO NÀY LẤY TỪ OBJECT SELECTED BÊN MAIN GỬI SANG 
        //BÊN MAIN , CÓ BIẾN AirCon select = trỏ sẵn đến cái dòng đc select
        //BÊN MÀN HÌNH NÀY PHẢI TRỎ VÀO BIẾN SELECTED BÊN KIA , TRỎ
        //ĐC THÌ CHẤM TỪNG MIẾNG ĐỂ ĐỔ VÀO Ô TEXT
        //VẬY BÊN MÀN HÌNH DETAIL ,TA CẦN KHAI BÁO MỘT BIẾN STYLE KIỂU AirCon
        //Vd : AirCon x = selected bên main
        //để sờ đc biến x , thì biến x phải là public để ta .x = selected
        //nó là hình thức của hàm Set()
        //do đó , bên detail này ta khai báo 1 property kiểu AirCon
        //VÌ ĐÂY LÀ 1 PROP , NÊN KHI NEW MÀN HÌNH DETAIL , NEW CLASS DETAIL MÀ
        //KO NÓI NĂNG GÌ CẢ , THÌ THẰNG NÀY MANG GIÁ TRỊ NULL
        //TỨC LÀ TA ĐANG Ở CREATE MODE
        //CÒN NẾU TA SET NÓ = SELECTED KHI TA NEW , TỨC LÀ TA ĐANG Ở EDIT MODE

        //EDITEDONE : BIẾN PHẤT CỜ , BIẾN FLAG , DÙNG ĐỂ KIỂM SOÁT STATE , TRẠNG THÁI CỦA OBJECT
        //IF MÀY == NULL : TẠO MỚI
        //IF MÀY == SELECTED (!=NULL ) -> UPDATE
        //THAY THẾ BIẾN BOOLEAN ĐỂ CHECK CREATE|UPDATE

        
        public AirConditioner EditedOne { get; set; }
        //EditedOne là 1 prop hứng cái selected bên Main vì chúng đều cùng là AirCon


        public DetailWindow()
        {
            InitializeComponent();
        }

        private void SupplierIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //1 đổ toàn bộ supplier vào cái combo box
            //2. Nếu là create , ko cần làm gì thêm , để màn hình cho user gõ máy lạnh mới
            //3. nếu là edit , lôi cổ thằng selected ở bên main đem sang đây , lấy các property
            //của selected đổ vô ô text .Text = "cái cần đổ"
                //3.1. Phải nhảy đến cái supplier tương ứng của máy lạnh đang edit thuộc dòng
                // Mitsubishi ở vị trí 3 , thì combobox phải hiển thị sẵn mitsubishi
            //tạo mới thì show cái nào cũng đc 
            //đổ combo y chang đổ grid , đều là hàng/cột , thằng này chỉ 1 cột , nhưng
            //vẫn nhiều hàng
            

            List<SupplierCompany> supplierCompanies = _supplierService.GetAllSuppliers();
            
            SupplierIdComboBox.ItemsSource =supplierCompanies;
            //mặc định show hết các cột , dưới dạng ToString() , mày là object loại gì 
            //vậy ta cần show 1 cột hoy , chọn 1 cột khác để lấy khóa ngoại 
            //đem tặng cho thằng AirCon
            SupplierIdComboBox.DisplayMemberPath = "SupplierName";//cột show ra cho ngta chọn
            SupplierIdComboBox.SelectedValuePath = "SupplierId";//Cột lấy value làm fk ( id )
            
            //ĐỔ CÁC Ô TEXT LẤY VALUE TỪ SELECTED CHUYỂN SANG ( NAY ĐAG ĐC TRỎ BỞI EDITEDONE)
            //NHỚ ĐIỀN DATA , ĐIỀN ĐỦ CÁC Ô TEXT ỨNG VỚI CÁC CỘT TRONG TABLE CHÍNH
            //CHECK STATUS : MODE EDIT HAY CREATE 

            if (EditedOne == null) 
            {
                DetailWindowModeLabel.Content = "Create new Air Conditioner";
                SupplierIdComboBox.SelectedValue = "SC0005";
                return;
               
            }
            DetailWindowModeLabel.Content = "Update current Air Conditioner";
            //disable ô nhập id máy lạnh
            AirConditionerIdTextBox.IsEnabled = false;

            AirConditionerIdTextBox.Text = EditedOne.AirConditionerId.ToString();
            AirConditionerNameTextBox.Text = EditedOne.AirConditionerName;
            WarrantyTextBox.Text = EditedOne.Warranty;
            SoundPressureLevelTextBox.Text = EditedOne.SoundPressureLevel;
            FeatureFunctionTextBox.Text = EditedOne.FeatureFunction;
            QuantityTextBox.Text = EditedOne.Quantity.ToString();
            DollarPriceTextBox.Text = EditedOne.DollarPrice.ToString();


            //Jump đến đúng cái value của ComboBox
            //VÍ DỤ MÌNH SINH Ở KIÊN GIUANG , THÌ EDIT HỒ SƠ MÌNH , DANH SÁCH 63 TỈNH , CÓ TỈNH
            //KIÊN GIANG ĐẦU BẢNG 
            //PHẢI SELECT SẴN KIÊN GIANG
            //SUPPLIER ID Y CHANG , PHẢI NHẢY ĐÊN NHÀ CUNG CẤP MÀ THẰNG EDIT ĐANG CÓ 
            //MÀN HÌNH NEW THÌ CỨ SET THẰNG ĐẦU TIÊN , MÁY LẠNH MỚI THÌ CHỦ ĐỘNG LỰA CHỌN NHÀ CUNG CẤP
            //TODO , TỰ LÀM THỬ
            SupplierIdComboBox.SelectedValue = EditedOne.SupplierId;
            //gán ngược supplier id của selected máy lạnh vào trong combo box -> combo sẽ nhảy đến item tương ứng 
            // nghĩa là edit 1 sản phẩm thì hiển thị nhà cung cấp tương ứng của nó

       

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //check Combobox xem user chon và mình get đúng cái id supplier
            //chọn gì nằm trong selected-value
            //MessageBox.Show("You have selected supplier Id : " + SupplierIdComboBox.SelectedValue);
            //CACS BƯỚC XỬ LÍ Ở NÚT BẤM NÀY : 
            //1.TẠO MỚI AIRCON OBJECT , THU NẠP CÁC INFO TỪ CÁC Ô NHẬP THẢ VÀO OBJECT , HÀM SET AIRCON
            //2.TÙY LÀ EDIT HAY CREATE ĐỂ TA GỌI SERVICE TƯƠNG ỨNG ĐỂ SAVE XUỐNG DATABASE
            //3.ĐÓNG MÀN HÌNH NÀY LẠI
            //4.TRỞ LẠI MÀN HÌNH MAIN VÀ F5 CÁI GRID ĐỂ CÓ ĐC INFO MỚI NHẤT TỪ DATABASE
            //LƯU Ý : CÁI VALIDATION (LƯỢNG ĐIỂM KHA KHÁ ) 
            //TÍNH VALIDATION RIÊNG CHO CREATE , CHO UPDATE , MẶC DÙ GIỐNG NHAU 100%
            //NHƯNG DO KHÁC TÍNH NĂNG 
            //VD : SỐ QUANTITY LÀ CON SỐ TỪ 100...1000 
            //GÕ CHỮ , GÕ SỐ NẰM NGOÀI BIÊN 
            //LƯU Ý : PK-PRIMARY KEY : TRÙNG HAY KO KHI TẠO MỚI
            //        KO CHO EDIT KHI UPDATE

            if (!CheckValidate())
            {
                return;
            }
            
            AirConditioner obj = new();
            //tui ko làm new theo style object initialization 
            //vì lấy ô text dài , chưa kể là kiểu số thì phải ép kiểu , nếu gộp gán giá trị lúc new
            //code khó theo dõi
            //nó ngược lại với lúc viết code từ edited vào ô text
            obj.AirConditionerId = int.Parse(AirConditionerIdTextBox.Text);
            obj.AirConditionerName = AirConditionerNameTextBox.Text;
            obj.Warranty = WarrantyTextBox.Text;
            obj.SoundPressureLevel = SoundPressureLevelTextBox.Text;
            obj.DollarPrice = double.Parse(DollarPriceTextBox.Text);
            obj.FeatureFunction = FeatureFunctionTextBox.Text ;
            obj.Quantity = int.Parse(QuantityTextBox.Text);
            obj.SupplierId = SupplierIdComboBox.SelectedValue.ToString();
            List<SupplierCompany> supplierCompanies = _supplierService.GetAllSuppliers();


            //combo xổ xuống có 
            if (EditedOne == null)
            {
                _airConService.CreateAirCon(obj);
            }
            else
            {
                _airConService.UpdateAirCon(obj);
            }
           //đóng cửa sổ này lại 
           this.Close();

        }

        private void AirConditionerIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private bool CheckValidate()
        {
            //check ô text nào đó có gõ hay ko , gõ có đủ độ dài nào đó hay ko
            //nếu bỏ trống ô nhập hoặc nhập 1 đống dấu cách , phím tab , chửi

            if (!int.TryParse(AirConditionerIdTextBox.Text,out int parsedAirConId))
            {
                MessageBox.Show("Airconditioner ID must be a integer number!", "ID required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            AirConditionerIdTextBox.Text = parsedAirConId.ToString();
            if (string.IsNullOrWhiteSpace(AirConditionerNameTextBox.Text))
            {
                MessageBox.Show("Air-con name is required","Field required",MessageBoxButton.OK,MessageBoxImage.Error);
                return false;
            }
            if (AirConditionerNameTextBox.Text.Trim().Length<5 || AirConditionerNameTextBox.Text.Trim().Length > 90)
            {
                MessageBox.Show("Air-con name length must be >=5 and <= 90", "Length constraint", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //HỎI KHÓ NHAU : ĐỔI CHỮ HOA TỪNG ĐẦU TỪ ...
            //Class phụ /đặc biệt : hỗ trợ trình bày văn bản theo các nền văn hóa khác nhau
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            //convert name thành chữ hoa từng đầu từ , trước đó biến chữ thường toàn bộ
            //tránh bị hiện tượng : nGOC tRINH -> NGOC TRINH ( sai )
                                             //-> ngoc trinh -> Ngoc Trinh
            string airConName = AirConditionerNameTextBox.Text.Trim();//lấy đc cái name vừa gõ
            airConName = textInfo.ToTitleCase(airConName.ToLower());
            AirConditionerNameTextBox.Text = airConName;
            //CHECK VALIDATE CHO KIỂU SỐ 
            if(!int.TryParse(QuantityTextBox.Text, out int parsedQuantity)) //cố gắng convert , nếu convert ko đc -> trả về boolean 
            {
                MessageBox.Show("Quantity must be a integer number!", "Quantity required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //range quantity 50...100
            if (parsedQuantity < 50 || parsedQuantity > 100) 
            {
                MessageBox.Show("Quantity must be between 50 - 100 ", "Quantity required", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            QuantityTextBox.Text = parsedQuantity.ToString();
            return true;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
