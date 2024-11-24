using System;
using System.Collections.Generic;

namespace ProyectoProgramacionAvanzada.Models;

public class Tarea
{
    public int TareaId { get; set; }
    public string Nombre { get; set; }

    public string Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaEjecucion { get; set; }

    public DateTime? FechaFinalizacion { get; set; }

    public int EstadoID { get; set; } // Llave foránea


    public int PrioridadID { get; set; } // Llave foránea
   

public virtual EstadosTarea Estado { get; set; } = null!;

    public virtual ICollection<LogsEjecucion> LogsEjecucions { get; set; } = new List<LogsEjecucion>();

    public virtual PrioridadesTarea Prioridad { get; set; } = null!;

}
