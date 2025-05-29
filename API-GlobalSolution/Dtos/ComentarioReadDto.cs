using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record ComentarioReadDto(
    int ComentarioId,
    string TextoComentario,
    DateTime DataComentario,
    PostagemResumoDto Postagem,
    UsuarioReadDto Usuario
)
{
    public static ComentarioReadDto ToDto(Comentario c) =>
        c == null ? null :
        new(
            c.ComentarioId,
            c.TextoComentario,
            c.DataComentario,
            PostagemResumoDto.ToDto(c.Postagem),
            UsuarioReadDto.ToDto(c.Usuario)
        );
};
