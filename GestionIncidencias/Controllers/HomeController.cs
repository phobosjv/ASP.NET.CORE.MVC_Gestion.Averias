
using GestionIncidencias.Entidades;

namespace GestionIncidencias.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private readonly IServicioCorreo servicioCorreo;

        public HomeController(/*ILogger<HomeController> logger, */ ApplicationDbContext context, IServicioCorreo servicioCorreo)
        {
            //_logger = logger;
            this.context = context;
            this.servicioCorreo = servicioCorreo;
        }

        public async Task<IActionResult> Index(string mensaje = null)
        {
            var incidencias = await context.Incidencias.Select(i => new Incidencia
            {
                Id = i.Id,
                FechaHora = i.FechaHora,
                Usuario = i.Usuario,
                OficinaId = i.OficinaId,
                Oficina = i.Oficina,
                DispositivoId = i.DispositivoId,
                Dispositivo = i.Dispositivo,
                Descripcion = i.Descripcion,
                Activo = i.Activo
            }).OrderByDescending(i => i.Id).ToListAsync();

            var modelo = new IncidenciasListadoViewModel();
            modelo.Incidencias = incidencias;
            modelo.Mensaje = mensaje;

            ViewBag.ModelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            ViewBag.TiposDispositivos = await context.TiposDispositivos.ToListAsync();
            ViewBag.Tecnicos = await context.Tecnicos.ToListAsync();

            return View(modelo);
        }

        public async Task<IActionResult> AveriasDia(DateTime dia, string mensaje = null)
        {
            var incidencias = await context.Incidencias.Select(i => new Incidencia
            {
                Id = i.Id,
                FechaHora = i.FechaHora,
                Usuario = i.Usuario,
                OficinaId = i.OficinaId,
                Oficina = i.Oficina,
                DispositivoId = i.DispositivoId,
                Dispositivo = i.Dispositivo,
                Descripcion = i.Descripcion,
                Activo = i.Activo
            }).Where(i => i.FechaHora.Date == dia.Date).OrderByDescending(i => i.Id).ToListAsync();

            var modelo = new IncidenciasDiaViewModel();
            modelo.Incidencias = incidencias;
            modelo.Mensaje = mensaje;
            modelo.Dia = dia;

            ViewBag.ModelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            ViewBag.TiposDispositivos = await context.TiposDispositivos.ToListAsync();
            ViewBag.Tecnicos = await context.Tecnicos.ToListAsync();

            return View(modelo);
        }

        public async Task<IActionResult> Crear_Seleccionar()
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
            }).Where(d => d.Activo).OrderBy(d => d.Oficina.Codigo).ThenBy(d => d.ModeloDispositivo.TipoDispositivoId).ToListAsync();

            var modelo = new DispositivosListadoViewModel();
            modelo.Dispositivos = dispositivos;

            ViewBag.SelectOficina = await context.Oficinas.Select(o => new Oficina
            {
                Id = o.Id,
                Codigo = o.Codigo + " - " + o.Nombre,
                Activo = o.Activo
            }).Where(o => o.Activo && !o.Codigo.Contains("0000")).OrderBy(o => o.Codigo).ToListAsync();

            ViewBag.SelectTiposDispositivos = await context.TiposDispositivos.Where(td => td.Activo).OrderBy(td => td.Titulo).ToListAsync();

            return View(modelo);
        }

        [HttpGet]
        public async Task<IActionResult> Crear_Registrar(int centroId, int dispositivoId)
        {
            var oficina = await context.Oficinas.Where(o => o.Id == centroId).FirstOrDefaultAsync();
            ViewBag.oficina = oficina;

            var dispositivo = await context.Dispositivos.Where(d => d.Id == dispositivoId).FirstOrDefaultAsync();
            ViewBag.dispositivo = dispositivo;

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            ViewBag.modeloDispositivo = modeloDispositivo;

            var tecnico = await context.Tecnicos.Where(t => t.Id == modeloDispositivo.TecnicoId).FirstOrDefaultAsync();
            ViewBag.tecnico = tecnico;

            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();
            ViewBag.tipoDispositivo = tipoDispositivo;

            string asunto = await sustituirComodines(tecnico.Asunto, oficina, dispositivo);
            ViewBag.asunto = asunto;

            string texto = await sustituirComodines(tecnico.Texto, oficina, dispositivo);
            ViewBag.texto = texto;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear_Registrar(Incidencia incidencia)
        {
            if (!ModelState.IsValid)
            {
                return View(incidencia);
            }

            incidencia.Activo = true;

            await context.Incidencias.AddAsync(incidencia);
            await context.SaveChangesAsync();

            string resultadoEnvio = await mandarCorreoIncidencia(incidencia);

            return RedirectToAction("Index", routeValues: new { mensaje = $"La incidencia se ha creado en la base de datos.", resultadoEnvio });

        }

        // Examinar Avería
        [HttpGet]
        public async Task<IActionResult> Examinar(int id)
        {
            Incidencia incidencia = await context.Incidencias.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (incidencia is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var dispositivo = await context.Dispositivos.Where(d => d.Id == incidencia.DispositivoId).FirstOrDefaultAsync();
            ViewBag.dispositivo = dispositivo;

            var oficina = await context.Oficinas.Where(o => o.Id == incidencia.OficinaId).FirstOrDefaultAsync();
            ViewBag.oficina = oficina;

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            ViewBag.modeloDispositivo = modeloDispositivo;

            var tecnico = await context.Tecnicos.Where(t => t.Id == modeloDispositivo.TecnicoId).FirstOrDefaultAsync();
            ViewBag.tecnico = tecnico;

            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();
            ViewBag.tipoDispositivo = tipoDispositivo;

            return View(incidencia);
        }

        // PreCancelar Avería
        [HttpGet]
        public async Task<IActionResult> PreCancelar(int id)
        {
            Incidencia incidencia = await context.Incidencias.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (incidencia is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var dispositivo = await context.Dispositivos.Where(d => d.Id == incidencia.DispositivoId).FirstOrDefaultAsync();
            ViewBag.dispositivo = dispositivo;

            var oficina = await context.Oficinas.Where(o => o.Id == incidencia.OficinaId).FirstOrDefaultAsync();
            ViewBag.oficina = oficina;

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            ViewBag.modeloDispositivo = modeloDispositivo;

            var tecnico = await context.Tecnicos.Where(t => t.Id == modeloDispositivo.TecnicoId).FirstOrDefaultAsync();
            ViewBag.tecnico = tecnico;

            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();
            ViewBag.tipoDispositivo = tipoDispositivo;

            return View(incidencia);
        }

        [HttpPost]
        public async Task<IActionResult> PreCancelar(Incidencia incidencia)
        {
            if (!ModelState.IsValid)
            {
                return View(incidencia);
            }

            incidencia.Activo = false;

            context.Incidencias.Update(incidencia);
            await context.SaveChangesAsync();

            string resultadoEnvio = await mandarCorreoCancelacion(incidencia);

            return RedirectToAction("Index", routeValues: new { mensaje = $"La incidencia con fecha '{incidencia.FechaHora.ToString("yyyy-MM-dd HH:mm")}' se ha cancelado", resultadoEnvio });

        }

        // PreReclamar Avería
        [HttpGet]
        public async Task<IActionResult> PreReclamar(int id)
        {
            Incidencia incidencia = await context.Incidencias.Where(i => i.Id == id).FirstOrDefaultAsync();

            if (incidencia is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var dispositivo = await context.Dispositivos.Where(d => d.Id == incidencia.DispositivoId).FirstOrDefaultAsync();
            ViewBag.dispositivo = dispositivo;

            var oficina = await context.Oficinas.Where(o => o.Id == incidencia.OficinaId).FirstOrDefaultAsync();
            ViewBag.oficina = oficina;

            var modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            ViewBag.modeloDispositivo = modeloDispositivo;

            var tecnico = await context.Tecnicos.Where(t => t.Id == modeloDispositivo.TecnicoId).FirstOrDefaultAsync();
            ViewBag.tecnico = tecnico;

            var tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();
            ViewBag.tipoDispositivo = tipoDispositivo;

            return View(incidencia);
        }

        [HttpPost]
        public async Task<IActionResult> PreReclamar(Incidencia incidencia)
        {
            if (!ModelState.IsValid)
            {
                return View(incidencia);
            }

            incidencia.Activo = true;

            context.Incidencias.Update(incidencia);
            await context.SaveChangesAsync();

            string resultadoEnvio = await mandarCorreoReclamacion(incidencia);

            return RedirectToAction("Index", routeValues: new { mensaje = $"La incidencia con fecha '{incidencia.FechaHora.ToString("yyyy-MM-dd HH:mm")}' se ha reclamado", resultadoEnvio });

        }


        private async Task<string> mandarCorreoIncidencia(Incidencia incidencia)
        {
            Tecnico tecnico = await context.Tecnicos.Where(t => t.Id == incidencia.TecnicoId).FirstOrDefaultAsync();

            MensajeCorreo mensajeCorreo = new MensajeCorreo(tecnico.CorreoPara, 
                                                            await sustituirComodines(tecnico.Asunto, incidencia),
                                                            incidencia.Descripcion,
                                                            tecnico.CorreoCopia ?? ""
                                                            );

            return servicioCorreo.EnviarCorreo(mensajeCorreo);

        }

        private async Task<string> mandarCorreoCancelacion(Incidencia incidencia)
        {
            Tecnico tecnico = await context.Tecnicos.Where(t => t.Id == incidencia.TecnicoId).FirstOrDefaultAsync();

            MensajeCorreo mensajeCorreo = new MensajeCorreo(tecnico.CorreoPara,
                                                            "CANCELAR: " + await sustituirComodines(tecnico.Asunto, incidencia),
                                                            incidencia.Descripcion,
                                                            tecnico.CorreoCopia ?? ""
                                                            );

            return servicioCorreo.EnviarCorreo(mensajeCorreo);

        }

        private async Task<string> mandarCorreoReclamacion(Incidencia incidencia)
        {
            Tecnico tecnico = await context.Tecnicos.Where(t => t.Id == incidencia.TecnicoId).FirstOrDefaultAsync();

            MensajeCorreo mensajeCorreo = new MensajeCorreo(tecnico.CorreoPara,
                                                            "RECLAMAR: " + await sustituirComodines(tecnico.Asunto, incidencia),
                                                            incidencia.Descripcion,
                                                            tecnico.CorreoCopia ?? ""
                                                            );

            return servicioCorreo.EnviarCorreo(mensajeCorreo);

        }

        private async Task<string> sustituirComodines(string texto, Incidencia incidencia)
        {
            Dispositivo dispositivo = await context.Dispositivos.Where(d => d.Id == incidencia.DispositivoId).FirstOrDefaultAsync();
            ModeloDispositivo modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            TipoDispositivo tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();
            Oficina oficina = await context.Oficinas.Where(o => o.Id == incidencia.OficinaId).FirstOrDefaultAsync();

            return sustituirComodines(texto, oficina, dispositivo, tipoDispositivo, modeloDispositivo);
        }

        private async Task<string> sustituirComodines(string texto, Oficina oficina, Dispositivo dispositivo)
        {
            ModeloDispositivo modeloDispositivo = await context.ModelosDispositivos.Where(md => md.Id == dispositivo.ModeloDispositivoId).FirstOrDefaultAsync();
            TipoDispositivo tipoDispositivo = await context.TiposDispositivos.Where(td => td.Id == modeloDispositivo.TipoDispositivoId).FirstOrDefaultAsync();

            return sustituirComodines(texto, oficina, dispositivo, tipoDispositivo, modeloDispositivo);
        }

        private static string sustituirComodines(string texto, Oficina oficina, Dispositivo dispositivo, TipoDispositivo tipoDispositivo, ModeloDispositivo modeloDispositivo)
        {
            string resultado = texto.Replace("[CENTRO]", oficina.Codigo)
                                    .Replace("[TIPO]", tipoDispositivo.Titulo)
                                    .Replace("[SN]", dispositivo.NumeroSerie)
                                    .Replace("[MODELO]", modeloDispositivo.Marca + " - " + modeloDispositivo.Modelo)
                                    .Replace("[OFICINA]", oficina.Nombre)
                                    .Replace("[DIRECCION]", oficina.Direccion)
                                    .Replace("[TELEFONO]", oficina.Telefono)
                                    .Replace("[COMENTARIOS]", oficina.Nota);
            return resultado;
        }


        public IActionResult NoEncontrado()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}