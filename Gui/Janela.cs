using GLib;
using Gtk;
using GW.Gui.Telas;
using MM.Gui.Telas;

namespace GW.Gui;

public class Janela : Window  {
    
    Notebook notebook = new Notebook();
    public Janela () : base ("M&M Sistema") {
        notebook.AppendPage(new TelaCliente(), new Label("Clientes"));
        notebook.AppendPage(new TelaProduto(), new Label("Produtos"));
        notebook.AppendPage(new TelaVendas(), new Label("Vendas"));
        

        Add(notebook);

        //eventos
        DeleteEvent += (s, o) => Gtk.Application.Quit();


        //configs da janela
        Resizable = false;
        SetDefaultSize(600,400);
        ShowAll();
    }
}