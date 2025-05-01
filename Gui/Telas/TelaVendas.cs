using Cairo;
using Gtk;
using MM.Data;

namespace MM.Gui.Telas;

public class TelaVendas : Box {
    AppDataContext context = new AppDataContext();
    public Label QtdvendasLabel {get; set;} = new();
    public Label LucroLabel {get; set;} = new();
    public Label PrincipalClienteLabel {get; set;} = new();  
    public event EventHandler OnQtdVendasChanged = delegate {};

    TreeView tabela = new TreeView();
    ListStore store = new ListStore(typeof(string), typeof(string), typeof(int), typeof(string));

    private int _qtdVendas;
    public int QtdVendas {
    get => _qtdVendas;
    set {
            if (_qtdVendas != value) {
                _qtdVendas = value;
                OnQtdVendasChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }

    float Lucro {get; set;}
    string? PrincipalCliente {get; set;}

    public TelaVendas () : base (Orientation.Vertical, 5) {
        atualizarDados();
        PackStart(QtdvendasLabel, false, false, 0);
        PackStart(LucroLabel, false, false, 0);
        PackStart(PrincipalClienteLabel, false, false, 0);

        QtdvendasLabel.StateChanged += (s, o) => {
            atualizarDados();
        };
        
        //produtos
        var col1 = new TreeViewColumn { Title = "Produto" };
        var cell1 = new CellRendererText();
        col1.PackStart(cell1, true);
        col1.AddAttribute(cell1, "text", 0);
        tabela.AppendColumn(col1);

        //cliente
        var col2 = new TreeViewColumn { Title = "Cliente" };
        var cell2 = new CellRendererText();
        col2.PackStart(cell2, true);
        col2.AddAttribute(cell2, "text", 1);
        tabela.AppendColumn(col2);

        //valor
        var col3 = new TreeViewColumn { Title = "Valor" };
        var cell3 = new CellRendererText();
        col3.PackStart(cell3, true);
        col3.AddAttribute(cell3, "text", 2);
        tabela.AppendColumn(col3);
        
        //data
        var col4 = new TreeViewColumn { Title = "Data" };
        var cell4 = new CellRendererText();
        col4.PackStart(cell4, true);
        col4.AddAttribute(cell4, "text", 3);
        tabela.AppendColumn(col4);

        tabela.Model = store;
        PackStart(tabela, false, false, 0);


    }

    public void atualizarDados (){
        var vendas = context.Vendas;
        var clientes = context.Clientes;

        QtdVendas = vendas.Count();
        Lucro = vendas.Sum(v => v.Valor);
        
        var principalCliente = (
            from venda in vendas
            join cliente in clientes on venda.ClienteId equals cliente.Id
            group venda by cliente into grupo
            orderby grupo.Sum(v => v.Valor) descending
            select grupo.Key
        ).FirstOrDefault();

        PrincipalCliente = principalCliente?.Nome;

        QtdvendasLabel.Text = "Quantidade de vendas: " +  QtdVendas;
        LucroLabel.Text = "Lucro: R$ " + Lucro;
        PrincipalClienteLabel.Text = "Principal cliente: " + (PrincipalCliente ?? "Nenhum");
    
        store.Clear();
        
        foreach (var venda in vendas) {
            var produtoNome = context.Produtos.FirstOrDefault(p => p.Id == venda.produtoId)?.Nome ?? "Desconhecido";
            var clienteNome = context.Clientes.FirstOrDefault(c => c.Id == venda.ClienteId)?.Nome ?? "Desconhecido";
            store.AppendValues(produtoNome, clienteNome, venda.Valor, venda.VendaData.ToString("dd/MM/yyyy"));
        }

    }
}