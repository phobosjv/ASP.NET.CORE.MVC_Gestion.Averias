
namespace GestionIncidencias.Controllers
{
    public class OficinasController: Controller
    {
        private readonly ApplicationDbContext context;

        public OficinasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Acción para el Index de Oficinas
        public async Task<IActionResult> Index(string mensaje = null)
        {
            var oficinas = await context.Oficinas.Select(o => new OficinaViewModel
            {
                Id = o.Id,
                Codigo = o.Codigo,
                Nombre = o.Nombre,
                Telefono = o.Telefono,
                Activado = o.Activo
            }).OrderByDescending(o => o.Activado).ThenBy(o => o.Codigo).ToListAsync();

            var modelo = new OficinasListadoViewModel();
            modelo.Oficinas = oficinas;
            modelo.Mensaje = mensaje;

            return View(modelo);
        }

        // Crear nueva Oficina
        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear(Oficina oficina)
        {
            if (!ModelState.IsValid)
            {
                return View(oficina);
            }

            oficina.Activo = true;

            await context.Oficinas.AddAsync(oficina);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"La oficina o centro '{oficina.Codigo} - {oficina.Nombre}' se ha creado." });
        }


        // Acción de Activar Oficina
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(int id)
        {
            var oficina = await context.Oficinas.Where(o => o.Id == id).FirstOrDefaultAsync();

            if (oficina is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            oficina.Activo = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"La oficina o centro '{oficina.Codigo} - {oficina.Nombre}' se ha activado." });
        }

        // Acción de Desactivar Oficina
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(int id)
        {
            var oficina = await context.Oficinas.Where(o => o.Id == id).FirstOrDefaultAsync();

            if (oficina is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            oficina.Activo = false;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", routeValues: new { mensaje = $"La oficina o centro '{oficina.Codigo} - {oficina.Nombre}' se ha desactivado." });
        }

        // Editar Oficina

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(int id)
        {

            Oficina oficina = await context.Oficinas.Where(o => o.Id == id).FirstOrDefaultAsync();

            if (oficina is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(oficina);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(Oficina oficina)
        {
            if (!ModelState.IsValid)
            {
                return View(oficina);
            }

            oficina.Activo = true;

            context.Oficinas.Update(oficina);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"La oficina o centro '{oficina.Codigo} - {oficina.Nombre}' se ha modificado." });
        }

    }
}
