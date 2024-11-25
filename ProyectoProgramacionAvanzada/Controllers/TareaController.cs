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
        [HttpGet("Tarea/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var tarea = await _context.Tareas
                                       .Include(t => t.Estado)    // Incluir Estado
                                       .Include(t => t.Prioridad) // Incluir Prioridad
                                       .FirstOrDefaultAsync(t => t.TareaId == id);

            if (tarea == null)
            {
                return NotFound();
            }

            // Pasar las opciones a la vista nuevamente si el modelo no es válido
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad", tarea.PrioridadID);
            return View(tarea);
        }


        // POST: Tarea/Edit/5
        [HttpPost("Tarea/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tarea tarea)
        {
            // Verificar si el ID de la tarea coincide con el de la tarea que estamos editando
            if (id != tarea.TareaId)
            {
                return NotFound();
            }

            // Validación del modelo
            if (!ModelState.IsValid)
            {
                try
                {
                    // Actualizar la tarea en la base de datos
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si ocurre un error de concurrencia, verificar si la tarea existe
                    if (!_context.Tareas.Any(e => e.TareaId == tarea.TareaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redirigir al Index de tareas después de guardar los cambios
                return RedirectToAction(nameof(Index));
            }

            // Pasar las opciones a la vista nuevamente si el modelo no es válido
            ViewBag.EstadoID = new SelectList(_context.EstadosTareas, "EstadoID", "NombreEstado", tarea.EstadoID);
            ViewBag.PrioridadID = new SelectList(_context.PrioridadesTareas, "PrioridadID", "NivelPrioridad", tarea.PrioridadID);
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
