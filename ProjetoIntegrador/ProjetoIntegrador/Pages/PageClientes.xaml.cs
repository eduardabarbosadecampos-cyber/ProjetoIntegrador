using System;
using System.Collections.Generic;
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
    /// Interação lógica para PageClientes.xam
    /// </summary>
    public partial class PageClientes : UserControl
    {
        public PageClientes()
        {
            InitializeComponent();
        }

        private void AddCliente(object sender, RoutedEventArgs e)
        {
            var cliente = new Clientes();
            cliente.Nome = "Luciano";

            dgClientes.Items.Add(cliente);
        }
    }

    public class Clientes()
    {
        public string Nome { get; set; }
    }
}
