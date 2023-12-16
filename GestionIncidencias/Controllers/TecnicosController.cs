
namespace GestionIncidencias.Controllers
{
    public class TecnicosController: Controller
    {
        private readonly ApplicationDbContext context;

        public TecnicosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Accion para el Index de Técnicos
        public async Task<IActionResult> Index(string mensaje = null)
        {
            var tecnicos = await context.Tecnicos.Select(t => new TecnicoViewModel
            {
                Id = t.Id,
                NombreEmpresa = t.NombreEmpresa,
                CorreoElectronico = t.CorreoPara,
                Telefono = t.Telefono,
                Activado = t.Activo
            }).OrderByDescending(t => t.Activado).ThenBy(t => t.NombreEmpresa).ToListAsync();

            var modelo = new TecnicosListadoViewModel();
            modelo.Tecnicos = tecnicos;
            modelo.Mensaje = mensaje;

            return View(modelo);
        }

        // Acción de Activar Técnico
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(int id)
        {
            var tecnico = await context.Tecnicos.Where(t => t.Id == id).FirstOrDefaultAsync();

            if (tecnico is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            tecnico.Activo = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El servicio técnico '{tecnico.NombreEmpresa}' ha sido activado" });
        }

        // Acción de Desactivar Técnico
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(int id)
        {
            var tecnico = await context.Tecnicos.Where(t => t.Id == id).FirstOrDefaultAsync();

            if (tecnico is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            tecnico.Activo = false;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El servicio técnico '{tecnico.NombreEmpresa}' ha sido desactivado" });
        }

        // Crear nuevo Servicio Técnico
        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear(Tecnico tecnico)
        {
            if (!ModelState.IsValid)
            {
                return View(tecnico);
            }

            tecnico.Activo = true;

            await context.Tecnicos.AddAsync(tecnico);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El servicio técnico '{tecnico.NombreEmpresa}' ha sido creado" });
        }


        // Editar Servicio Técnico

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(int id)
        {

            Tecnico tecnico = await context.Tecnicos.Where(t => t.Id == id).FirstOrDefaultAsync();

            if (tecnico is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tecnico);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(Tecnico tecnico)
        {
            if (!ModelState.IsValid)
            {
                return View(tecnico);
            }

            tecnico.Activo = true;

            context.Tecnicos.Update(tecnico);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El servicio técnico '{tecnico.NombreEmpresa}' ha sido modificado" });
        }

    }
}
