using System.ComponentModel.DataAnnotations.Schema;

namespace MM.Models;

[Table("produtos")]
public class Produtos {
    [Column("prod_id")]
    public int Id {get; set;}

    [Column("prod_nome")]
    public string? Nome {get; set;}
    
    [Column("prod_qtd")]
    public int Quantidade {get; set;}
    
    [Column("prod_ultvenda")]
    public DateTime? UltimaVenda {get; set;}
    
    [Column("prod_obs")]
    public string? Observacao {get; set;}
}