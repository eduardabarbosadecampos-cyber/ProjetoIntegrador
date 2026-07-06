using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageClientes.xam
    /// </summary>
    public partial class PageClientes : UserControl
    {
        public PageClientes()
        {
            InitializeComponent();
            AtualizarGrid();
        }

        private void AtualizarGrid()
        {
            dgClientes.ItemsSource = null;
            dgClientes.ItemsSource = Dados.Clientes;
        }

        private void BtnNovoCliente_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNome.Text))
            {
                MessageBox.Show("Informe o nome do cliente.");
                return;
            }

            Dados.Clientes.Add(new Cliente
            {
                Id = Dados.ProximoIdCliente(),
                Nome = tbNome.Text,
                Telefone = tbTelefone.Text,
                CPF = tbCPF.Text,
                Cidade = tbCidade.Text
            });

            Dados.Salvar();
            AtualizarGrid();
            LimparCampos();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is not Cliente cli)
            {
                MessageBox.Show("Selecione um cliente na tabela para editar.");
                return;
            }

            if (!string.IsNullOrWhiteSpace(tbNome.Text)) cli.Nome = tbNome.Text;
            if (!string.IsNullOrWhiteSpace(tbTelefone.Text)) cli.Telefone = tbTelefone.Text;
            if (!string.IsNullOrWhiteSpace(tbCPF.Text)) cli.CPF = tbCPF.Text;
            if (!string.IsNullOrWhiteSpace(tbCidade.Text)) cli.Cidade = tbCidade.Text;

            Dados.Salvar();
            AtualizarGrid();
            LimparCampos();
        }

        private void BtnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientes.SelectedItem is not Cliente cli)
            {
                MessageBox.Show("Selecione um cliente na tabela para excluir.");
                return;
            }

            if (MessageBox.Show($"Excluir o cliente \"{cli.Nome}\"?", "Confirmar",
                    MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            Dados.Clientes.Remove(cli);
            Dados.Salvar();
            AtualizarGrid();
        }

        private void BtnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            string termo = tbPesquisa.Text?.Trim() ?? "";

            dgClientes.ItemsSource = string.IsNullOrEmpty(termo)
                ? Dados.Clientes
                : Dados.Clientes
                    .Where(c => c.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase)
                             || c.CPF.Contains(termo, StringComparison.OrdinalIgnoreCase)
                             || c.Cidade.Contains(termo, StringComparison.OrdinalIgnoreCase))
                    .ToList();
        }

        private void LimparCampos()
        {
            tbNome.Clear();
            tbTelefone.Clear();
            tbCPF.Clear();
            tbCidade.Clear();
        }
    }
}
