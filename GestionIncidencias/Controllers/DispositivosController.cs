
namespace GestionIncidencias.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly ApplicationDbContext context;

        public DispositivosController(ApplicationDbContext context)
        {
            this.context = context;
        }


        // Acción para el index de Dispositivos
        public async Task<IActionResult> Index(string mensaje = null)
        {
            var dispositivos = await context.Dispositivos.Select(d => new Dispositivo
            {
                Id = d.Id,
                NumeroSerie = d.NumeroSerie,
                ModeloDispositivoId = d.ModeloDispositivoId,
                ModeloDispositivo = d.ModeloDispositivo,
                OficinaId = d.OficinaId,
                Oficina = d.Oficina,
                Activo = d.Activo
            }).OrderByDescending(d => d.Activo).ThenBy(d =>d.Oficina.Codigo).ToListAsync();

            var modelo = new DispositivosListadoViewModel();
            modelo.Dispositivos = dispositivos;
            modelo.Mensaje = mensaje;

            ViewBag.TD = await context.TiposDispositivos.ToListAsync();

            return View(modelo);
        }

        // Crear Nuevo Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear()
        {

            ViewBag.SelectModeloDispositivos = await context.ModelosDispositivos.Select(md => new ModeloDispositivo
            {
                Id = md.Id,
                Modelo = md.Marca + " - " + md.Modelo,
                Activo = md.Activo,
                TipoDispositivoId = md.TipoDispositivoId
            }).Where(md => md.Activo).OrderBy(md => md.Modelo).ToListAsync();

            ViewBag.SelectOficina = await context.Oficinas.Select(o => new Oficina
            {
                Id = o.Id,
                Codigo = o.Codigo + " - " + o.Nombre,
                Activo = o.Activo
            }).Where(o => o.Activo).OrderBy(o => o.Codigo).ToListAsync();

            ViewBag.SelectTiposDispositivos = await context.TiposDispositivos.Where(td => td.Activo).OrderBy(td => td.Titulo).ToListAsync();

            return View();
  
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Crear(Dispositivo dispositivo)
        {
            if (!ModelState.IsValid)
            {
                return View(dispositivo);
            }

            dispositivo.Activo = true;

            await context.Dispositivos.AddAsync(dispositivo);

            await context.SaveChangesAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El dispositivo con S/N '{dispositivo.NumeroSerie}' se ha creado." });
        }


        // Activar Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(int id)
        {
            var dispositivo = await context.Dispositivos.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (dispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            dispositivo.Activo = true;

            await context.SaveChangesAsync();

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El dispositivo '{modeloDispositivo.Marca} - {modeloDispositivo.Modelo}' con S/N '{dispositivo.NumeroSerie}' se ha activado." });
        }


        // Desactivar Dispositivo

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(int id)
        {
            var dispositivo = await context.Dispositivos.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (dispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            dispositivo.Activo = false;

            await context.SaveChangesAsync();

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El dispositivo '{modeloDispositivo.Marca} - {modeloDispositivo.Modelo}' con S/N '{dispositivo.NumeroSerie}' se ha desactivado." });
        }


        // Editar Modelo Dispositivo

        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(int id)
        {
            Dispositivo dispositivo = await context.Dispositivos.Where(d => d.Id == id).FirstOrDefaultAsync();

            if (dispositivo is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            ViewBag.SelectModeloDispositivos = await context.ModelosDispositivos.Select(md => new ModeloDispositivo
            {
                Id = md.Id,
                Modelo = md.Marca + " - " + md.Modelo,
                Activo = md.Activo,
                TipoDispositivoId = md.TipoDispositivoId
            }).Where(md => md.Activo).OrderBy(md => md.Modelo).ToListAsync();

            ViewBag.SelectOficina = await context.Oficinas.Select(o => new Oficina
            {
                Id = o.Id,
                Codigo = o.Codigo + " - " + o.Nombre,
                Activo = o.Activo
            }).Where(o => o.Activo).OrderBy(o => o.Codigo).ToListAsync();

            ViewBag.SelectTiposDispositivos = await context.TiposDispositivos.Where(td => td.Activo).OrderBy(td => td.Titulo).ToListAsync();

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            ViewBag.TipoDispositivo = modeloDispositivo.TipoDispositivoId.ToString();

            return View(dispositivo);
        }

        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Editar(Dispositivo dispositivo)
        {
            if (!ModelState.IsValid)
            {
                return View(dispositivo);
            }

            dispositivo.Activo = true;

            context.Dispositivos.Update(dispositivo);

            await context.SaveChangesAsync();

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();

            return RedirectToAction("Index", routeValues: new { mensaje = $"El dispositivo '{modeloDispositivo.Marca} - {modeloDispositivo.Modelo}' con S/N '{dispositivo.NumeroSerie}' se ha modificado." });

        }

    }
}
