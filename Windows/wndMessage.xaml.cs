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
using System.Windows.Shapes;
using Terminals.Model;

namespace Terminals.Windows
{
    /// <summary>
    /// Логика взаимодействия для wndMessage.xaml
    /// </summary>
    public partial class wndMessage : Window
    {
        HranitelPRO7 db = HranitelPRO7.GetContext();
        Zajavki selectionApli = new Zajavki();

        public wndMessage(Zajavki z)
        {
            selectionApli = z;

            InitializeComponent();
        }

        private void tbMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text == "Сообщение.....")
            {
                tbMessage.Clear();
            }
        }

        private void tbMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbMessage.Text == "")
            { tbMessage.Text = "Сообщение....."; }
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var updateAppli = db.Zajavki.Where(x => x.Nomer_zajavki == selectionApli.Nomer_zajavki).FirstOrDefault();

            if (tbMessage.Text != "" && tbMessage != null && tbMessage.Text != "Сообщение.....")
            { 
                updateAppli.Soobshenie = tbMessage.Text;
                updateAppli.ID_Statusa = 2;

                selectionApli.Soobshenie = tbMessage.Text;
                selectionApli.ID_Statusa = 2;

                db.SaveChanges();

                Close();

                MessageBox.Show("Заявка успешно отклонена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            { 
                MessageBox.Show("Нужно заполнить сообщение для пользователя!", "Предупреждение", 
                MessageBoxButton.OK, MessageBoxImage.Warning); 
            }
        }
    }
}
