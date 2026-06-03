using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Jogo_da_Memória
{
    public partial class MainWindow : Window
    {
        private bool isChecking = false;
        private Button firstCard = null;
        private Button secondCard = null;
        private int matchesFound = 0;
        private const int TotalPairs = 8;

        private List<Button> allCards;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            allCards = new List<Button>
            {
                Card1, Card1_Copiar, Card1_Copiar1, Card1_Copiar2,
                Card1_Copiar3, Card1_Copiar4, Card1_Copiar5, Card1_Copiar6,
                Card1_Copiar7, Card1_Copiar8, Card1_Copiar9, Card1_Copiar10,
                Card1_Copiar11, Card1_Copiar12, Card1_Copiar13, Card1_Copiar14
            };

            var emojis = new List<string>
            {
                "😂","😂",
                "😇","😇",
                "😤","😤",
                "🥶","🥶",
                "👻","👻",
                "😏","😏",
                "😻","😻",
                "🥳","🥳"
            };

            Shuffle(emojis);

            for (int i = 0; i < allCards.Count; i++)
            {
                allCards[i].Tag = emojis[i];
            }

            foreach (var card in allCards)
            {
                var text = FindVisualChild<TextBlock>(card);

                if (text != null)
                    text.Text = "?";

                card.Background = new SolidColorBrush(Color.FromRgb(90, 158, 122));
                card.IsEnabled = true;
            }

            firstCard = null;
            secondCard = null;
            isChecking = false;
            matchesFound = 0;
        }

        private static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (isChecking)
                return;

            Button card = sender as Button;

            if (card == null || !card.IsEnabled)
                return;

            TextBlock text = FindVisualChild<TextBlock>(card);

            if (text == null || text.Text != "?")
                return;

            FlipCard(card, revealedCard =>
            {
                if (firstCard == null)
                {
                    firstCard = revealedCard;
                }
                else if (secondCard == null && revealedCard != firstCard)
                {
                    secondCard = revealedCard;
                    CheckMatch();
                }
            });
        }

        private void FlipCard(Button card, Action<Button> onComplete)
        {
            ScaleTransform scale = card.RenderTransform as ScaleTransform;

            if (scale == null)
                return;

            DoubleAnimation shrink =
                new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));

            shrink.Completed += (s, e) =>
            {
                TextBlock text = FindVisualChild<TextBlock>(card);

                if (text != null)
                    text.Text = card.Tag.ToString();

                card.Background =
                    new SolidColorBrush(Color.FromRgb(34, 85, 60));

                DoubleAnimation grow =
                    new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));

                grow.Completed += (s2, e2) =>
                {
                    onComplete?.Invoke(card);
                };

                scale.BeginAnimation(ScaleTransform.ScaleXProperty, grow);
            };

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, shrink);
        }

        private void CheckMatch()
        {
            isChecking = true;

            if (firstCard.Tag.ToString() == secondCard.Tag.ToString())
            {
                firstCard.IsEnabled = false;
                secondCard.IsEnabled = false;

                firstCard = null;
                secondCard = null;

                matchesFound++;
                isChecking = false;

                if (matchesFound == TotalPairs)
                {
                    ShowVictory();
                }
            }
            else
            {
                Button card1 = firstCard;
                Button card2 = secondCard;

                firstCard = null;
                secondCard = null;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);

                timer.Tick += (s, e) =>
                {
                    timer.Stop();

                    FlipBack(card1);
                    FlipBack(card2);

                    isChecking = false;
                };

                timer.Start();
            }
        }

        private void FlipBack(Button card)
        {
            ScaleTransform scale = card.RenderTransform as ScaleTransform;

            if (scale == null)
                return;

            DoubleAnimation shrink =
                new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));

            shrink.Completed += (s, e) =>
            {
                TextBlock text = FindVisualChild<TextBlock>(card);

                if (text != null)
                    text.Text = "?";

                card.Background =
                    new SolidColorBrush(Color.FromRgb(90, 158, 122));

                DoubleAnimation grow =
                    new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));

                scale.BeginAnimation(ScaleTransform.ScaleXProperty, grow);
            };

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, shrink);
        }

        private void ShowVictory()
        {
            MessageBoxResult result = MessageBox.Show(
                "🎉 Parabéns! Você encontrou todos os pares!\n\nDeseja jogar novamente?",
                "Você Ganhou!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                InitializeGame();
            }
        }

        private T FindVisualChild<T>(DependencyObject parent)
            where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child =
                    VisualTreeHelper.GetChild(parent, i);

                if (child is T)
                    return (T)child;

                T found = FindVisualChild<T>(child);

                if (found != null)
                    return found;
            }

            return null;
        }
    }
}