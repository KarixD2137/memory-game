using System.Collections.Immutable;
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
using System.Windows.Threading;

namespace GraMemory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Karta[] Karty = new Karta[16];
        List<String> ObrazkiDoWylosowania = new List<string>();

        Karta pierwszaOdslonieta;
        Karta drugaOdslonieta;
        int pierwszaOdslonietaId;
        int drugaOdslonietaId;
        int odslaniana = 1;
        int zgadniete = 0;
        int odkryteKarty = 0;
        DispatcherTimer timer;
        bool timerWlaczony = false;
        int czas = 0;

        public MainWindow()
        {
            InitializeComponent();

            rozmiescKarty();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
        }

        public void wylosuj()
        {
            Random random = new Random();
            for (int i = 15; i >= 0; i--)
            {
                int wylosowanyObrazek = random.Next(0, i);
                Karty[i].sciezka = ObrazkiDoWylosowania[wylosowanyObrazek];
                ObrazkiDoWylosowania.RemoveAt(wylosowanyObrazek);
            }
        }

        public void rozmiescKarty()
        {
            Karty[0] = new Karta(obr0);
            Karty[1] = new Karta(obr1);
            Karty[2] = new Karta(obr2);
            Karty[3] = new Karta(obr3);
            Karty[4] = new Karta(obr4);
            Karty[5] = new Karta(obr5);
            Karty[6] = new Karta(obr6);
            Karty[7] = new Karta(obr7);
            Karty[8] = new Karta(obr8);
            Karty[9] = new Karta(obr9);
            Karty[10] = new Karta(obr10);
            Karty[11] = new Karta(obr11);
            Karty[12] = new Karta(obr12);
            Karty[13] = new Karta(obr13);
            Karty[14] = new Karta(obr14);
            Karty[15] = new Karta(obr15);

            ObrazkiDoWylosowania.Add("img/0.png");
            ObrazkiDoWylosowania.Add("img/1.png");
            ObrazkiDoWylosowania.Add("img/2.png");
            ObrazkiDoWylosowania.Add("img/3.png");
            ObrazkiDoWylosowania.Add("img/4.png");
            ObrazkiDoWylosowania.Add("img/5.png");
            ObrazkiDoWylosowania.Add("img/6.png");
            ObrazkiDoWylosowania.Add("img/7.png");
            ObrazkiDoWylosowania.Add("img/0.png");
            ObrazkiDoWylosowania.Add("img/1.png");
            ObrazkiDoWylosowania.Add("img/2.png");
            ObrazkiDoWylosowania.Add("img/3.png");
            ObrazkiDoWylosowania.Add("img/4.png");
            ObrazkiDoWylosowania.Add("img/5.png");
            ObrazkiDoWylosowania.Add("img/6.png");
            ObrazkiDoWylosowania.Add("img/7.png");

            wylosuj();

            foreach (Karta karta in Karty)
            {
                karta.obrazek.Source = new BitmapImage(new Uri("img/rewers.png", UriKind.Relative));
            }
        }

        public void odwrocKarte(int idKarty)
        {
            if (!timerWlaczony)
            {
                timer.Interval = TimeSpan.FromSeconds(1);
                lbl_czas.Content = "⏱ 00:00";
                timer.Start();
                timerWlaczony = true;
            }
            Karta karta = Karty[idKarty];
            if (!karta.zgadnieta)
            {
                if (odslaniana == 1)
                {
                    if (pierwszaOdslonieta != null && drugaOdslonieta != null)
                    {
                        if (pierwszaOdslonieta.sciezka != drugaOdslonieta.sciezka)
                        {
                            Karty[pierwszaOdslonietaId].obrazek.Source = new BitmapImage(new Uri("img/rewers.png", UriKind.Relative));
                            Karty[drugaOdslonietaId].obrazek.Source = new BitmapImage(new Uri("img/rewers.png", UriKind.Relative));
                            pierwszaOdslonieta = null;
                            drugaOdslonieta = null;
                        }
                    }
                    pierwszaOdslonieta = karta;
                    pierwszaOdslonietaId = idKarty;
                    odslaniana = 2;
                }
                else
                {  
                    if (karta != pierwszaOdslonieta)
                    {
                        odkryteKarty++;
                        lbl_ilosc_kart.Content = "🔼 " + odkryteKarty;
                        drugaOdslonieta = karta;
                        drugaOdslonietaId = idKarty;
                        if (pierwszaOdslonieta.sciezka == drugaOdslonieta.sciezka)
                        {
                            pierwszaOdslonieta.zgadnieta = true;
                            drugaOdslonieta.zgadnieta = true;
                            zgadniete++;
                        }
                        odslaniana = 1;
                    }
                }
                karta.obrazek.Source = new BitmapImage(new Uri(karta.sciezka, UriKind.Relative));
                if (zgadniete == 8)
                {
                    timer.Stop();
                    MessageBoxResult rezultat = MessageBox.Show("Gratulacje! Udało ci się odnaleźć wszystkie karty!\nTwój czas: " + sformatujCzas(czas) + "\nOdsłonięte pary: " + odkryteKarty + "\nCzy chcesz zagrać jeszcze raz?", "Gra Memory", MessageBoxButton.YesNo);
                    if (rezultat == MessageBoxResult.Yes)
                    {
                        ZresetujGre();
                    }
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            czas++;
            string sformatowanyCzas = sformatujCzas(czas);
            lbl_czas.Content = "⏱ " + sformatowanyCzas;
        }

        string sformatujCzas(int czas)
        {
            int sekundy = czas % 60;
            int minuty = czas / 60;
            if (sekundy < 10)
            {
                if (minuty < 10)
                {
                     return "0" + minuty + ":0" + sekundy;
                }
                else
                {
                    return minuty + ":0" + sekundy;
                }
            }
            else
            {
                if (minuty < 10)
                {
                    return "0" + minuty + ":" + sekundy;
                }
                else
                {
                    return minuty + ":" + sekundy;
                }
            }
        }

        private void ZresetujGre()
        {
            timer.Stop();
            timerWlaczony = false;
            czas = 0;
            odslaniana = 1;
            zgadniete = 0;
            odkryteKarty = 0;
            lbl_czas.Content = "⏱ 00:00";
            lbl_ilosc_kart.Content = "🔼 " + odkryteKarty;
            rozmiescKarty();
        }

        private void Karta0(object sender, RoutedEventArgs e)
        {
            odwrocKarte(0);
        }

        private void Karta1(object sender, RoutedEventArgs e)
        {
            odwrocKarte(1);
        }

        private void Karta2(object sender, RoutedEventArgs e)
        {
            odwrocKarte(2);
        }

        private void Karta3(object sender, RoutedEventArgs e)
        {
            odwrocKarte(3);
        }

        private void Karta4(object sender, RoutedEventArgs e)
        {
            odwrocKarte(4);
        }

        private void Karta5(object sender, RoutedEventArgs e)
        {
            odwrocKarte(5);
        }

        private void Karta6(object sender, RoutedEventArgs e)
        {
            odwrocKarte(6);
        }

        private void Karta7(object sender, RoutedEventArgs e)
        {
            odwrocKarte(7);
        }

        private void Karta8(object sender, RoutedEventArgs e)
        {
            odwrocKarte(8);
        }

        private void Karta9(object sender, RoutedEventArgs e)
        {
            odwrocKarte(9);
        }

        private void Karta10(object sender, RoutedEventArgs e)
        {
            odwrocKarte(10);
        }

        private void Karta11(object sender, RoutedEventArgs e)
        {
            odwrocKarte(11);
        }

        private void Karta12(object sender, RoutedEventArgs e)
        {
            odwrocKarte(12);
        }

        private void Karta13(object sender, RoutedEventArgs e)
        {
            odwrocKarte(13);
        }

        private void Karta14(object sender, RoutedEventArgs e)
        {
            odwrocKarte(14);
        }

        private void Karta15(object sender, RoutedEventArgs e)
        {
            odwrocKarte(15);
        }

        private void btn_reset(object sender, RoutedEventArgs e)
        {
            ZresetujGre();
        }
    }
}