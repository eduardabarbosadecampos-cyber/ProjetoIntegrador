using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageProdutos.xam
    /// </summary>
    public partial class PageProdutos : UserControl
    {
        private string? _imagem;

        public PageProdutos()
        {
            InitializeComponent();
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            gridProdutos.Children.Clear();
            foreach (var p in Dados.Produtos)
                gridProdutos.Children.Add(CriarCardProduto(p.Imagem, p.Codigo, p.Nome, p.Preco));
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
            if (string.IsNullOrWhiteSpace(tbCodigoProd.Text) ||
                string.IsNullOrWhiteSpace(tbProduto.Text) ||
                string.IsNullOrWhiteSpace(tbPreco.Text))
            {
                MessageBox.Show("Preencha código, produto e preço.");
                return;
            }

            if (!int.TryParse(tbCodigoProd.Text, out int codigo))
            {
                MessageBox.Show("Código inválido.");
                return;
            }

            if (Dados.BuscarProduto(codigo) != null)
            {
                MessageBox.Show($"Já existe um produto com o código {codigo}.");
                return;
            }

            if (!decimal.TryParse(tbPreco.Text, out decimal preco))
            {
                MessageBox.Show("Preço inválido.");
                return;
            }

            int.TryParse(tbEstoque.Text, out int estoque);

            var produto = new Produto
            {
                Codigo = codigo,
                Nome = tbProduto.Text,
                Genero = (cbGenero.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                Tamanho = (cbTamanho.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                Preco = preco,
                Estoque = estoque,
                Imagem = _imagem
            };

            Dados.Produtos.Add(produto);
            Dados.Salvar();

            gridProdutos.Children.Add(CriarCardProduto(_imagem, codigo, produto.Nome, preco));

            _imagem = null;
            tbCodigoProd.Clear();
            tbProduto.Clear();
            tbPreco.Clear();
            tbEstoque.Clear();
        }

        private Border CriarCardProduto(string? imagem, int codigo, string nome, decimal preco)
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

            if (!string.IsNullOrEmpty(imagem) && File.Exists(imagem))
            {
                stack.Children.Add(new Image
                {
                    Height = 150,
                    Stretch = Stretch.Uniform,
                    Margin = new Thickness(10),
                    Source = new BitmapImage(new Uri(imagem, UriKind.Absolute))
                });
            }
            else
            {
                stack.Children.Add(new TextBlock
                {
                    Text = "📦",
                    FontSize = 90,
                    Height = 150,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(10)
                });
            }

            stack.Children.Add(new TextBlock
            {
                Text = $"#{codigo}",
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
