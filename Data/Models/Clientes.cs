using System.ComponentModel.DataAnnotations.Schema;

namespace MM.Models;

[Table("clientes")]
public class Clientes {
    [Column("cli_id")]
    public int Id {get; set;}
    
    [Column("cli_nome")]
    public string? Nome {get; set;}
    
    [Column("cli_tel")]
    public string? Telefone {get; set;} 
}