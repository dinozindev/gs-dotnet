using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record ComentarioResumoDto(
    int ComentarioId,
    string TextoComentario,
    DateTime DataComentario,
    UsuarioReadDto Usuario
)
{
    public static ComentarioResumoDto ToDto(Comentario c) =>
        new(
            c.ComentarioId,
            c.TextoComentario,
            c.DataComentario,
            c.Usuario != null ? UsuarioReadDto.ToDto(c.Usuario) : null
        );
};