using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace jednoreki
{
    public partial class MainWindow : Window
    {
        private readonly string[] icons = {
            "Images/cherry.jpg",
            "Images/grape.jpg",
            "Images/lemon.jpg",
            "Images/seven.jpg"
        };

        private readonly Random random = new Random();

        // saldo gracza
        private decimal saldo = 10000;

        public MainWindow()
        {
            InitializeComponent();

            // startowe symbole
            SetSlot(Slot1Shape, icons[0]);
            SetSlot(Slot2Shape, icons[1]);
            SetSlot(Slot3Shape, icons[2]);

            WygranaText.Text = "0 zł";

            // dodajemy saldo nad dolnym panelem
            ShowSaldo();
        }

        private void Spin_Click(object sender, RoutedEventArgs e)
        {
            // walidacja stawki
            if (!decimal.TryParse(StawkaTextBox.Text, out decimal stawka) || stawka <= 0)
            {
                MessageBox.Show("Wpisz poprawną stawkę (liczba > 0).", "Błąd stawki");
                return;
            }

            // sprawdź czy gracz ma wystarczająco salda
            if (stawka > saldo)
            {
                MessageBox.Show("Nie masz tyle pieniędzy!", "Brak środków");
                return;
            }

            // odejmujemy stawkę od salda
            saldo -= stawka;

            // losowanie symboli
            string s1 = icons[random.Next(icons.Length)];
            string s2 = icons[random.Next(icons.Length)];
            string s3 = icons[random.Next(icons.Length)];

            SetSlot(Slot1Shape, s1);
            SetSlot(Slot2Shape, s2);
            SetSlot(Slot3Shape, s3);

            // sprawdzanie wygranej
            if (s1 == s2 && s2 == s3)
            {
                decimal wygrana = stawka * 5;
                WygranaText.Text = $"{wygrana} zł";
                WygranaText.Foreground = Brushes.Gold;

                // dodajemy wygraną do salda
                saldo += wygrana;

                MessageBox.Show($"🎉 Wygrałeś {wygrana} zł!", "JACKPOT!");
            }
            else
            {
                WygranaText.Text = "0 zł";
                WygranaText.Foreground = Brushes.White;
            }

            // odśwież saldo
            ShowSaldo();
        }

        private void Regulamin_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "REGULAMIN\n\n" +
                "1. Gra ma charakter demonstracyjny.\n" +
                "2. Wygrana tylko przy 3 takich samych symbolach.\n" +
                "3. Stawkę ustala gracz.\n" +
                "4. Projekt edukacyjny.",
                "Regulamin",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void Zasady_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "ZASADY GRY\n\n" +
                "1. Wpisz stawkę.\n" +
                "2. Kliknij LOSUJ.\n" +
                "3. 3 takie same symbole = wygrana.\n" +
                "4. Wygrana = stawka x5.",
                "Zasady gry",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void SetSlot(Rectangle slot, string imagePath)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{imagePath}");
                slot.Fill = new ImageBrush(new BitmapImage(uri))
                {
                    Stretch = Stretch.UniformToFill
                };
            }
            catch
            {
                slot.Fill = Brushes.Black;
            }
        }

        // wyświetla saldo w dolnym panelu
        private void ShowSaldo()
        {
            // jeśli nie ma TextBlocka na saldo, dodajemy go dynamicznie
            if (this.FindName("SaldoText") == null)
            {
                var saldoText = new System.Windows.Controls.TextBlock()
                {
                    Name = "SaldoText",
                    Text = $"Saldo: {saldo} zł",
                    Foreground = Brushes.Gold,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 6)
                };

                // dodajemy nad dolnym panelem
                Grid.SetRow(saldoText, 1);
                ((Grid)this.Content).Children.Add(saldoText);
            }
            else
            {
                var saldoText = (System.Windows.Controls.TextBlock)this.FindName("SaldoText");
                saldoText.Text = $"Saldo: {saldo} zł";
            }
        }
    }
}
