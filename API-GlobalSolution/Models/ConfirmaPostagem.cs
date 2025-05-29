using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("CONFIRMA_POSTAGEM")]
public class ConfirmaPostagem
{
    [Column("ID_USUARIO")]
    public int UsuarioId { get; set; }
    
    [Column("ID_POSTAGEM")]
    public int PostagemId { get; set; }
    
    [Column("DATA_CONFIRMA")]
    public DateTime DataConfirma { get; set; }
    
    [JsonIgnore]
    public Usuario Usuario { get; set; }
    
    [JsonIgnore]
    public Postagem Postagem { get; set; }
}