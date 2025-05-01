using Gtk;

public class MenuHandler {
    public TreeView Tabela {get; set;}
    public object Item {get; set;} = null!;
    public Menu? ContextMenu {get; set;}
    public event Action<Menu, object>? OnMenuBuild;


    public MenuHandler (TreeView tabela) {
        Tabela = tabela;
        Tabela.ButtonReleaseEvent += (o, args) => OnButtonReleaseEvent(args);
    }   

    private void OnButtonReleaseEvent(ButtonReleaseEventArgs args) {
         if (args.Event.Button == 3) { //btn direito
            TreePath path;
            if (Tabela.GetPathAtPos((int)args.Event.X, (int)args.Event.Y, out path))
            {
                TreeIter iter;
                if (!Tabela.Model.GetIter(out iter, path)) return;

                // Obtem o objeto cliente (ou qualquer outro objeto associado ao item)
                Item = Tabela.Model.GetValue(iter, 0);

                Tabela.GrabFocus();
                Tabela.SetCursor(path, null, false);

                ContextMenu = new Menu();
                OnMenuBuild?.Invoke(ContextMenu, Item); // dispara evento


                ContextMenu.ShowAll();
                ContextMenu.PopupAtPointer(args.Event);
            }
        }
    }

}