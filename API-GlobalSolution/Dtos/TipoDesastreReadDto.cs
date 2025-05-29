using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record TipoDesastreReadDto(
    int TipoDesastreId,
    string NomeDesastre,
    string DescricaoDesastre
)
{
    public static TipoDesastreReadDto ToDto(TipoDesastre td) =>
        new(
            td.TipoDesastreId,
            td.NomeDesastre,
            td.DescricaoDesastre
        );
};