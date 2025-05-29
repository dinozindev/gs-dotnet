using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record AlertaReadDto(
    int AlertaId,
    string NivelRisco,
    DateTime DataAlerta,
    string? DescricaoAlerta,
    string StatusAlerta,
    SensorReadDto Sensor,
    TipoDesastreReadDto TipoDesastre
)
{
    public static AlertaReadDto ToDto(Alerta a) =>
        new(
            a.AlertaId,
            a.NivelRisco,
            a.DataAlerta,
            a.DescricaoAlerta,
            a.StatusAlerta,
            SensorReadDto.ToDto(a.Sensor),
            TipoDesastreReadDto.ToDto(a.TipoDesastre)
        );
};