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
using WpfAppAuth2.DbModels;

namespace WpfAppAuth2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Server = cfif31.ru; Port = 3306; User ID = ISPr23-35_RogozhnikovaYO; Database = ISPr23-35_RogozhnikovaYO_wpf; Password = ISPr23-35_RogozhnikovaYO; Character Set = utf8
        }

        private void Authorization_Click(object sender, RoutedEventArgs e)
        {
            if (tbLogin.Text == null || tbPassword.Text == null)
            {
                MessageBox.Show("Заполните логин и пароль для входа!");
            }
            else
            {
                Authorization authorization = CoreModel.init().Authorizations.FirstOrDefault(p => p.Login == tbLogin.Text && p.Password == tbPassword.Text);

                if (authorization.Role == "admin")
                {
                    AdminWindow window_Admin = new AdminWindow();
                    window_Admin.Show();
                    Hide();
                }
                else if (authorization.Role == "user")
                {
                    UserWindow window_User = new UserWindow();
                    window_User.Show();
                    Hide();
                }

            }
        }
    }
}
