
namespace GestionIncidencias.Controllers
{
    public class ModelosDispositivosController: Controller
    {
        private readonly ApplicationDbContext context;

        public ModelosDispositivosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // Acción para el Index de Modelos Dispositivos

        public async Task<IActionResult> Index(string mensaje=null)
        {
            var modelosDispositivo = await context.ModelosDispositivos.Select(md => new ModeloDispositivo
            {
                Id = md.Id,
                Marca = md.Marca,
                Modelo = md.Modelo,
                TipoDispositivoId = md.TipoDispositivoId,
                TipoDispositivo = md.TipoDispositivo,
                TecnicoId = md.TecnicoId,
                Tecnico = md.Tecnico,
                Descripcion = md.Descripcion,
                Activo = md.Activo
            }).OrderByDescending(md => md.Activo).ThenBy(md => md.TipoDispositivo).ToListAsync();

            var modelo = new ModelosDispositivosListadoViewModel();
            modelo.ModelosDispositivos = modelosDispositivo;
            modelo.Mensaje = mensaje;

            return View(modelo);
        }


        // Crear Nuevo Modelo de Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear()
        {
            ViewBag.SelectTiposDispositivos = await context.TiposDispositivos.Where(td => td.Activo).OrderBy(td => td.Titulo).ToListAsync();
            ViewBag.SelectTecnicos = await context.Tecnicos.Where(t => t.Activo).OrderBy(t => t.NombreEmpresa).ToListAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear(ModeloDispositivo modeloDispositivo)
        {
            if (!ModelState.IsValid)
            {
                return View(modeloDispositivo);
            }

            modeloDispositivo.Activo = true;

            await context.ModelosDispositivos.AddAsync(modeloDispositivo);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El Modelo de dispositivo '{modeloDispositivo.Marca + " - " + modeloDispositivo.Modelo}' se ha creado." });
        }


        // Activar Modelo de Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(int id)
        {
            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == id).FirstOrDefaultAsync();

            if (modeloDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            modeloDispositivo.Activo = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El modelo de dispositivo '{modeloDispositivo.Marca + " - " + modeloDispositivo.Modelo}' se ha activado" });
        }


        // Desactivar Modelo de Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(int id)
        {
            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == id).FirstOrDefaultAsync();

            if (modeloDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            modeloDispositivo.Activo = false;

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El modelo de dispositivo '{modeloDispositivo.Marca + " - " + modeloDispositivo.Modelo}' se ha desactivado" });
        }


        // Editar Modelo Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(int id)
        {
            ModeloDispositivo modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == id).FirstOrDefaultAsync();

            if (modeloDispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            ViewBag.SelectTiposDispositivos = await context.TiposDispositivos.Where(td => td.Activo).OrderBy(td => td.Titulo).ToListAsync();
            ViewBag.SelectTecnicos = await context.Tecnicos.Where(t => t.Activo).OrderBy(t => t.NombreEmpresa).ToListAsync();

            return View(modeloDispositivo);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(ModeloDispositivo modeloDispositivo)
        {
            if (!ModelState.IsValid)
            {
                return View(modeloDispositivo);
            }

            modeloDispositivo.Activo = true;

            context.ModelosDispositivos.Update(modeloDispositivo);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El modelo de dispositivo '{modeloDispositivo.Marca + " - " + modeloDispositivo.Modelo}' se ha modificado" });
        }

    }
}
