using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoProgramacionAvanzada.Models;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class TareaController : Controller
    {
        private readonly ProyectoPrograDbContext _context;

        public TareaController(ProyectoPrograDbContext context)
        {
            _context = context;
        }

        // GET: Tarea
        public async Task<IActionResult> Index()
        {
            var proyectoPrograDbContext = _context.Tareas
                .Include(t => t.Estado)
                .Include(t => t.Prioridad);
            return View(await proyectoPrograDbContext.ToListAsync());
        }

        // GET: Tarea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.TareaId == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tarea/Create
        public IActionResult Create()
        {
            // Asegúrate de que los datos para los select se pasen correctamente
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado");
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad");
            return View();
        }

        // POST: Tarea/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,PrioridadID,EstadoID,FechaEjecucion,FechaFinalizacion")] Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                tarea.FechaCreacion = DateTime.Now; // Configura la fecha de creación
                _context.Tareas.Add(tarea); // Agrega la tarea al contexto
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return RedirectToAction(nameof(Index));
            }

            // Pasar las opciones a la vista nuevamente si el modelo no es válido
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad", tarea.PrioridadID);
            return View(tarea);
        }

        // GET: Tarea/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Si no hay ID, redirige a NotFound
            }

            // Buscar la tarea por su ID
            var tarea = await _context.Tareas
                                      .Include(t => t.Estado)    // Incluimos la entidad Estado
                                      .Include(t => t.Prioridad) // Incluimos la entidad Prioridad
                                      .FirstOrDefaultAsync(m => m.TareaId == id); // Usamos FirstOrDefaultAsync para obtener la tarea

            if (tarea == null)
            {
                return NotFound(); // Si la tarea no existe, redirige a NotFound
            }

            // Creamos las listas para los dropdowns de Estado y Prioridad
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad", tarea.PrioridadID);

            // Retornamos la vista con los datos de la tarea
            return View(tarea);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TareaId,Nombre,Descripcion,PrioridadID,EstadoID,FechaCreacion,FechaEjecucion,FechaFinalizacion")] Tarea tarea)
        {
            // Verifica que el ID de la tarea coincida con el parámetro de ID.
            if (id != tarea.TareaId)
            {
                return NotFound();  // Si los IDs no coinciden, se retorna un error 404
            }

            // Verifica si el modelo es válido
            if (ModelState.IsValid)
            {
                try
                {
                    // Marca la tarea como modificada y guarda los cambios
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();

                    // Redirige al índice (lista de tareas)
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si ocurre un error de concurrencia, simplemente lanza la excepción
                    throw;
                }
            }

            // Si el modelo no es válido, recarga los datos de estado y prioridad para mostrar en la vista
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad", tarea.PrioridadID);

            // Devuelve la vista con los datos actuales de la tarea
            return View(tarea);
        }


        // GET: Tarea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.TareaId == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.TareaId == id);
        }
    }
}
