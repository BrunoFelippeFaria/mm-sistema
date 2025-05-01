using Cairo;
using Gtk;
using MM.Data;
using MM.Models;

namespace GW.Gui.Telas;

public class TelaProduto : Box {
    AppDataContext context;

    TreeView tabela = new TreeView();
    ListStore store = new ListStore(typeof(Produtos) ,typeof(string), typeof(int), typeof(string));
    Button CadProdutoBtn = new Button("Cadastrar Produto");
    
    public TelaProduto () : base (Orientation.Vertical, 5) {
        context = new AppDataContext();
        Add(CadProdutoBtn);
        //coluna 1
        var col1 = new TreeViewColumn { Title = "Nome" };
        var cell1 = new CellRendererText();
        col1.PackStart(cell1, true);
        col1.AddAttribute(cell1, "text", 1);
        tabela.AppendColumn(col1);

        //coluna 2
        var col2 = new TreeViewColumn { Title = "Quantidade" };
        var cell2 = new CellRendererText();
        col2.PackStart(cell2, true);
        col2.AddAttribute(cell2, "text", 2);
        tabela.AppendColumn(col2);

        //coluna 3
        var col3 = new TreeViewColumn { Title = "Última Venda" };
        var cell3 = new CellRendererText();
        col3.PackStart(cell3, true);
        col3.AddAttribute(cell3, "text", 3);
        tabela.AppendColumn(col3);

        CarregarDados();

        Add(tabela);

        //eventos
        CadProdutoBtn.Clicked += (s, o) => {
            var cad = new CadProduto();
            cad.OkBtn.Clicked  += (s, o) => {
                var produto = new Produtos{
                    Nome = cad.NomeEntry.Text,
                    Quantidade = Convert.ToInt16(cad.QuantidadeEnty.Text)
                };
                
                context.Produtos.Add(produto);
                context.SaveChanges();
                CarregarDados();
                cad.Destroy();
            };
        };

        MenuHandler menuHandler = new MenuHandler(tabela);
        
        menuHandler.OnMenuBuild += (se, ob) => {
            //atualizar
            var editar = new MenuItem("editar");
                editar.Activated += (s, e) => {
                    var produto = (Produtos)menuHandler.Item;
                    var cad = new CadProduto(produto.Nome ?? "", Convert.ToString(produto.Quantidade) ?? "");
                    
                    cad.OkBtn.Clicked += (o, s) => {
                        produto.Nome = cad.NomeEntry.Text;
                        produto.Quantidade = Convert.ToInt16(cad.QuantidadeEnty.Text); 
                        context.Produtos.Update(produto);
                        context.SaveChanges();
                        CarregarDados();
                        cad.Destroy();
                    };
                };
            
            //deletar
            var deletar = new MenuItem("deletar");
            
            deletar.Activated += (s, o) => {
                var produto = (Produtos)menuHandler.Item;
                var msg = new MessageDialog(
                    null,
                    DialogFlags.Modal,
                    MessageType.Question,
                    ButtonsType.YesNo,
                    $"Tem certeza que deseja deletar {produto.Nome} ?"
                );
            
                msg.ShowAll();

                msg.Response += (o, s) => {
                    if (s.ResponseId == ResponseType.Yes){
                        context.Produtos.Remove(produto);
                        context.SaveChanges();
                        CarregarDados();
                        msg.Destroy();
                    }

                    else {
                        msg.Destroy();
                    }
                };
            };

            // vender
            var vender = new MenuItem("vender");
            
            vender.Activated += (s, o) => {
                var produto = (Produtos)menuHandler.Item;
                var comp = new Comprar();
                var clienteStore = new ListStore(typeof(Clientes), typeof(string));
                var clientes = context.Clientes.ToArray();

                foreach (var cliente in clientes) {
                    clienteStore.AppendValues(cliente, cliente.Nome);
                }
                
                var cell = new CellRendererText();
                comp.ClientesBox.PackStart(cell, false);
                comp.ClientesBox.AddAttribute(cell, "text", 1);

                comp.ClientesBox.Model = clienteStore;

                comp.OkBtn.Clicked += (s, o) => {
                    TreeIter iter;
                    if (comp.ClientesBox.GetActiveIter(out iter)) {
                        var clienteEscolhido = (Clientes)comp.ClientesBox.Model.GetValue(iter, 0);
                        var venda = new Vendas {
                            ClienteId = clienteEscolhido.Id,
                            produtoId = produto.Id,
                            Valor =  (float)Convert.ToDecimal(comp.ValorVendaEntry.Text),
                            VendaData = DateTime.Now
                        };
                        
                        produto.Quantidade--;
                        produto.UltimaVenda = DateTime.Now;

                        context.Vendas.Add(venda);
                        context.Update(produto);
                        context.SaveChanges();
                        CarregarDados();

                        comp.Destroy();
                    }
                };             

            };

            menuHandler.ContextMenu?.Append(vender);
            menuHandler.ContextMenu?.Append(editar);
            menuHandler.ContextMenu?.Append(deletar);
        };
    }

    void CarregarDados () {
        store.Clear();
        
        var produtos = context.Produtos.ToArray(); 
        foreach (var produto in produtos) {
            store.AppendValues(produto, produto.Nome!, produto.Quantidade!, produto?.UltimaVenda != null ? produto.UltimaVenda.Value.ToString("dd/MM/yyyy") : "");
        }

        tabela.Model = store;
    }

}