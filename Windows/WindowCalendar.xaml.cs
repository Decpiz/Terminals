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

namespace Terminals.Windows
{
    /// <summary>
    /// Логика взаимодействия для WindowCalendar.xaml
    /// </summary>
    public partial class WindowCalendar : Window
    {
        internal DateTime InputValue;

        public WindowCalendar()
        {
            InitializeComponent();

            calendar.BlackoutDates.AddDatesInPast();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            InputValue = ((DateTime)calendar.SelectedDate).Date;
            Close();
        }

        
    }
}
