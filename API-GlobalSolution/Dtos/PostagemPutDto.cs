namespace API_GlobalSolution.Dtos;

public record PostagemPutDto(
    string TituloPostagem,
    string DescricaoPostagem,
    string StatusPostagem,
    int TipoDesastreId,
    EnderecoPostDto Endereco
    );