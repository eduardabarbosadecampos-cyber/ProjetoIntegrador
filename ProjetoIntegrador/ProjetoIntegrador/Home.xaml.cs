using Mysqlx.Connection;
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

namespace ProjetoIntegrador
{
    /// <summary>
    /// Interação lógica para Home.xam
    /// </summary>
    public partial class Home : Page
    {
        internal List<Button> MenuButtons = new List<Button>();

        public Home()
        {
            InitializeComponent();
            MenuButtons = new List<Button> { BtnVendas, BtnProdutos, BtnClientes, BtnEstoque, BtnRelatorios, BtnFinanceiro, BtnConfiguracoes };
        }

        private void SetActiveButton()
        {

        }


        private void Exit_Sistema_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
