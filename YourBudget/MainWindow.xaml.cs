using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace YourBudget
{
    /// <summary>
    /// Logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string dataIncomes = "IncomesData.xml";
        private string dataOutcomes = "OutcomesData.xml";

        private List<Income> incomesList = new List<Income>();
        private List<Outcome> outcomesList = new List<Outcome>();

        private int indexOfSelectedFromIncomeListBox = -1;
        private int indexOfSelectedFromOutcomeListBox = -1;

        public static string dateFromCalendarSelected;


        public MainWindow()
        {
            InitializeComponent();
            LoadDataOnStart();
        }
        //Buttons and listBox selecting events-------------------------------------------------------------
        //INCOMES

        /// <summary>
        /// Incomes refresh fields btn event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncomeRefreshFields_Click(object sender, RoutedEventArgs e)
        {
            //Caling refresh fields method
            IncomesFieldsRefresh();
        }

        /// <summary>
        /// Incomes btn "Add" event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncomeAdd_Click(object sender, RoutedEventArgs e)
        {
            IncomesAddToList();
        }

        /// <summary>
        /// Incomes delete btn event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIncomeDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteIncomesItem();
        }

        //This event have 1 string of code inside
        /// <summary>
        /// Index of selected item in incomes list, put in this variable "indexOfSelectedFromIncomeListBox"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxIncomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            indexOfSelectedFromIncomeListBox = listBoxIncomeList.SelectedIndex; // get the index of selected item

            //ListBoxItem listitem = (ListBoxItem)(listBoxIncomeList.ItemContainerGenerator.ContainerFromIndex(index));
            //MessageBox.Show(listitem.ToString());
        }


        //OUTCOMES
        /// <summary>
        /// btn refresh fields event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutcomeRefreshFields_Click(object sender, RoutedEventArgs e)
        {
            OutcomesFieldsRefresh();
        }

        /// <summary>
        /// btn add event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutcomeAdd_Click(object sender, RoutedEventArgs e)
        {
            OutcomesAddToList();
        }

        /// <summary>
        /// btn delete outcome event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutcomeDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteOutcomesItem();
        }

        //This event have 1 string of code inside
        /// <summary>
        /// Index of selected item in incomes list, put in this variable "indexOfSelectedFromOutcomeListBox"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxOutcomeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indexOfSelectedFromOutcomeListBox = listBoxOutcomeList.SelectedIndex;
        }

        /// <summary>
        /// Calendar selected data changed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOutcomeToListFromDialog(sender);
        }


        //INCOMES ADD, DELETE, FIELDS REFRESH, LIST REFRESH | methods.-------------------------------------

        /// <summary>
        /// Refreshing Incomes textBoxes
        /// </summary>
        private void IncomesFieldsRefresh()
        {
            tbIncomeName.Text = "Name";
            tbIncomePrice.Text = "0,0";
        }

        /// <summary>
        /// Refreshing Incomes textBox "Price" only
        /// </summary>
        private void IncomesFieldsRefreshPriceOnly()
        {
            tbIncomePrice.Text = "";
        }

        /// <summary>
        /// IncomeList refresh method
        /// </summary>
        private void IncomesListRefresh()
        {
            listBoxIncomeList.Items.Clear();

            foreach (Income income in incomesList)
            {
                listBoxIncomeList.Items.Add($"{income.Name} Price: {income.Price}");
            }

            Recalculation();
        }

        /// <summary>
        /// Deleting incomes item from listBox and from "incomeList" variabe
        /// </summary>
        private void DeleteIncomesItem()
        {
            if (indexOfSelectedFromIncomeListBox != -1)
            {
                string nameFromListBox = incomesList.ElementAt(indexOfSelectedFromIncomeListBox).Name;

                bool flag = false;
                int i = 0;

                while (i < incomesList.Count)
                {
                    Income income = incomesList.ElementAt(i);

                    if (nameFromListBox == income.Name)
                    {
                        flag = true;
                        break;
                    }

                    i++;
                }

                if (flag)
                {
                    incomesList.RemoveAt(i);
                    listBoxIncomeList.Items.RemoveAt(indexOfSelectedFromIncomeListBox);
                    indexOfSelectedFromIncomeListBox = -1;
                    IncomesListRefresh();
                    SaveIncomesData();
                }
            }
        }

        /// <summary>
        /// Adding item to incomesList
        /// </summary>
        private void IncomesAddToList()
        {
            string incomeName = tbIncomeName.Text;
            bool flagToAddElement = true;
            double incomePrice;
            bool parsing = Double.TryParse(tbIncomePrice.Text, out incomePrice);

            foreach (var income in incomesList)
            {
                if (income.Name == tbIncomeName.Text)
                {
                    flagToAddElement = false;
                    break;
                }
            }

            if (flagToAddElement)
            {
                if (parsing && tbIncomeName.Text != String.Empty)
                {
                    incomesList.Add(new Income(tbIncomeName.Text, incomePrice));
                    IncomesListRefresh();
                    SaveIncomesData();
                }
                else
                {
                    MessageBox.Show("Price can be a number only, please retype price", "Sorry", MessageBoxButton.OK);
                    IncomesFieldsRefreshPriceOnly();
                }

            }
            else
            {
                MessageBox.Show($"Income with name \"{incomeName}\" already exist, please type other name", "Sorry", MessageBoxButton.OK);
                IncomesFieldsRefresh();
            }
        }

        /// <summary>
        /// Serealize and save incomes list to XML file
        /// </summary>
        public void SaveIncomesData()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Income>));
            Stream fStream = new FileStream(
                dataIncomes,
                FileMode.Create,
                FileAccess.Write
                );
            xmlFormat.Serialize(fStream, incomesList);
            fStream.Close();
        }


        //OUTCOMES ADD, DELETE, FIELDS REFRESH, LIST REFRESH | methods.-------------------------------------

        /// <summary>
        /// outcomes fields refresh
        /// </summary>
        public void OutcomesFieldsRefresh()
        {
            tbOutcomeName.Text = "Name";
            tbOutcomePrice.Text = "0,0";
            tbOutcomeDate.Text = "day";
            tickBoxIsDirectDebit.IsChecked = false;
        }

        /// <summary>
        /// Add outcome to outcomesList
        /// </summary>
        public void OutcomesAddToList()
        {
            bool isDirectDebit = false;
            string outcomeName = tbOutcomeName.Text;
            bool flagToAddElement = true;
            double outcomePrice;
            int day = 0;
            bool parseDay = false;

            bool parsing = Double.TryParse(tbOutcomePrice.Text, out outcomePrice);

            if (tickBoxIsDirectDebit.IsChecked == true)
            {
                isDirectDebit = true;
                parseDay = Int32.TryParse(tbOutcomeDate.Text, out day);
            }

            foreach (var outcome in outcomesList)
            {
                if (outcome.Name == tbOutcomeName.Text)
                {
                    flagToAddElement = false;
                    break;
                }
            }

            if (flagToAddElement)
            {
                if (parsing && tbOutcomePrice.Text != String.Empty)
                {
                    if (isDirectDebit)
                    {
                        if (parseDay)
                        {
                            outcomesList.Add(new Outcome(tbOutcomeName.Text, outcomePrice, isDirectDebit, day));
                            OutcomesListRefresh();
                            SaveOutcomesData();
                        }
                        else
                        {
                            MessageBox.Show("Date can by only number. Please enter a date of direct debit", "Sorry", MessageBoxButton.OK);
                            OutcomesFieldsRefreshDateeOnly();

                        }
                    }
                    else
                    {
                        outcomesList.Add(new Outcome(tbOutcomeName.Text, outcomePrice, isDirectDebit));
                        OutcomesListRefresh();
                        SaveOutcomesData();
                    }
                }
                else
                {
                    MessageBox.Show("Price can be a number only, please retype price", "Sorry", MessageBoxButton.OK);
                    OutcomesFieldsRefreshPriceOnly();
                }
            }
            else
            {
                MessageBox.Show($"Outcome with name \"{outcomeName}\" already exist, please type other name", "Sorry", MessageBoxButton.OK);
                OutcomesFieldsRefresh();
            }
        }

        /// <summary>
        /// Delete outcome item from list
        /// </summary>
        public void DeleteOutcomesItem()
        {
            if (indexOfSelectedFromOutcomeListBox != -1)
            {
                string nameFromListBox = outcomesList.ElementAt(indexOfSelectedFromOutcomeListBox).Name;

                bool flag = false;
                int i = 0;

                while (i < outcomesList.Count)
                {
                    Outcome outcome = outcomesList.ElementAt(i);

                    if (nameFromListBox == outcome.Name)
                    {
                        flag = true;
                        break;
                    }

                    i++;
                }

                if (flag)
                {
                    outcomesList.RemoveAt(i);
                    listBoxOutcomeList.Items.RemoveAt(indexOfSelectedFromOutcomeListBox);
                    indexOfSelectedFromOutcomeListBox = -1;
                    OutcomesListRefresh();
                    SaveOutcomesData();
                }
            }
        }

        /// <summary>
        /// refresh outcomes list
        /// </summary>
        public void OutcomesListRefresh()
        {
            listBoxOutcomeList.Items.Clear();

            foreach (Outcome outcome in outcomesList)
            {
                bool isDirectDebit = outcome.IsDirectDebit;

                if (isDirectDebit)
                    listBoxOutcomeList.Items.Add($"{outcome.Name} Price: {outcome.Price}  (Direct debit day - {outcome.Day})");
                else
                    listBoxOutcomeList.Items.Add($"{outcome.Name} Price: {outcome.Price}");
            }

            Recalculation();
        }

        /// <summary>
        /// Refresh outcome price textBox only
        /// </summary>
        public void OutcomesFieldsRefreshPriceOnly()
        {
            tbOutcomePrice.Text = "0,0";
        }

        /// <summary>
        /// Refresh outcome date textBox only
        /// </summary>
        public void OutcomesFieldsRefreshDateeOnly()
        {
            tbOutcomeDate.Text = "day";
        }

        /// <summary>
        /// Serealize and save outcomes list to XML file
        /// </summary>
        public void SaveOutcomesData()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Outcome>));
            Stream fStream = new FileStream(
                dataOutcomes,
                FileMode.Create,
                FileAccess.Write
            );
            xmlFormat.Serialize(fStream, outcomesList);
            fStream.Close();
        }


        //CALCULATION---------------------------------------------------------------------------------------

        /// <summary>
        /// Recalculation. Each updates in list this method recalculationg results.
        /// </summary>
        private void Recalculation()
        {
            // IMPORTANT - all counting is in separate whiles and variables
            // I done this way special, it will alowe you to customize this method easyle if need

            //Counting number of incomes
            int incomesTotal = incomesList.Count;
            //Show number of all incomes
            lblIncomesTotal.Content = $"Incomes total: {incomesTotal}";

            // Summing all prices from incomes list
            double sumOfAllIncomes = 0;
            foreach (var income in incomesList)
                sumOfAllIncomes += income.Price;
            //Show number of all incomes
            lblSumOfAllIncomes.Content = $"Sum of all incomes £: {sumOfAllIncomes}";


            //Counting number of outcomes
            int outcomesTotal = outcomesList.Count;
            //Show number of all outcomes
            lblOutcomesTotal.Content = $"Outcomes total: {outcomesTotal}";

            // Summing all prices from outcomes list
            double sumOfAllOutcomes = 0;
            foreach (var outcome in outcomesList)
                sumOfAllOutcomes += outcome.Price;
            //Show number of all outcomes
            lblSumOfAllOutcomes.Content = $"Sum of all outcomes £: {sumOfAllOutcomes}";

            //Count direct debits only
            int directDebitsTotal = 0;
            double sumOfAllDirectDebits = 0;
            foreach (var outcome in outcomesList)
            {
                if (outcome.IsDirectDebit)
                {
                    directDebitsTotal += 1;
                    sumOfAllDirectDebits += outcome.Price;
                }
            }

            lblDirectDebitTotal.Content = $"Direct debits total: {directDebitsTotal}";
            lblSumOfAllDirectDebits.Content = $"Sum of all direct debits £: {sumOfAllDirectDebits}";

            //Count sum of all outcomes NOT direct Debits
            double sumOfAllOutcomesNotDirectDebits = 0;
            foreach (var outcome in outcomesList)
            {
                if (!(outcome.IsDirectDebit))
                {
                    sumOfAllOutcomesNotDirectDebits += outcome.Price;
                }
            }
            lblSumOfAllOutcomesNotDirectDebits.Content = $"Sum of all outcomes NOT direct debits £: {sumOfAllOutcomesNotDirectDebits}";

            //Count total price to pay
            double totalToPay = 0;
            foreach (var outcome in outcomesList)
            {
                totalToPay += outcome.Price;
            }

            lblToPayTotal.Content = $"To pay total £: {totalToPay}";

            //Count Money left
            double sumAllOutcomes = 0;
            double sumAllIncomes = 0;
            foreach (var outcome in outcomesList)
            {
                sumAllOutcomes += outcome.Price;
            }

            foreach (var income in incomesList)
            {
                sumAllIncomes += income.Price;
            }

            double moneyLeft = sumAllIncomes - sumAllOutcomes;
            lblMoneyLeft.Content = $"Money left £: {moneyLeft}";
        }

        //LOAD ON START
        /// <summary>
        /// Loading data from xml files to lists on program start
        /// </summary>
        public void LoadDataOnStart()
        {
            //Protecting first run. At first run xml files have not data inside and this give exception
            try
            {
                //Deserialize incomes list from xml file to incomes list
                XmlSerializer xmlFormatIncomes = new XmlSerializer(typeof(List<Income>));
                Stream fStreamIncomes = new FileStream(
                    dataIncomes,
                    FileMode.Open,
                    FileAccess.Read
                );
                incomesList = (List<Income>)xmlFormatIncomes.Deserialize(fStreamIncomes);
                fStreamIncomes.Close();
                IncomesListRefresh();

                //Deserialize outcomes list from xml file to outcomes list
                XmlSerializer xmlFormatOutcomes = new XmlSerializer(typeof(List<Outcome>));
                Stream fStreamOutcomes = new FileStream(
                    dataOutcomes,
                    FileMode.Open,
                    FileAccess.Read
                );
                outcomesList = (List<Outcome>)xmlFormatOutcomes.Deserialize(fStreamOutcomes);
                fStreamOutcomes.Close();
                OutcomesListRefresh();
            }
            catch (Exception) { }
        }



        // DIALOG WINDOW------------------------------------------------------------------------------------

        /// <summary>
        /// Add outcome direct debit from calendar, if some day was selected
        /// </summary>
        /// <param name="sender"></param>
        public void AddOutcomeToListFromDialog(object sender)
        {
            dateFromCalendarSelected = sender.ToString();

            AddDirectDebitDialog addDirectDebitDialog = new AddDirectDebitDialog();

            if (addDirectDebitDialog.ShowDialog() == true)
            {
                string[] arr = addDirectDebitDialog.GetDataFromDialog;
                OutcomesAddToListFromDialog(arr[0], arr[1]);

            }
        }

        /// <summary>
        /// Add Direct Debit Outcome from dialog window.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="price"></param>
        public void OutcomesAddToListFromDialog(string name, string price)
        {
            bool isDirectDebit = true; //Leaved this variablo to be more easy read source
            string outcomeName = name;
            bool flagToAddElement = true;
            double outcomePrice;

            string[] dateSplited = dateFromCalendarSelected.Split(' ');
            string[] dateSplited2 = dateSplited[0].Split('.');
            int day = Int32.Parse(dateSplited2[0]);

            bool parsing = Double.TryParse(price, out outcomePrice);


            foreach (var outcome in outcomesList)
            {
                if (outcome.Name == tbOutcomeName.Text)
                {
                    flagToAddElement = false;
                    break;
                }
            }

            if (flagToAddElement)
            {
                if (parsing && tbOutcomePrice.Text != String.Empty)
                {
                    outcomesList.Add(new Outcome(name, outcomePrice, isDirectDebit, day));
                    OutcomesListRefresh();
                    SaveOutcomesData();
                }
                else
                {
                    MessageBox.Show("Price can be a number only, please retype price", "Sorry", MessageBoxButton.OK);
                    OutcomesFieldsRefreshPriceOnly();
                }
            }
            else
            {
                MessageBox.Show($"Outcome with name \"{outcomeName}\" already exist, please type other name", "Sorry", MessageBoxButton.OK);
                OutcomesFieldsRefresh();
            }
        }
    }
}
