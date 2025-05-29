namespace API_GlobalSolution.Dtos;

public record EnderecoPostDto(
    string? LogradouroEndereco,
    string? NumeroEndereco,
    string? ComplementoEndereco,
    string? CepEndereco,
    int BairroId
    );