using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record BairroReadDto(
    int BairroId,
    string NomeBairro,
    string ZonaBairro)
{
    public static BairroReadDto ToDto(Bairro b) =>
        new (b.BairroId, b.NomeBairro, b.ZonaBairro);
};