using API_GlobalSolution.Models;

namespace API_GlobalSolution.Dtos;

public record SensorReadDto(
    int SensorId,
    string NomeSensor,
    string TipoSensor,
    BairroReadDto? Bairro
)
{
    public static SensorReadDto ToDto(Sensor s) =>
        new(
            s.SensorId,
            s.NomeSensor,
            s.TipoSensor,
            BairroReadDto.ToDto(s.Bairro)
        );
};