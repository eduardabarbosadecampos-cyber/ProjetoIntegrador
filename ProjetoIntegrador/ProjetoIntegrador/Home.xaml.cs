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
            RefreshMenus(BtnVendas);
        }

        private void Exit_Sistema_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                RefreshMenus(btn);
            }
        }

        private void RefreshMenus(Button curMenu)
        {
            foreach (var btn in MenuButtons)
            {
                if (btn == curMenu)
                {
                    btn.Background = (Brush)new BrushConverter().ConvertFrom("#22C55E");
                }
                else
                {
                    btn.Background = (Brush)new BrushConverter().ConvertFrom("#6B7280");
                }
            }
        }
    }
}
