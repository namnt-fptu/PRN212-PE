using AirConditionerShop.BLL.Services;
using AirConditionerShop.DAL.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirConditionerShop_HoangNgocTrinh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AirConService _airConService = new();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AirCondDataGrid.ItemsSource = _airConService.GetAllAirConditioners();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //1.Xem user đã click 1 dòng máy lạnh trong grid hay chưa
            //2.Chưa click dòng nào thì chửi
            //3.Rồi thì chuyển cái thằng vừa bị select sang trang Detail
            //giao nó sang màn hình detail , tạm gọi thằng này là selected item
            //4.Mở màn hình detail ... ShowDialog(); ( show 1 màn hình thui )
            //5.Chờ màn hinh detail đóng lại ( giả sử user có sửa và save )
            // Ta reload lại cái Grid 
            // vì nếu có sửa trong database thì phải render lại cái mới nhất

            AirConditioner? selectedAirCon = AirCondDataGrid.SelectedItem as AirConditioner;
            // as là toán tử ép kiểu biến object này thành object khác ( nếu ép đc )
            // nó thay thế cho ép kiểu truyền thống (AirConditioner)AirCondDataGrid.SelectedItem 
            // as : ép ko đc trả về null , đc thì trỏ đến 1 cái item , vùng new AirCon trong ram ( vùng new này đc tạo ra lúc gọi service.GetAll() )
            // () : ép ko đc throw exception

            if(selectedAirCon == null)
            {
                MessageBox.Show("Please select item before editing","Select one",MessageBoxButton.OK,MessageBoxImage.Stop);
                return;
            }
            //ngược lại tức là chọn 1 dòng , mình show thử xem có đúng dòng chọn hok 
            //đang nằ, trong selected chính là 1 object của class AirCon
            MessageBox.Show("You have selected : " + selectedAirCon.AirConditionerId + " | " + selectedAirCon.AirConditionerName);
            //mở màn hình details và show nó lên , chờ ngta edit -> save -> reload grid bên main
            DetailWindow dw = new DetailWindow();
            //chuyển selected item sang details... ngày
            dw.EditedOne = selectedAirCon;
            dw.ShowDialog();

            //CHỐT HẠ : 2 BIẾN OBJECT GÁN BẰNG NHAU , TỨC LÀ 2 CHÀNG TRỎ 1 NÀNG
            //SELECTED ĐANG TRỎ AI , CHO EDITEDONE TRỎ CÙNG VỚI
            //BIẾN OBJECT = NHAU -> PASS BY REFERENCE
            //NÊN NHỚ , TẠO MỚI OBJECT BẮT BUỘC PHẢI XUẤT HIỆN TOÁN TỬ NEW 
            //MỌI SỰ THAY ĐỔI OBJECT MÀ KO CÓ NEW ĐỀU LÀ OBJECT CŨ ĐÃ NEW SẴN RỒI

            //reload grid 
            FillDataGrid(_airConService.GetAllAirConditioners());


        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailWindow dw = new DetailWindow();
            dw.ShowDialog();//mở màn hình để ngta nhập data
          
            FillDataGrid(_airConService.GetAllAirConditioners());
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //BẮT XEM ĐÃ CHỌN 1 DÒNG CẦN XÓA HAY CHƯA , CHƯA THÌ CHỬI
            // CONFIRM THÊM 1 NHÁT , HỎI YES/NO , YES THÌ XÓA , 
            // XÓA XONG THÌ RELOAD GRID
            AirConditioner? selectedAirCon = AirCondDataGrid.SelectedItem as AirConditioner;
            if (selectedAirCon == null)
            {
                MessageBox.Show("Please select item before editing", "Select one", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
            //nếu chọn rồi thì confirm xóa - 0.75đ
           
            MessageBoxResult answer =  MessageBox.Show("Are you sure to delete this item","Confirm ?",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (answer == MessageBoxResult.No) return;
            //yes roi 
            //Nhờ service giúp xóa 1 object , là thằng selected
            _airConService.DeleteAirCon(selectedAirCon);
            FillDataGrid(_airConService.GetAllAirConditioners());


        }
        //TA THẤY RELOAD , ĐỔ DATA VÀO DATA GRID <AIR-CON> ĐC XÀI HOÀI 
        // LOAD MÀN HÌNH , CREATE , UPDATE ,DELETE ĐỀU CẦN 
        //TÁCH RELOAD THÀNH 1 HÀM NỘI BỘ TRONG CLASS NÀY ĐỂ DÙNG CHUNG 
        //CHO NHIỀU HÀM KHÁC ĐỂ CODE RÕ RÀNG TỪNG KHỐI CÔNG VIỆC 
        //---> HÀM HELPER - HÀM TRỢ GIÚP
        private void FillDataGrid(List<AirConditioner> arr)
        {
            AirCondDataGrid.ItemsSource = null;// xóa grid 
            AirCondDataGrid.ItemsSource = arr ;//gán bằng danh sách mới 

        }

        private void AirCondDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}