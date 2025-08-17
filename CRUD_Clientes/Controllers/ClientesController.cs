using CRUD_Clientes.Data;
using CRUD_Clientes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Clientes.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteContext _context;

        public ClientesController(ClienteContext context)
        {
            _context = context;
        }

        // Utilidad para detectar llamadas AJAX (fetch)
        private bool IsAjaxRequest() =>
            Request.Headers["X-Requested-With"] == "XMLHttpRequest";

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var data = await _context.Clientes.AsNoTracking().ToListAsync();
            return View(data);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null) return NotFound();

            // Si viene por fetch (AJAX), devolvemos solo el modal (Layout = null en la vista)
            if (IsAjaxRequest())
                return PartialView("Details", cliente);

            // Navegación directa opcional
            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create() => View();

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Email,Telefono")] Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Telefono")] Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();
            if (!ModelState.IsValid) return View(cliente);

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cliente == null) return NotFound();

            // Si viene por fetch (AJAX), devolvemos solo el modal
            if (IsAjaxRequest())
                return PartialView("Delete", cliente);

            // Navegación directa opcional
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                if (IsAjaxRequest())
                    return Json(new { success = false, message = "No encontrado" });
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            // Si el borrado se hizo por AJAX, devolver JSON para que puedas quitar la fila sin recargar
            if (IsAjaxRequest())
                return Json(new { success = true, removedId = id });

            // Flujo tradicional
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id) =>
            _context.Clientes.Any(e => e.Id == id);
    }
}
