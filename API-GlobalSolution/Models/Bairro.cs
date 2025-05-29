using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GlobalSolution.Models;

[Table("BAIRRO")]
public class Bairro
{
    [Column("ID_BAIRRO")]
    public int BairroId { get; set; }
    
    [Column("NOME_BAIRRO")]
    [StringLength(100)]
    public string NomeBairro { get; set; }
    
    [Column("ZONA_BAIRRO")]
    [StringLength(50)]
    public string ZonaBairro { get; set; }
}