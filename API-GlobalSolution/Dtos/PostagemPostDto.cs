namespace API_GlobalSolution.Dtos;

public record PostagemPostDto(
    string TituloPostagem,
    string DescricaoPostagem,
    int UsuarioId,
    EnderecoPostDto Endereco,
    int TipoDesastreId
    );