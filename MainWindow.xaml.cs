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
using Terminals.Model;
using Terminals.Windows;

namespace Terminals
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HranitelPRO7 db = HranitelPRO7.GetContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            Sotrudniki sotrudnik = new Sotrudniki();
            var sotrudniki = db.Sotrudniki.ToList();

            if (tbCode.Text == "")
            { MessageBox.Show("Заполните поле", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); }
            else
            {
                if (db.Sotrudniki.Any(p => p.Kod_avtorizacii == tbCode.Text))
                {
                    foreach (Sotrudniki s in sotrudniki)
                    {
                        if (tbCode.Text == s.Kod_avtorizacii)
                        { sotrudnik = s; break; }
                    }

                    switch (sotrudnik.ID_Dolzhnosti)
                    {
                        case 1:
                            wndSecurity wnd1 = new wndSecurity(sotrudnik);
                            wnd1.Show();
                            this.Close();
                            break;

                        case 2:
                            wndPostGlobal wnd3 = new wndPostGlobal();
                            wnd3.Show();
                            this.Close();
                            break;

                        case 3:
                            wndPostOtdel wnd2 = new wndPostOtdel(sotrudnik);
                            wnd2.Show();
                            this.Close();
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Такого сотрудника не существует!\nПроверьте правильность заполнения.",
                        "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    tbCode.Text = "";
                }
            }            
        }
    }
}
