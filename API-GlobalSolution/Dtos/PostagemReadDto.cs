using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record PostagemReadDto(
    int PostagemId,
    string TituloPostagem,
    string DescricaoPostagem,
    DateTime DataPostagem,
    string TipoPostagem,
    string StatusPostagem,
    UsuarioReadDto Usuario,
    EnderecoReadDto Endereco,
    TipoDesastreReadDto TipoDesastre,
    List<ComentarioResumoDto> Comentarios
)
{
    public static PostagemReadDto ToDto(Postagem p) =>
        new(
            p.PostagemId,
            p.TituloPostagem,
            p.DescricaoPostagem,
            p.DataPostagem,
            p.TipoPostagem,
            p.StatusPostagem,
            UsuarioReadDto.ToDto(p.Usuario),
            EnderecoReadDto.ToDto(p.Endereco),
            TipoDesastreReadDto.ToDto(p.TipoDesastre),
            p.Comentarios != null 
                ? p.Comentarios.Select(ComentarioResumoDto.ToDto).ToList() 
                : new List<ComentarioResumoDto>()
        );
};