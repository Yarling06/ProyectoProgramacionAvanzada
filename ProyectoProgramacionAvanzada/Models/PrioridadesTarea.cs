using System;
using System.Collections.Generic;

namespace ProyectoProgramacionAvanzada.Models;

public class PrioridadesTarea
{
    public int PrioridadID { get; set; }
    public string NivelPrioridad { get; set; } = null!;

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}

