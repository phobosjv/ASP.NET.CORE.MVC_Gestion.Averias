
namespace GestionIncidencias.Controllers
{
    public class GraficosController : Controller
    {
        private readonly ApplicationDbContext context;

        public GraficosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> ObtenerIncidenciasGraficoLineas(DateTime start, DateTime end)
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();

            List<ElementoGraficoLineas> incidenciasGrafico = new List<ElementoGraficoLineas>();

            foreach (TipoDispositivo tp in tiposDispositivos)
            {
                ElementoGraficoLineas elemento = new ElementoGraficoLineas();

                elemento.Label = tp.Titulo;
                elemento.BorderColor = Constantes.asignarColor(tp.Id);
                elemento.Fill = false;
                elemento.Tension = 0.1f;

                List<int> lista = new List<int>();

                for (DateTime fecha = start; fecha < end; fecha = fecha.AddMonths(1))
                {
                    var subconsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      where incidencia.FechaHora > fecha.AddDays(-1) && incidencia.FechaHora < fecha.AddMonths(1) && incidencia.Activo == true && modelo.TipoDispositivoId == tp.Id
                                      select new
                                      {
                                          incidencia
                                      };
                    int resultado = subconsulta.Count();
                    lista.Add(resultado);
                }
                elemento.Data = lista.ToArray();

                incidenciasGrafico.Add(elemento);
            }

            return Json(incidenciasGrafico);
        }

        public async Task<JsonResult> ObtenerIncidenciasGraficoUsuarios()
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var usuarios = await context.Users.ToListAsync();

            List<int> data = new List<int>(); // Cuenta de Incidencias por Usuario
            List<string> labels = new List<string>(); // Códigos de Usuarios
            List<string> backgroundColor = new List<string>();
            int contador = 6;

            foreach (IdentityUser usuario in usuarios)
            {
                var subconsulta = from incidencia in incidencias
                                  where incidencia.Usuario == usuario.UserName
                                  select new
                                  {
                                      incidencia
                                  };
                int resultado = subconsulta.Count();

                if (resultado > 0)
                {
                    data.Add(resultado);
                    labels.Add(usuario.UserName);
                    backgroundColor.Add(Constantes.asignarColor(contador));
                    contador++;
                }
            }

            DatosGraficoDonut datosGraficoDonut = new DatosGraficoDonut();
            datosGraficoDonut.Labels = labels.ToArray();
            DatasetDonut datasetDonut = new DatasetDonut();
            datasetDonut.Label = "Incidencias por Usuario";
            datasetDonut.Data = data.ToArray();
            datasetDonut.BackgroundColor = backgroundColor.ToArray();
            datosGraficoDonut.Datasets = (new DatasetDonut[] { datasetDonut }).ToArray();

            return Json(datosGraficoDonut);

        }

        public async Task<JsonResult> ObtenerIncidenciasGraficoOficinas()
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var ubicaciones = await context.Oficinas.ToListAsync();


            List<Tuple<string, int, string>> elementos = new List<Tuple<string, int, string>>();
            // La primera posición es el nombre de la ubicación, la segunda la cantidad de incidencias y la tercera el color

            int contador = 9;

            foreach (Oficina oficina in ubicaciones)
            {
                var subconsulta = from incidencia in incidencias
                                  where incidencia.OficinaId == oficina.Id && incidencia.Activo == true
                                  select new
                                  {
                                      incidencia
                                  };
                int resultado = subconsulta.Count();

                elementos.Add(new Tuple<string, int, string>(oficina.Nombre, resultado, "#409B88"));
                contador++;
            }

            elementos.Sort((x, y) => y.Item2.CompareTo(x.Item2));
            elementos = elementos.Take(10).ToList();


            List<string> labels = new List<string>();
            List<int> data = new List<int>();
            List<string> backgroundColor = new List<string>();
           
            foreach (Tuple<string, int, string> elemento in elementos)
            {
                labels.Add(elemento.Item1);
                data.Add(elemento.Item2);
                backgroundColor.Add(elemento.Item3);
            }
            
            DatosGraficoDonut datosGraficoDonut = new DatosGraficoDonut();
            datosGraficoDonut.Labels = labels.ToArray();
            DatasetDonut datasetDonut = new DatasetDonut();
            datasetDonut.Data = data.ToArray();
            datasetDonut.BackgroundColor = backgroundColor.ToArray();
            datosGraficoDonut.Datasets = (new DatasetDonut[] { datasetDonut }).ToArray();

            return Json(datosGraficoDonut);

        }
    }
}