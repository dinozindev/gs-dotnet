using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_GlobalSolution.Models;

[Table("USUARIO")]
public class Usuario
{   
    [Column("ID_USUARIO")]
    public int UsuarioId {get; set;}
    
    [Column("NOME_USUARIO")]
    [StringLength(100)]
    public string NomeUsuario {get; set;}
    
    [Column("EMAIL_USUARIO")]
    [StringLength(200)]
    public string EmailUsuario {get; set;}
    
    [Column("SENHA_USUARIO")]
    [StringLength(50)]
    public string SenhaUsuario {get; set;}
    
    [Column("TELEFONE_USUARIO")]
    [StringLength(11)]
    public string TelefoneUsuario {get; set;}
    
    [Column("TIPO_USUARIO")]
    [StringLength(50)]
    public string TipoUsuario {get; set;}
}