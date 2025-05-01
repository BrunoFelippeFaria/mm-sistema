using Cairo;
using Gtk;
using MM.Data;
using MM.Models;

namespace GW.Gui.Telas;

public class TelaCliente : Box {
    AppDataContext context;

    TreeView tabela = new TreeView();
    ListStore store = new ListStore(typeof(Clientes) ,typeof(string), typeof(string));
    Button CadClienteBtn = new Button("Cadastrar Cliente");
    
    public TelaCliente () : base (Orientation.Vertical, 5) {
        context = new AppDataContext();
        Add(CadClienteBtn);
        //coluna 1
        var col1 = new TreeViewColumn { Title = "Nome" };
        var cell1 = new CellRendererText();
        col1.PackStart(cell1, true);
        col1.AddAttribute(cell1, "text", 1);
        tabela.AppendColumn(col1);

        //coluna 2
        var col2 = new TreeViewColumn { Title = "Telefone" };
        var cell2 = new CellRendererText();
        col2.PackStart(cell2, true);
        col2.AddAttribute(cell2, "text", 2);
        tabela.AppendColumn(col2);

        CarregarDados();

        Add(tabela);

        //eventos
        MenuHandler menuHandler = new MenuHandler(tabela);
        
        menuHandler.OnMenuBuild += (se, ob) => { 
            var editar = new MenuItem("editar");
                editar.Activated += (s, e) => {
                    var cliente = (Clientes)menuHandler.Item;
                    var cad = new CadCliente(cliente.Nome ?? "", cliente.Telefone ?? "");
                    
                    cad.OkBtn.Clicked += (o, s) => {
                        cliente.Nome = cad.NomeEntry.Text;
                        cliente.Telefone = cad.TelefoneEntry.Text; 
                        context.Clientes.Update(cliente);
                        context.SaveChanges();
                        CarregarDados();
                        cad.Destroy();
                    };
                };
            
            var deletar = new MenuItem("deletar");
                var cliente = (Clientes)menuHandler.Item;
                deletar.Activated += (s, e) => {
                    var msg = new MessageDialog(
                        null,
                        DialogFlags.Modal,
                        MessageType.Question,
                        ButtonsType.YesNo,
                        $"Tem certeza que deseja deletar {cliente.Nome} ?"  
                    );
                    msg.ShowAll();

                    msg.Response += (o, s) => {
                        if (s.ResponseId == ResponseType.Yes){
                            context.Clientes.Remove(cliente);
                            context.SaveChanges();
                            CarregarDados();
                            msg.Destroy();
                        }

                        else {
                            msg.Destroy();
                        }
                    };
                };

                menuHandler.ContextMenu?.Append(editar);
                menuHandler.ContextMenu?.Append(deletar);
        };

        CadClienteBtn.Clicked += (o, s) => {
            var cad = new CadCliente();
            cad.CancelBtn.Clicked += (o, s) => cad.Destroy();
            cad.OkBtn.Clicked += (o, s) => {
                var cliente = new Clientes{
                    Nome = cad.NomeEntry.Text,
                    Telefone = cad.TelefoneEntry.Text
                };

                context.Clientes.Add(cliente);
                context.SaveChanges();
                CarregarDados();
                cad.Destroy();
            };
        };

    }

    void CarregarDados () {
        store.Clear();
        
        var clientes = context.Clientes.ToArray(); 
        foreach (var cliente in clientes) {
            store.AppendValues(cliente, cliente.Nome, cliente.Telefone ?? "sem telefone");
        }

        tabela.Model = store;
    }

}