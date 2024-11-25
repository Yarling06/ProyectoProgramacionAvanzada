using System;
using System.Collections.Generic;

namespace ProyectoProgramacionAvanzada.Models;

public  class LogsEjecucion
{
    public int LogId { get; set; }

    public int TareaId { get; set; }

    public int EstadoID { get; set; }

    public string? Mensaje { get; set; }

    public DateTime? FechaLog { get; set; }

    public virtual EstadosTarea Estado { get; set; } = null!;

    public virtual Tarea Tarea { get; set; } = null!;

    public string Descripcion { get; set; } = null!;
    public DateTime FechaCreacion { get; set; }
}
