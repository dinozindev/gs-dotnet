using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("ENDERECO")]
public class Endereco
{
   [Column("ID_ENDERECO")]
   public int EnderecoId { get; set; } 
   
   [Column("LOGRADOURO_ENDERECO")]
   [StringLength(255)]
   public string? LogradouroEndereco {get; set;}
   
   [Column("NUMERO_ENDERECO")]
   [StringLength(10)]
   public string? NumeroEndereco { get; set; }
   
   [Column("COMPLEMENTO_ENDERECO")]
   [StringLength(255)]
   public string? ComplementoEndereco { get; set; }
   
   [Column("CEP_ENDERECO")]
   [StringLength(9)]
   public string? CepEndereco { get; set; }
   
   [Column("BAIRRO_ID_BAIRRO")]
   public int BairroId { get; set; }
   
   [JsonIgnore]
   public Bairro Bairro { get; set; }
}