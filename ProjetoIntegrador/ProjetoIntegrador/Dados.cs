using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjetoIntegrador
{
    // ============================================================
    // MODELOS
    // ============================================================

    public class Produto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; } = "";
        public string Genero { get; set; } = "";
        public string Tamanho { get; set; } = "";
        public string Cor { get; set; } = "";
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public string? Imagem { get; set; }

        // Colunas de exibicao no grid de estoque (nao persistidas)
        [JsonIgnore] public int QuantidadeDisponivel => Estoque;
        [JsonIgnore]
        public string Status =>
            Estoque <= 0 ? "Sem estoque" :
            Estoque < 5 ? "Estoque baixo" :
            "Disponível";
    }

    public class ItemVenda
    {
        public int Codigo { get; set; }
        public string Produto { get; set; } = "";
        public string Cor { get; set; } = "";
        public string Tam { get; set; } = "";
        public int Qtd { get; set; }
        public decimal Unitario { get; set; }
        public decimal Desc { get; set; } // percentual
        [JsonIgnore] public decimal Total => Math.Round(Qtd * Unitario * (1 - Desc / 100m), 2);
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string CPF { get; set; } = "";
        public string Cidade { get; set; } = "";
    }

    public class Movimento
    {
        public string Data { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Tipo { get; set; } = ""; // Entrada / Saída
        public decimal Valor { get; set; }
        public string Pagamento { get; set; } = "";
    }

    // Objeto serializado no arquivo JSON
    public class BancoJson
    {
        public List<Produto> Produtos { get; set; } = new();
        public List<Cliente> Clientes { get; set; } = new();
        public List<Movimento> Movimentos { get; set; } = new();
        public decimal TotalVendido { get; set; }
        public int QtdVendida { get; set; }
        public Dictionary<string, int> VendasPorProduto { get; set; } = new();
        public string NomeLoja { get; set; } = "Minha Loja";
        public string Cnpj { get; set; } = "";
        public string Telefone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Tema { get; set; } = "Claro";
        public string Idioma { get; set; } = "Português";
        public bool AbrirCaixaAuto { get; set; }
        public bool EmitirComprovante { get; set; }
    }

    // ============================================================
    // ARMAZENAMENTO EM ARQUIVO JSON (sem banco de dados)
    // ============================================================

    public static class Dados
    {
        private static readonly string Arquivo =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dados.json");

        private static readonly JsonSerializerOptions Opcoes = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static ObservableCollection<Produto> Produtos { get; } = new();
        public static ObservableCollection<Cliente> Clientes { get; } = new();
        public static ObservableCollection<Movimento> Movimentos { get; } = new();

        // Metricas de venda acumuladas
        public static decimal TotalVendido { get; set; }
        public static int QtdVendida { get; set; }
        public static Dictionary<string, int> VendasPorProduto { get; } = new();

        // Configuracoes da loja
        public static string NomeLoja { get; set; } = "Minha Loja";
        public static string Cnpj { get; set; } = "";
        public static string Telefone { get; set; } = "";
        public static string Email { get; set; } = "";
        public static string Tema { get; set; } = "Claro";
        public static string Idioma { get; set; } = "Português";
        public static bool AbrirCaixaAuto { get; set; }
        public static bool EmitirComprovante { get; set; }

        static Dados()
        {
            if (File.Exists(Arquivo))
                Carregar();
            else
            {
                Semear();
                Salvar();
            }
        }

        private static void Semear()
        {
            Produtos.Add(new Produto { Codigo = 1, Nome = "Camiseta Básica", Genero = "M", Tamanho = "M", Cor = "Branco", Preco = 49.90m, Estoque = 20 });
            Produtos.Add(new Produto { Codigo = 2, Nome = "Calça Skinny", Genero = "F", Tamanho = "P", Cor = "Preto", Preco = 129.90m, Estoque = 8 });
            Produtos.Add(new Produto { Codigo = 3, Nome = "Moletom Vermelho", Genero = "M", Tamanho = "GG", Cor = "Vermelho", Preco = 159.90m, Estoque = 3 });
            Produtos.Add(new Produto { Codigo = 4, Nome = "Jaqueta de Couro", Genero = "M", Tamanho = "XG", Cor = "Marrom", Preco = 299.90m, Estoque = 0 });
        }

        // ---------- Persistencia ----------

        public static void Salvar()
        {
            var banco = new BancoJson
            {
                Produtos = Produtos.ToList(),
                Clientes = Clientes.ToList(),
                Movimentos = Movimentos.ToList(),
                TotalVendido = TotalVendido,
                QtdVendida = QtdVendida,
                VendasPorProduto = new Dictionary<string, int>(VendasPorProduto),
                NomeLoja = NomeLoja,
                Cnpj = Cnpj,
                Telefone = Telefone,
                Email = Email,
                Tema = Tema,
                Idioma = Idioma,
                AbrirCaixaAuto = AbrirCaixaAuto,
                EmitirComprovante = EmitirComprovante
            };

            try
            {
                File.WriteAllText(Arquivo, JsonSerializer.Serialize(banco, Opcoes));
            }
            catch
            {
                // ignora falha de gravacao (ex.: disco/permissao)
            }
        }

        private static void Carregar()
        {
            try
            {
                var banco = JsonSerializer.Deserialize<BancoJson>(File.ReadAllText(Arquivo));
                if (banco == null) { Semear(); return; }

                Produtos.Clear();
                foreach (var p in banco.Produtos) Produtos.Add(p);

                Clientes.Clear();
                foreach (var c in banco.Clientes) Clientes.Add(c);

                Movimentos.Clear();
                foreach (var m in banco.Movimentos) Movimentos.Add(m);

                TotalVendido = banco.TotalVendido;
                QtdVendida = banco.QtdVendida;
                VendasPorProduto.Clear();
                foreach (var kv in banco.VendasPorProduto) VendasPorProduto[kv.Key] = kv.Value;

                NomeLoja = banco.NomeLoja;
                Cnpj = banco.Cnpj;
                Telefone = banco.Telefone;
                Email = banco.Email;
                Tema = banco.Tema;
                Idioma = banco.Idioma;
                AbrirCaixaAuto = banco.AbrirCaixaAuto;
                EmitirComprovante = banco.EmitirComprovante;
            }
            catch
            {
                Semear();
            }
        }

        // ---------- Regras ----------

        public static Produto? BuscarProduto(int codigo)
            => Produtos.FirstOrDefault(p => p.Codigo == codigo);

        public static int ProximoIdCliente()
            => Clientes.Count == 0 ? 1 : Clientes.Max(c => c.Id) + 1;

        // Registra uma venda finalizada: baixa estoque, acumula metricas, lanca no financeiro e salva
        public static void RegistrarVenda(IEnumerable<ItemVenda> itens, decimal total, string pagamento)
        {
            foreach (var item in itens)
            {
                var prod = BuscarProduto(item.Codigo);
                if (prod != null)
                    prod.Estoque = Math.Max(0, prod.Estoque - item.Qtd);

                QtdVendida += item.Qtd;
                VendasPorProduto[item.Produto] =
                    (VendasPorProduto.TryGetValue(item.Produto, out var q) ? q : 0) + item.Qtd;
            }

            TotalVendido += total;

            Movimentos.Add(new Movimento
            {
                Data = DateTime.Now.ToString("dd/MM/yyyy"),
                Descricao = "Venda no PDV",
                Tipo = "Entrada",
                Valor = total,
                Pagamento = pagamento
            });

            Salvar();
        }

        public static string ProdutoMaisVendido()
            => VendasPorProduto.Count == 0
                ? "—"
                : VendasPorProduto.OrderByDescending(kv => kv.Value).First().Key;
    }
}
