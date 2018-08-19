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

namespace YourBudget
{
    /// <summary>
    /// Логика взаимодействия для AddDirectDebitDialog.xaml
    /// </summary>
    public partial class AddDirectDebitDialog : Window
    {
        public AddDirectDebitDialog()
        {
            InitializeComponent();
            lblDialogDate.Content = MainWindow.dateFromCalendarSelected;
        }

        private void btnDialogAdd_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string[] GetDataFromDialog
        {
            get
            {
                string[] arr = new string[2];

                arr[0] = tbDialogName.Text;
                arr[1] = tbDialogPrice.Text;

                return arr;
            }
        }
    }
}
