using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageFinanceiro.xam
    /// </summary>
    public partial class PageFinanceiro : UserControl
    {
        public PageFinanceiro()
        {
            InitializeComponent();
            Carregar(Dados.Movimentos);
        }

        private void Carregar(IEnumerable<Movimento> movs)
        {
            var lista = movs.ToList();

            decimal entradas = lista.Where(m => m.Tipo == "Entrada").Sum(m => m.Valor);
            decimal saidas = lista.Where(m => m.Tipo == "Saída").Sum(m => m.Valor);

            txtEntradas.Text = entradas.ToString("C");
            txtSaidas.Text = saidas.ToString("C");
            txtLucro.Text = (entradas - saidas).ToString("C");

            dgMovimentos.ItemsSource = lista;
        }

        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            string filtro = (cbMovimentacao.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Todas";

            IEnumerable<Movimento> movs = Dados.Movimentos;
            if (filtro == "Entradas")
                movs = movs.Where(m => m.Tipo == "Entrada");
            else if (filtro == "Saídas")
                movs = movs.Where(m => m.Tipo == "Saída");

            Carregar(movs);
        }
    }
}
