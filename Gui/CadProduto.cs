using Gtk;

public class CadProduto : Window {
    public Label NomeLabel = new("Nome");
    public Label QuantidadeLabel = new("Quantidade");
    public Entry NomeEntry = new ();
    public Entry QuantidadeEnty = new();
    public Button OkBtn = new("Ok");
    public Button CancelBtn = new("Cancelar");


    public CadProduto (string Nome = "", string quantidade = "") : base ("Cadastrar cliente"){
        NomeEntry.Text = Nome;
        QuantidadeEnty.Text = quantidade;

        var Box = new Box(Orientation.Vertical, 5);

        Box.PackStart(NomeLabel, false, false, 0);
        Box.PackStart(NomeEntry, false, false, 0);
        Box.PackStart(QuantidadeLabel, false, false, 0);
        Box.PackStart(QuantidadeEnty, false, false, 0);

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