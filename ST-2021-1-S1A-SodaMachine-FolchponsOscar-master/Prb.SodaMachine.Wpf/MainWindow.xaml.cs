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

namespace Prb.SodaMachine.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<decimal> PrijzenList = new List<decimal> { 1.5M, 1.5M, 2 };
        //Index van prijzen laten overeenkomen met drank
        List<int> AantalDrinks = new List<int> { 0, 0, 0 };
        List<string> drinks = new List<string> { "Cola", "Sprite", "Iced Tea"};
        Decimal wisselgeld;
        Decimal GeldigePrijs;
        int d = 0;


        enum WorkingStates
        {
            canAddProduct,
            editProduct,
            readyToOrder,
            productSelected,
            productDelivered
        }

        public MainWindow()
        {
            InitializeComponent();
            KeuzeLst.Items.Add("Cola - ‎€ 1,5");
            KeuzeLst.Items.Add("Sprite - ‎€ 1,5");
            KeuzeLst.Items.Add("Iced Tea - ‎€ 2");

           
            InworpLst.Items.Add("0,10");
            InworpLst.Items.Add("0,20");
            InworpLst.Items.Add("0,50");
            InworpLst.Items.Add("1");
            InworpLst.Items.Add("2");
                                       
           ChangeWorkingMode(WorkingStates.readyToOrder);
          
        }

        void ChangeWorkingMode(WorkingStates workingState)
        {
            //Lijst van elementen die verdwijnen voor elke mogelijke workingstate
            List<FrameworkElement> StateZero = new List<FrameworkElement>
            {InworpLst,AnBestellingBtn, CheersBtn, wisselgeldLbl,SaldoLbl, BeheerGrd};
            List<FrameworkElement> StateOne = new List<FrameworkElement>
            { };
            List<FrameworkElement> StateTwo = new List<FrameworkElement>
            { StatistiekLbl};
            List<FrameworkElement> StateThree = new List<FrameworkElement>
            { InworpLst, AnBestellingBtn, wisselgeldLbl, SaldoLbl};
            List<FrameworkElement> StateFour = new List<FrameworkElement>
            { InworpLst,AnBestellingBtn, CheersBtn, wisselgeldLbl,SaldoLbl};
            List<FrameworkElement> Allelements = new List<FrameworkElement>
            { KeuzeLst,InworpLst, AnBestellingBtn, CheersBtn, StatistiekLbl, BeheerGrd, BeheerBtn, StatistiekLbl };
            // Lijst om alle elementen te enabelen

            foreach (FrameworkElement e in Allelements)
            {
                e.IsEnabled = true;
                //StatistiekLbl.Visibility = Visibility.Hidden;
            }

            if (workingState == WorkingStates.readyToOrder)
            {
                foreach (FrameworkElement e in StateZero)
                {
                    e.IsEnabled = false;
                }

            }
            else if (workingState == WorkingStates.editProduct)
            {
                foreach (FrameworkElement e in StateOne)
                {
                    e.IsEnabled = false;
                }
            }
            else if (workingState == WorkingStates.productDelivered)
            {
                foreach (FrameworkElement e in StateTwo)
                {
                    //e.Visibility = Visibility.Visible;
                }
            }
            else if (workingState == WorkingStates.productSelected)
            {
                foreach (FrameworkElement e in StateThree)
                {
                    e.IsEnabled = true;
                }
            }
            else if (workingState == WorkingStates.canAddProduct)
            {
                foreach (FrameworkElement e in StateFour)
                {
                    e.IsEnabled = false;
                }
            }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeWorkingMode(WorkingStates.productSelected);
            GeldigePrijs = PrijzenList[KeuzeLst.SelectedIndex];
            SaldoLbl.Content = ("Balance: € " + GeldigePrijs);
                               
        }

        private void BeheerBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWorkingMode(WorkingStates.canAddProduct);
        }

        private void BeheerGrd_Initialized(object sender, EventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PrijsLbl.Text == "")
            {
                FeedbackTxtB.Text = "Geef een geldige prijs";
            }
            else
            {
                try
                {
                    PrijzenList.Add(Convert.ToDecimal(PrijsLbl.Text));
                    KeuzeLst.Items.Add(ProductLbl.Text + " - € " + PrijsLbl.Text);
                    drinks.Add(ProductLbl.Text);
                    AantalDrinks.Add(0);
                    PrijzenList.Add(Convert.ToDecimal (PrijsLbl.Text));
                }
                catch { FeedbackTxtB.Text = "Geef een geldige prijs"; }

            }
           
            ProductLbl.Text = "";
            PrijsLbl.Text = "";
            //prijs toevoegen aan prijzen List

            ChangeWorkingMode(WorkingStates.readyToOrder);
        }

        private void AnnuleerBtn_Click(object sender, RoutedEventArgs e)
        {
            ProductLbl.Text = "";
            PrijsLbl.Text = "";
        }

        private void CheersBtn_Click(object sender, RoutedEventArgs e)
        {
            StatistiekLbl.Text = "";
            ChangeWorkingMode(WorkingStates.readyToOrder);
            wisselgeld = 0;
            wisselgeldLbl.Content = wisselgeld;
            ChangeWorkingMode(WorkingStates.productDelivered);
            AantalDrinks[KeuzeLst.SelectedIndex] = AantalDrinks[KeuzeLst.SelectedIndex] + 1;
            d = 0;
            while (d < drinks.Count)
            {
                StatistiekLbl.Text = StatistiekLbl.Text + "\n" + AantalDrinks[d] + " x " + drinks[d] + " € " + PrijzenList[d] * AantalDrinks[d];
                d++;
            }
            
        }

        private void AnBestellingBtn_Click(object sender, RoutedEventArgs e)
        {
            ChangeWorkingMode(WorkingStates.readyToOrder);
            wisselgeld = 0;
            wisselgeldLbl.Content = wisselgeld;
            StatistiekLbl.Text = "";
        }

        private void InworpLst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void InworpLst_MouseUp(object sender, MouseButtonEventArgs e)
        {
            GeldigePrijs = GeldigePrijs - Convert.ToDecimal(InworpLst.SelectedItem);
            SaldoLbl.Content = "Saldo: € " + GeldigePrijs;
            if (Convert.ToDecimal(InworpLst.SelectedItem) >= GeldigePrijs)
            {

                wisselgeld = GeldigePrijs;
                SaldoLbl.Content = "je hebt betaald";
                wisselgeldLbl.Content = wisselgeld;
            }

           /*int tempwisselgeld =  Convert.ToInt32(10 * wisselgeld);

            

                if (Math.Truncate((double) tempwisselgeld / 20) != 0)
                {
                    wisselgeldLbl.Content = tempwisselgeld / 20 + " * 2";
                    tempwisselgeld = tempwisselgeld % 20;
                }
                if (Math.Truncate((double)tempwisselgeld / 10) != 0)
                {
                    wisselgeldLbl.Content = wisselgeldLbl.Content + "\n" + tempwisselgeld / 10 + " * 1";
                    tempwisselgeld = tempwisselgeld % 10;
                }
                if (Math.Truncate((double)tempwisselgeld / 5) != 0)
                {
                    wisselgeldLbl.Content = wisselgeldLbl.Content + "\n" + tempwisselgeld / 5 + " * 0,5";
                    tempwisselgeld = tempwisselgeld % 5 ;
                }
                if (Math.Truncate((double)tempwisselgeld / 2)!= 0)
                {
                    wisselgeldLbl.Content = wisselgeldLbl.Content + "\n" + tempwisselgeld / 2 + " * 0,2";
                    tempwisselgeld = tempwisselgeld % 2;
                }
                if (Math.Truncate((double)tempwisselgeld / 1) != 0)
                {
                    wisselgeldLbl.Content = wisselgeldLbl.Content + "\n" + tempwisselgeld / 1 + " * 0,1";
                    tempwisselgeld = tempwisselgeld % 1;
                }*/
            
            
        }

        private void StatistiekLbl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StatistiekLbl_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
