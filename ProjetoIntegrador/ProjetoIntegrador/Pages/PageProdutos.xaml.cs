using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageProdutos.xam
    /// </summary>
    public partial class PageProdutos : UserControl
    {
        private string _imagem;
        public PageProdutos()
        {
            InitializeComponent();
        }

        private void BtnSelecionarImagem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Title = "Selecione uma imagem";
            dialog.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if (dialog.ShowDialog() == true)
            {
                string origem = dialog.FileName;

                string pastaDestino = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                if (!Directory.Exists(pastaDestino))
                    Directory.CreateDirectory(pastaDestino);

                string destino = System.IO.Path.Combine(pastaDestino, System.IO.Path.GetFileName(origem));

                File.Copy(origem, destino, true);

                _imagem = destino;
            }
        }

        private void BtnCadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbCodigoProd.Text) || string.IsNullOrEmpty(tbProduto.Text) || string.IsNullOrEmpty(tbPreco.Text))
                return;


            gridProdutos.Children.Add(CriarCardProduto(_imagem, Convert.ToInt32(tbCodigoProd.Text), tbProduto.Text, Convert.ToDecimal(tbPreco.Text)));

        }

        private Border CriarCardProduto(string imagem, int codigo, string nome, decimal preco)
        {
            Border border = new Border
            {
                Width = 180,
                Height = 250,
                Background = Brushes.White,
                CornerRadius = new CornerRadius(10),
                Margin = new Thickness(10),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            StackPanel stack = new StackPanel();

            stack.Children.Add(new Image
            {
                Height = 150,
                Stretch = Stretch.Uniform,
                Margin = new Thickness(10),
                Source = new BitmapImage(new Uri(imagem, UriKind.Absolute))
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"#{codigo.ToString()}",
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            });

            stack.Children.Add(new TextBlock
            {
                Text = nome,
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(5)
            });

            stack.Children.Add(new TextBlock
            {
                Text = preco.ToString("C"),
                Foreground = Brushes.Green,
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                TextAlignment = TextAlignment.Center
            });

            border.Child = stack;

            return border;
        }
    }
}
