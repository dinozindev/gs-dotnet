using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record EnderecoReadDto(
    int EnderecoId,
    string? LogradouroEndereco,
    string? NumeroEndereco,
    string? ComplementoEndereco,
    string? CepEndereco,
    BairroReadDto Bairro
)
{
    public static EnderecoReadDto ToDto(Endereco e) =>
        new(
        e.EnderecoId,
        e.LogradouroEndereco,
        e.NumeroEndereco,
        e.ComplementoEndereco,
        e.CepEndereco,
        BairroReadDto.ToDto(e.Bairro)
        );
};