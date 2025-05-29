using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record PostagemResumoDto
(
int PostagemId,
string TituloPostagem,
string DescricaoPostagem,
DateTime DataPostagem,
string TipoPostagem,
string StatusPostagem,
UsuarioReadDto Usuario,
EnderecoReadDto Endereco,
TipoDesastreReadDto TipoDesastre
)
{
public static PostagemResumoDto ToDto(Postagem p) =>
    new(
        p.PostagemId,
        p.TituloPostagem,
        p.DescricaoPostagem,
        p.DataPostagem,
        p.TipoPostagem,
        p.StatusPostagem,
        UsuarioReadDto.ToDto(p.Usuario),
        EnderecoReadDto.ToDto(p.Endereco),
        TipoDesastreReadDto.ToDto(p.TipoDesastre)
    );
};