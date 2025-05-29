namespace API_GlobalSolution.Dtos;

public record UsuarioPostDto(
    string NomeUsuario,
    string EmailUsuario,
    string SenhaUsuario,
    string TelefoneUsuario
    );