
namespace GestionIncidencias.Servicios
{
    public interface IServicioCorreo
    {
        string EnviarCorreo(MensajeCorreo mensajeCorreo);

        public class MensajeCorreo
        {
            public string correoPara { get; set; }
            public string correoCopia { get; set; }
            public string correoOculto { get; set; }
            public string asunto { get; set; }
            public string mensaje { get; set; }
            public string firma { get; set; }
            public MensajeCorreo(string correoPara, string asunto, string mensaje)
            {
                this.correoPara = correoPara;
                this.asunto = asunto;
                this.mensaje = mensaje;
            }
            public MensajeCorreo(string correoPara, string asunto, string mensaje, string correoCopia)
            {
                this.correoPara = correoPara;
                this.asunto = asunto;
                this.mensaje = mensaje;
                this.correoCopia = correoCopia;
            }
        }
        public class ServicioCorreo : IServicioCorreo
        {
            private readonly IConfiguration configuracion;

            public ServicioCorreo(IConfiguration configuracion)
            {
                this.configuracion = configuracion;
            }

            public string EnviarCorreo(MensajeCorreo mensajeCorreo)
            {
                return EnviarCorreo(mensajeCorreo.correoPara, mensajeCorreo.correoCopia, mensajeCorreo.asunto, mensajeCorreo.mensaje);
            }
            private string EnviarCorreo(string correoPara, string correoCopia, string asunto, string mensaje)
            {
                var resultado = "Mensaje mandado correctamente."; ;
                var remitente = configuracion["Remitente"];
                var usuario = configuracion["Usuario"];
                var password = configuracion["Password"];
                var servidor = configuracion["ServidorCorreo"];
                var puerto = Convert.ToInt16(configuracion["Puerto"]);

                SmtpClient client = new SmtpClient(servidor, puerto);

                client.EnableSsl = Convert.ToBoolean(configuracion["SSL"]);
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(usuario, password);

                MailMessage message = new MailMessage(remitente, correoPara, asunto, mensaje);

                if (correoCopia != null)
                {
                    if (correoCopia.Length > 0)
                    {
                        try
                        {
                            message.CC.Add(correoCopia);
                        }
                        catch (Exception)
                        {
                            resultado = "AVISO: Fallos en las direcciones COPIA: " + correoCopia;
                        }
                    }
                }
                

                try
                {
                    client.Send(message);
                }
                catch (Exception)
                {
                    resultado = "ERROR: Fallo en el envío del CORREO";
                }

                return resultado;

            }
        }
    }
}
