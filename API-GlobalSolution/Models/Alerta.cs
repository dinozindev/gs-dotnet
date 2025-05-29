using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace API_GlobalSolution.Models;

[Table("ALERTA")]
public class Alerta
{
    [Column("ID_ALERTA")]
    public int AlertaId { get; set; }
    
    [Column("NIVEL_RISCO")]
    [StringLength(50)]
    public string NivelRisco { get; set; }
    
    [Column("DATA_ALERTA")]
    public DateTime DataAlerta { get; set; }
    
    [Column("DESCRICAO_ALERTA")]
    [StringLength(255)]
    public string? DescricaoAlerta { get; set; }
    
    [Column("STATUS_ALERTA")]
    [StringLength(50)]
    public string StatusAlerta { get; set; }
    
    [Column("SENSOR_ID_SENSOR")]
    public int SensorId { get; set; }
    
    [Column("TIPO_DESASTRE_ID_DESASTRE")]
    public int TipoDesastreId { get; set; }
    
    [JsonIgnore]
    public Sensor Sensor { get; set; }
    
    [JsonIgnore]
    public TipoDesastre TipoDesastre { get; set; }
}