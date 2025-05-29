using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record ConfirmaPostagemReadDto(
    int UsuarioId,
    int PostagemId,
    DateTime DataConfirma
)
{
    public static ConfirmaPostagemReadDto ToDto(ConfirmaPostagem cp) =>
        new(
            cp.UsuarioId,
            cp.PostagemId,
            cp.DataConfirma
        );
};