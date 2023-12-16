namespace GestionIncidencias.Controllers
{
    public class ExportarController : Controller
    {
        private readonly ApplicationDbContext context;

        public ExportarController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var incidencias = await context.Incidencias.ToListAsync();
            var incidenciasOrdenado = incidencias.OrderBy(i => i.FechaHora);
            int anyoMin = incidenciasOrdenado.First().FechaHora.Year;
            int anyoMax = incidenciasOrdenado.Last().FechaHora.Year;
            ViewBag.anyoMin = anyoMin;
            ViewBag.anyoMax = anyoMax;

            return View();
        }

        [HttpGet]
        public async Task<FileResult> ExportarExcelTodo()
        {

            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();
            var tecnicos = await context.Tecnicos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();

            var incidenciasConsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      join tipo in tiposDispositivos on modelo.TipoDispositivoId equals tipo.Id
                                      join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                      join tecnico in tecnicos on incidencia.TecnicoId equals tecnico.Id
                                      select new
                                      {
                                          incidencia,
                                          oficina,
                                          dispositivo,
                                          modelo,
                                          tipo,
                                          tecnico
                                      };

            var incidenciasExportar = incidenciasConsulta.Select(ic => new IncidenciaExportar()
            {
                Id = ic.incidencia.Id,
                FechaHora = ic.incidencia.FechaHora,
                Usuario = ic.incidencia.Usuario,
                CodOficina = ic.oficina.Codigo,
                NombreOficina = ic.oficina.Nombre,
                TipoDispositivo = ic.tipo.Titulo,
                ModeloDispositivo = ic.modelo.Marca + " " + ic.modelo.Modelo,
                Tecnico = ic.tecnico.NombreEmpresa,
                NumSerie = ic.dispositivo.NumeroSerie,
                Mensaje = ic.incidencia.Descripcion
            });

            var nombreArchivo = $"HistóricoAverías.xlsx";

            return GenerarExcel(nombreArchivo, incidenciasExportar);


        }

        public async Task<FileResult> ExportarExcelTodoSinDescripcion()
        {

            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();
            var tecnicos = await context.Tecnicos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();

            var incidenciasConsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      join tipo in tiposDispositivos on modelo.TipoDispositivoId equals tipo.Id
                                      join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                      join tecnico in tecnicos on incidencia.TecnicoId equals tecnico.Id
                                      select new
                                      {
                                          incidencia,
                                          oficina,
                                          dispositivo,
                                          modelo,
                                          tipo,
                                          tecnico
                                      };

            var incidenciasExportar = incidenciasConsulta.Select(ic => new IncidenciaExportar()
            {
                Id = ic.incidencia.Id,
                FechaHora = ic.incidencia.FechaHora,
                Usuario = ic.incidencia.Usuario,
                CodOficina = ic.oficina.Codigo,
                NombreOficina = ic.oficina.Nombre,
                TipoDispositivo = ic.tipo.Titulo,
                ModeloDispositivo = ic.modelo.Marca + " " + ic.modelo.Modelo,
                NumSerie = ic.dispositivo.NumeroSerie,
                Tecnico = ic.tecnico.NombreEmpresa
            });

            var nombreArchivo = $"HistóricoAverías.xlsx";

            return GenerarExcelSinDescripcion(nombreArchivo, incidenciasExportar);


        }

        public async Task<FileResult> ExportarExcelAnyo(string anyo)
        {
            int intAnyo = 0;
            int.TryParse(anyo, out intAnyo);

            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();
            var tecnicos = await context.Tecnicos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();

            var incidenciasConsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      join tipo in tiposDispositivos on modelo.TipoDispositivoId equals tipo.Id
                                      join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                      join tecnico in tecnicos on incidencia.TecnicoId equals tecnico.Id
                                      where incidencia.FechaHora.Year == intAnyo
                                      select new
                                      {
                                          incidencia,
                                          oficina,
                                          dispositivo,
                                          modelo,
                                          tipo,
                                          tecnico
                                      };

            var incidenciasExportar = incidenciasConsulta.Select(ic => new IncidenciaExportar()
            {
                Id = ic.incidencia.Id,
                FechaHora = ic.incidencia.FechaHora,
                Usuario = ic.incidencia.Usuario,
                CodOficina = ic.oficina.Codigo,
                NombreOficina = ic.oficina.Nombre,
                TipoDispositivo = ic.tipo.Titulo,
                ModeloDispositivo = ic.modelo.Marca + " " + ic.modelo.Modelo,
                Tecnico = ic.tecnico.NombreEmpresa,
                NumSerie = ic.dispositivo.NumeroSerie,
                Mensaje = ic.incidencia.Descripcion
            });

            var nombreArchivo = $"Averías-{anyo}.xlsx";

            return GenerarExcel(nombreArchivo, incidenciasExportar);


        }

        public async Task<FileResult> ExportarExcelAnyoSinDescripcion(string anyo)
        {
            int intAnyo = 0;
            int.TryParse(anyo, out intAnyo);

            var incidencias = await context.Incidencias.ToListAsync();
            var dispositivos = await context.Dispositivos.ToListAsync();
            var modelosDispositivos = await context.ModelosDispositivos.ToListAsync();
            var oficinas = await context.Oficinas.ToListAsync();
            var tecnicos = await context.Tecnicos.ToListAsync();
            var tiposDispositivos = await context.TiposDispositivos.ToListAsync();

            var incidenciasConsulta = from incidencia in incidencias
                                      join dispositivo in dispositivos on incidencia.DispositivoId equals dispositivo.Id
                                      join modelo in modelosDispositivos on dispositivo.ModeloDispositivoId equals modelo.Id
                                      join tipo in tiposDispositivos on modelo.TipoDispositivoId equals tipo.Id
                                      join oficina in oficinas on incidencia.OficinaId equals oficina.Id
                                      join tecnico in tecnicos on incidencia.TecnicoId equals tecnico.Id
                                      where incidencia.FechaHora.Year == intAnyo
                                      select new
                                      {
                                          incidencia,
                                          oficina,
                                          dispositivo,
                                          modelo,
                                          tipo,
                                          tecnico
                                      };

            var incidenciasExportar = incidenciasConsulta.Select(ic => new IncidenciaExportar()
            {
                Id = ic.incidencia.Id,
                FechaHora = ic.incidencia.FechaHora,
                Usuario = ic.incidencia.Usuario,
                CodOficina = ic.oficina.Codigo,
                NombreOficina = ic.oficina.Nombre,
                TipoDispositivo = ic.tipo.Titulo,
                ModeloDispositivo = ic.modelo.Marca + " " + ic.modelo.Modelo,
                NumSerie = ic.dispositivo.NumeroSerie,
                Tecnico = ic.tecnico.NombreEmpresa
            });

            var nombreArchivo = $"Averías-{anyo}.xlsx";

            return GenerarExcelSinDescripcion(nombreArchivo, incidenciasExportar);

        }

        private FileResult GenerarExcel(string nombreArchivo, IEnumerable<IncidenciaExportar> incidencias)
        {
            DataTable dataTable = new DataTable("Incidencias");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("FechaHora"),
                new DataColumn("Usuario"),
                new DataColumn("CodOficina"),
                new DataColumn("NombreOficina"),
                new DataColumn("TipoDispositivo"),
                new DataColumn("ModeloDispositivo"),
                new DataColumn("NumSerie"),
                new DataColumn("Tecnico"),
                new DataColumn("Mensaje")
            });

            foreach (var incidencia in incidencias)
            {
                dataTable.Rows.Add(
                    incidencia.Id,
                    incidencia.FechaHora,
                    incidencia.Usuario,
                    incidencia.CodOficina,
                    incidencia.NombreOficina,
                    incidencia.TipoDispositivo,
                    incidencia.ModeloDispositivo,
                    incidencia.NumSerie,
                    incidencia.Tecnico,
                    incidencia.Mensaje
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }

        private FileResult GenerarExcelSinDescripcion(string nombreArchivo, IEnumerable<IncidenciaExportar> incidencias)
        {
            DataTable dataTable = new DataTable("Incidencias");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("FechaHora"),
                new DataColumn("Usuario"),
                new DataColumn("CodOficina"),
                new DataColumn("NombreOficina"),
                new DataColumn("TipoDispositivo"),
                new DataColumn("ModeloDispositivo"),
                new DataColumn("NumSerie"),
                new DataColumn("Tecnico")
            });

            foreach (var incidencia in incidencias)
            {
                dataTable.Rows.Add(
                    incidencia.Id,
                    incidencia.FechaHora,
                    incidencia.Usuario,
                    incidencia.CodOficina,
                    incidencia.NombreOficina,
                    incidencia.TipoDispositivo,
                    incidencia.ModeloDispositivo,
                    incidencia.NumSerie,
                    incidencia.Tecnico
                    );
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }
            }
        }

    }
}