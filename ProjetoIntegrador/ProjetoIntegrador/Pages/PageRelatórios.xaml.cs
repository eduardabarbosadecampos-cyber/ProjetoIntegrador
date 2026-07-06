using System;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageRelatórios.xam
    /// </summary>
    public partial class PageRelatórios : UserControl
    {
        public PageRelatórios()
        {
            InitializeComponent();
            CarregarIndicadores();
        }

        private void CarregarIndicadores()
        {
            txtTotalVendas.Text = Dados.TotalVendido.ToString("C");
            txtQuantidade.Text = Dados.QtdVendida.ToString();
            txtMaisVendido.Text = Dados.ProdutoMaisVendido();
        }
    }
}
