using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("COMENTARIO")]
public class Comentario
{
    [Column("ID_COMENTARIO")]
    public int ComentarioId { get; set; }
    
    [Column("TEXTO_COMENTARIO")]
    [StringLength(255)]
    public string TextoComentario { get; set; }
    
    [Column("DATA_COMENTARIO")]
    public DateTime DataComentario { get; set; }
    
    [Column("POSTAGEM_ID_POSTAGEM")]
    public int PostagemId { get; set; }
    
    [Column("USUARIO_ID_USUARIO")]
    public int UsuarioId { get; set; }
    
    [JsonIgnore]
    public Postagem Postagem { get; set; }
    
    [JsonIgnore]
    public Usuario Usuario { get; set; }
}