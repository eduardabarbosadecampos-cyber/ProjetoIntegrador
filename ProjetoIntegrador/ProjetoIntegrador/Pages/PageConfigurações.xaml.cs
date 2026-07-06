using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetoIntegrador.Pages
{
    /// <summary>
    /// Interação lógica para PageConfigurações.xam
    /// </summary>
    public partial class PageConfigurações : UserControl
    {
        public PageConfigurações()
        {
            InitializeComponent();
            CarregarConfiguracoes();
        }

        private void CarregarConfiguracoes()
        {
            tbNomeLoja.Text = Dados.NomeLoja;
            tbCnpj.Text = Dados.Cnpj;
            tbTelefone.Text = Dados.Telefone;
            tbEmail.Text = Dados.Email;
            SelecionarItem(cbTema, Dados.Tema);
            SelecionarItem(cbIdioma, Dados.Idioma);
            chkCaixaAuto.IsChecked = Dados.AbrirCaixaAuto;
            chkComprovante.IsChecked = Dados.EmitirComprovante;
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            Dados.NomeLoja = tbNomeLoja.Text;
            Dados.Cnpj = tbCnpj.Text;
            Dados.Telefone = tbTelefone.Text;
            Dados.Email = tbEmail.Text;
            Dados.Tema = (cbTema.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Claro";
            Dados.Idioma = (cbIdioma.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Português";
            Dados.AbrirCaixaAuto = chkCaixaAuto.IsChecked == true;
            Dados.EmitirComprovante = chkComprovante.IsChecked == true;

            Dados.Salvar();
            MessageBox.Show("Configurações salvas com sucesso!", "Sucesso");
        }

        private static void SelecionarItem(ComboBox cb, string valor)
        {
            foreach (var obj in cb.Items)
            {
                if (obj is ComboBoxItem item && item.Content?.ToString() == valor)
                {
                    cb.SelectedItem = item;
                    return;
                }
            }
        }
    }
}
