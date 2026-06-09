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
        internal List<Button> MenuButtons = new();
        internal List<Canvas> Menus = new();

        public Home()
        {
            InitializeComponent();

            MenuButtons = new()
        {
            BtnVendas,
            BtnProdutos,
            BtnClientes,
            BtnEstoque,
            BtnRelatorios,
            BtnFinanceiro,
            BtnConfiguracoes
        };

            Menus = new()
        {
            MenuVendas,
            MenuProdutos,
            MenuClientes,
            MenuEstoque,
            MenuRelatorios
        };

            RefreshMenus(BtnVendas);
        }

        private void ShowMenu(object sender, RoutedEventArgs e)
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
                btn.Background = btn == curMenu
                    ? (Brush)new BrushConverter().ConvertFrom("#22C55E")
                    : (Brush)new BrushConverter().ConvertFrom("#6B7280");
            }

            OpeMenu(curMenu);
        }

        private void OpeMenu(Button btn)
        {
            foreach (var menu in Menus)
            {
                menu.Visibility = Visibility.Collapsed;
            }

            switch (btn.Name)
            {
                case "BtnVendas":
                    MenuVendas.Visibility = Visibility.Visible;
                    break;

                case "BtnProdutos":
                    MenuProdutos.Visibility = Visibility.Visible;
                    break;

                case "BtnClientes":
                    MenuClientes.Visibility = Visibility.Visible;
                    break;

                case "BtnEstoque":
                    MenuEstoque.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void FecharSistema(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}