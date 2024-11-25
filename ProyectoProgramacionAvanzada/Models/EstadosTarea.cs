using System;
using System.Collections.Generic;

namespace ProyectoProgramacionAvanzada.Models;

public class EstadosTarea
{
    public int EstadoID { get; set; }
    public string NombreEstado { get; set; } = null!;

    // Propiedad para los LogsEjecucion (asegúrate de que esto sea necesario)
    public virtual ICollection<LogsEjecucion> LogsEjecucion { get; set; } = new List<LogsEjecucion>();
    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}

