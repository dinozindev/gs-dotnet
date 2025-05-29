using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GlobalSolution.Models;

[Table("TIPO_DESASTRE")]
public class TipoDesastre
{
    [Column("ID_DESASTRE")]
    public int TipoDesastreId { get; set; }
    
    [Column("NOME_DESASTRE")]
    [StringLength(100)]
    public string NomeDesastre { get; set; }
    
    [Column("DESCRICAO_DESASTRE")]
    [StringLength(255)]
    public string DescricaoDesastre { get; set; }
}