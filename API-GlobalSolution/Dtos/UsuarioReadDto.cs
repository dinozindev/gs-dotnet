using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record UsuarioReadDto(
    int UsuarioId,
    string NomeUsuario,
    string EmailUsuario,
    string SenhaUsuario,
    string TelefoneUsuario,
    string TipoUsuario
    )
{
    public static UsuarioReadDto ToDto(Usuario u) => 
            new(
                u.UsuarioId,
                u.NomeUsuario,
                u.EmailUsuario,
                u.SenhaUsuario,
                u.TelefoneUsuario,
                u.TipoUsuario
            );
    }