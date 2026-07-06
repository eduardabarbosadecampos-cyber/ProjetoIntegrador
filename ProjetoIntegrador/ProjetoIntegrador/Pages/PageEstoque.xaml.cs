using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageEstoque.xam
    /// </summary>
    public partial class PageEstoque : UserControl
    {
        public PageEstoque()
        {
            InitializeComponent();
            CarregarEstoque();
        }

        private void CarregarEstoque()
        {
            dgEstoque.ItemsSource = Dados.Produtos.ToList();
        }

        private void btnAtualizar_Click(object sender, RoutedEventArgs e)
        {
            txtPesquisar.Clear();
            CarregarEstoque();
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            string termo = txtPesquisar.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(termo))
            {
                CarregarEstoque();
                return;
            }

            dgEstoque.ItemsSource = Dados.Produtos
                .Where(p => p.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)
                         || p.Cor.Contains(termo, StringComparison.OrdinalIgnoreCase)
                         || p.Codigo.ToString() == termo)
                .ToList();
        }
    }
}
