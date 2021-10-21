using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserData.pages
{
    /// <summary>
    /// Логика взаимодействия для pageInfoUser.xaml
    /// </summary>
    public partial class pageInfoUser : Page
    {
        public void DataLoad()
        {
            DataContext = DB.currentUser;
            List<users_to_traits> traits = DB.DataBase.users_to_traits.Where(x => x.id_user == DB.currentUser.id).ToList();
            foreach (users_to_traits tr in traits)
            {
                lbInfo.Content = lbInfo.Content + tr.traits.trait + "; ";
            }
        }
        public pageInfoUser()
        {
            InitializeComponent();
            DataLoad();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.GoBack();
        }
    }
}
