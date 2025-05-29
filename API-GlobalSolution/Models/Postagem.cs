using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("POSTAGEM")]
public class Postagem
{
    [Column("ID_POSTAGEM")]
    public int PostagemId { get; set; }
    
    [Column("TITULO_POSTAGEM")]
    [StringLength(100)]
    public string TituloPostagem { get; set; }
    
    [Column("DESCRICAO_POSTAGEM")]
    [StringLength(255)]
    public string DescricaoPostagem { get; set; }
    
    [Column("DATA_POSTAGEM")]
    public DateTime DataPostagem { get; set; }
    
    [Column("TIPO_POSTAGEM")]
    [StringLength(50)]
    public string TipoPostagem { get; set; }
    
    [Column("STATUS_POSTAGEM")]
    [StringLength(50)]
    public string StatusPostagem { get; set; }
    
    [Column("USUARIO_ID_USUARIO")]
    public int UsuarioId { get; set; }
    
    [Column("ENDERECO_ID_ENDERECO")]
    public int EnderecoId { get; set; }
    
    [Column("TIPO_DESASTRE_ID_DESASTRE")]
    public int TipoDesastreId { get; set; }

    [JsonIgnore] 
    public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    
    [JsonIgnore]
    public Usuario Usuario { get; set; }
    
    [JsonIgnore]
    public Endereco Endereco { get; set; }
    
    [JsonIgnore]
    public TipoDesastre TipoDesastre { get; set; }
}