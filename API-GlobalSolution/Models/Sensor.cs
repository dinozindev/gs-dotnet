using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("SENSOR")]
public class Sensor
{
    [Column("ID_SENSOR")]
    public int SensorId { get; set; }
    
    [Column("NOME_SENSOR")]
    [StringLength(100)]
    public string NomeSensor { get; set; }
    
    [Column("TIPO_SENSOR")]
    [StringLength(50)]
    public string TipoSensor { get; set; }
    
    [Column("BAIRRO_ID_BAIRRO")]
    public int? BairroId { get; set; }
    
    [JsonIgnore]
    public Bairro? Bairro { get; set; }
    
}