
using GestionIncidencias.Entidades;

namespace GestionIncidencias.Controllers
{
    public class CalendarioController : Controller
    {
        private readonly ApplicationDbContext context;

        public CalendarioController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TiposDispositivos = await context.TiposDispositivos.ToListAsync();
            return View();
        }

        public async Task<JsonResult> ObtenerIncidencias(DateTime start, DateTime end)
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();

            var incidenciasConsulta =   from incidencia in incidencias
                                        join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                        join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                        join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                        where incidencia.FechaHora > start.AddDays(-1) && incidencia.FechaHora < end.AddDays(1) && incidencia.Activo == true
                                        select new
                                        {
                                            incidencia,
                                            dispositivo,
                                            modelo,
                                            oficina
                                        };


            var incidenciasCalendario = incidenciasConsulta.Select(incidencia => new IncidenciaCalendario()
            {
                Title = incidencia.oficina.Codigo + " - " + incidencia.modelo.Marca + " " + incidencia.modelo.Modelo,
                Start = incidencia.incidencia.FechaHora.ToString("yyyy-MM-dd"),
                End = incidencia.incidencia.FechaHora.ToString("yyyy-MM-dd"),
                Color = Constantes.asignarColor(incidencia.modelo.TipoDispositivoId)
            });

            return Json(incidenciasCalendario.ToList());
        }

        public async Task<JsonResult> ObtenerIncidenciasPorFecha(DateTime fecha)
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();
            var tecnicos = await context.Tecnicos.ToListAsync();

            var incidenciasConsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      join tipo in tiposDispositivos on modelo.TipoDispositivoId equals tipo.Id
                                      join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                      join tecnico in tecnicos on incidencia.TecnicoId equals tecnico.Id
                                      where incidencia.FechaHora.ToShortDateString() == fecha.ToShortDateString() && incidencia.Activo == true
                                      select new
                                      {
                                          incidencia,
                                          dispositivo,
                                          modelo,
                                          tipo,
                                          oficina,
                                          tecnico
                                      };

            var detallesDia = incidenciasConsulta.Select(detalle => new DetalleIncidenciaCalendario()
            {
                Id = detalle.incidencia.Id,
                Usuario = detalle.incidencia.Usuario,
                Hora = detalle.incidencia.FechaHora.ToShortTimeString(),
                CodOficina = detalle.oficina.Codigo,
                TipoDispositivo = detalle.tipo.Titulo,
                ModeloDispositivo = detalle.modelo.Marca + " " + detalle.modelo.Modelo,
                Tecnico = detalle.tecnico.NombreEmpresa,
                Descripcion = detalle.incidencia.Descripcion
            });
            

            return Json(detallesDia.ToList());
        }

    }
}