namespace API_GlobalSolution.Dtos;

public record ComentarioPostDto
    (
        string TextoComentario,
        int PostagemId,
        int UsuarioId
        );