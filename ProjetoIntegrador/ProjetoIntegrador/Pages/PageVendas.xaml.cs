using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageVendas.xam
    /// </summary>
    public partial class PageVendas : UserControl
    {
        private readonly ObservableCollection<ItemVenda> _itens = new();

        public PageVendas()
        {
            InitializeComponent();
            dgItens.ItemsSource = _itens;
            _itens.CollectionChanged += (_, __) => AtualizarTotais();
        }

        private void BtnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbCodigo.Text, out int codigo))
            {
                MessageBox.Show("Informe um código de produto válido.");
                return;
            }

            var prod = Dados.BuscarProduto(codigo);
            if (prod == null)
            {
                MessageBox.Show($"Produto de código {codigo} não encontrado.");
                return;
            }

            if (!int.TryParse(tbQtd.Text, out int qtd) || qtd <= 0)
                qtd = 1;

            decimal.TryParse(tbDesc.Text, out decimal desc);
            if (desc < 0) desc = 0;
            if (desc > 100) desc = 100;

            if (prod.Estoque < qtd)
            {
                MessageBox.Show($"Estoque insuficiente. Disponível: {prod.Estoque}.");
                return;
            }

            _itens.Add(new ItemVenda
            {
                Codigo = prod.Codigo,
                Produto = prod.Nome,
                Cor = prod.Cor,
                Tam = prod.Tamanho,
                Qtd = qtd,
                Unitario = prod.Preco,
                Desc = desc
            });

            tbCodigo.Clear();
            tbQtd.Text = "1";
            tbDesc.Text = "0";
            tbCodigo.Focus();
        }

        private void AtualizarTotais()
        {
            decimal subtotal = _itens.Sum(i => i.Qtd * i.Unitario);
            decimal total = _itens.Sum(i => i.Total);
            decimal desconto = subtotal - total;

            txtSubtotal.Text = subtotal.ToString("C");
            txtDescontoTotal.Text = desconto.ToString("C");
            txtTotal.Text = total.ToString("C");

            CalcularTroco();
        }

        private decimal TotalAtual() => _itens.Sum(i => i.Total);

        private void tbRecebido_TextChanged(object sender, TextChangedEventArgs e) => CalcularTroco();

        private void CalcularTroco()
        {
            if (txtTroco == null) return;
            decimal.TryParse(tbRecebido.Text, out decimal recebido);
            decimal troco = recebido - TotalAtual();
            txtTroco.Text = (troco > 0 ? troco : 0m).ToString("C");
        }

        private void BtnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            if (_itens.Count == 0)
            {
                MessageBox.Show("Adicione ao menos um item para finalizar a venda.");
                return;
            }

            decimal total = TotalAtual();
            string pagamento = (cbPagamento.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Dinheiro";

            if (pagamento == "Dinheiro")
            {
                decimal.TryParse(tbRecebido.Text, out decimal recebido);
                if (recebido < total)
                {
                    MessageBox.Show("Valor recebido é menor que o total da venda.");
                    return;
                }
            }

            Dados.RegistrarVenda(_itens, total, pagamento);

            string msg = $"Venda finalizada!\nCliente: {tbCliente.Text}\nTotal: {total:C}\nPagamento: {pagamento}";
            if (Dados.EmitirComprovante)
                msg += "\n\n--- Comprovante emitido ---";
            MessageBox.Show(msg, "Sucesso");

            LimparVenda();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (_itens.Count == 0) return;
            if (MessageBox.Show("Cancelar a venda atual?", "Confirmar",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                LimparVenda();
        }

        private void LimparVenda()
        {
            _itens.Clear();
            tbRecebido.Clear();
            tbCliente.Text = "Consumidor Final";
            AtualizarTotais();
        }
    }
}
