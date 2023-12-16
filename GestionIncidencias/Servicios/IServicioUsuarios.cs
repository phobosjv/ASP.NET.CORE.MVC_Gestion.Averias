
using System.Security.Claims;

namespace GestionIncidencias.Servicios
{
    public interface IServicioUsuarios
    {
        string ObtenerUsuarioId();

        public class ServicioUsuarios : IServicioUsuarios
        {
            private readonly HttpContext httpContext;

            public ServicioUsuarios(IHttpContextAccessor httpContextAccessor) 
            {
                httpContext = httpContextAccessor.HttpContext;
            }
            public string ObtenerUsuarioId()
            {
                Claim idClaim = null;
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    idClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                    return idClaim.Value;
                }
                else
                {
                    throw new ApplicationException("El usuario no está autenticado");
                }
            }
        }
    }
}
