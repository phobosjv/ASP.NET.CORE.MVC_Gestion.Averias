
namespace GestionIncidencias.Controllers
{
    public class TiposDispositivosController : Controller
    {
        private readonly ApplicationDbContext context;

        public TiposDispositivosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // Acción para el Index de Tipos de Dispositivos

        public async Task<IActionResult> Index(string mensaje = null)
        {
            var tiposDispositivos = await context.TiposDispositivos.Select(td => new TipoDispositivoViewModel
            {
                Id = td.Id,
                Titulo = td.Titulo,
                Descripcion = td.Descripcion,
                Activo = td.Activo
            }).OrderByDescending(td => td.Activo).ThenBy(td => td.Titulo).ToListAsync();

            var modelo = new TiposDispositivosListadoViewModel();
            modelo.TiposDispositivos = tiposDispositivos;
            modelo.Mensaje = mensaje;

            return View(modelo);
        }


        // Crear Nuevo Tipo de Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear(TipoDispositivo tipoDispositivo)
        {
            if (!ModelState.IsValid)
            {
                return View(tipoDispositivo);
            }

            tipoDispositivo.Activo = true;

            await context.TiposDispositivos.AddAsync(tipoDispositivo);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El tipo de dispositivo '{tipoDispositivo.Titulo}' se ha creado." });
        }


        // Activar Tipo de Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(int id)
        {
            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == id).FirstOrDefaultAsync();

            if (tipoDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            tipoDispositivo.Activo = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El tipo de dispositivo '{tipoDispositivo.Titulo}' se ha activado" });
        }


        // Desactivar Tipo de Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(int id)
        {
            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == id).FirstOrDefaultAsync();

            if (tipoDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            tipoDispositivo.Activo = false;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El tipo de dispositivo '{tipoDispositivo.Titulo}' se ha desactivado" });
        }


        // Editar Tipo Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(int id)
        {
            TipoDispositivo tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == id).FirstOrDefaultAsync();

            if (tipoDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(tipoDispositivo);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(TipoDispositivo tipoDispositivo)
        {
            if(!ModelState.IsValid)
            {
                return View(tipoDispositivo);
            }

            tipoDispositivo.Activo = true;

            context.TiposDispositivos.Update(tipoDispositivo);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El tipo de dispositivo '{tipoDispositivo.Titulo}' se ha modificado" });
        }
    }
}
