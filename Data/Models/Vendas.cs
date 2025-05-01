
using System.ComponentModel.DataAnnotations.Schema;

[Table("vendas")]
public class Vendas {
    [Column("vend_id")]
    public int Id {get; set;}

    [Column("cli_id")]
    public int ClienteId {get; set;}
    
    [Column("prod_id")]
    public int produtoId {get; set;}
    
    [Column("vend_data")]
    public DateTime VendaData {get; set;}

    [Column("vend_valor")]
    public float Valor {get; set;}
}