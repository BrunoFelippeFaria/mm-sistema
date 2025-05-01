using Gtk;

public class Comprar : Window {
    public Label ValorVendaLabel = new("Valor da venda R$");
    public Entry ValorVendaEntry = new ();
  
    public Button OkBtn = new("Ok");
    public Button CancelBtn = new("Cancelar");

    public ComboBox ClientesBox = new();

    public Comprar () : base ("Cadastrar cliente"){
        var Box = new Box(Orientation.Vertical, 5);

        var VendaBox = new Box(Orientation.Horizontal, 5);
        

        VendaBox.PackStart(ValorVendaLabel, false, false, 0);
        VendaBox.PackStart(ValorVendaEntry, true, true, 0);
        
        
        VendaBox.PackStart(ClientesBox, false, false, 0);

        Box.PackStart(VendaBox, false, false, 0);

        var ButtonBox = new Box(Orientation.Horizontal, 5);
        ButtonBox.PackStart(OkBtn, true, true, 0);
        ButtonBox.PackStart(CancelBtn, true, true, 0);

        Box.PackStart(ButtonBox, false, false, 0);

        Add(Box);


        //eventos
        DeleteEvent += (s, o) => this.Destroy();
        CancelBtn.Clicked += (o, s) => this.Destroy();


        //configs da janela
        Resizable = false;
        SetDefaultSize(300,200);
        ShowAll();
    }
}