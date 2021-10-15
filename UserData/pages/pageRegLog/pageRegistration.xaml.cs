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

namespace UserData.pages.pageRegLog
{
    /// <summary>
    /// Логика взаимодействия для pageRegistration.xaml
    /// </summary>
    public partial class pageRegistration : Page
    {
        public pageRegistration()
        {
            InitializeComponent();
            genderListBox.ItemsSource = DB.DataBase.genders.ToList();
            genderListBox.SelectedValuePath = "id";
            genderListBox.DisplayMemberPath = "gender";
        }

        private void btnSendUser(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtLogin.Text != "" && txtPassword.Password != "")
                {

                    auth newUser = new auth { login = txtLogin.Text, password = txtPassword.Password, role = 2 };
                    DB.DataBase.auth.Add(newUser);
                    DB.DataBase.SaveChanges();
                    if (txtName.Text != "" &&
                          dateBirth.SelectedDate != null &&
                          genderListBox.SelectedIndex != -1
                          )
                    {
                        users data = new users();
                        data.id = newUser.id;
                        data.name = txtName.Text;
                        data.dr = dateBirth.SelectedDate.Value;
                        data.gender = (int)genderListBox.SelectedValue;
                        DB.DataBase.users.Add(data);
                        if (goodCB.IsChecked == true)
                        {
                            users_to_traits tr = new users_to_traits();
                            tr.id_user = newUser.id;
                            tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == goodCB.Content.ToString()).id;
                            DB.DataBase.users_to_traits.Add(tr);
                        }
                        if (nejnCB.IsChecked == true)
                        {
                            users_to_traits tr = new users_to_traits();
                            tr.id_user = newUser.id;
                            tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == nejnCB.Content.ToString()).id;
                            DB.DataBase.users_to_traits.Add(tr);
                        }
                        if (laskovCB.IsChecked == true)
                        {
                            users_to_traits tr = new users_to_traits();
                            tr.id_user = newUser.id;
                            tr.id_trait = DB.DataBase.traits.FirstOrDefault(x => x.trait == laskovCB.Content.ToString()).id;
                            DB.DataBase.users_to_traits.Add(tr);
                        }
                        DB.DataBase.SaveChanges();

                    }
                    MessageBox.Show("Регистрация прошла успешно!");
                    User.mainFrame.GoBack();
                }
                else
                {
                    MessageBox.Show("Заполните обязательные поля: \"Логин\", \"Пароль\" для регистрации в системе!");
                }
            }
            catch
            {
                MessageBox.Show("Произошла неизвестная ошибка. Повторите попытку позже!", "Ошибка");
            }


        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            User.mainFrame.GoBack();
        }
    }
}