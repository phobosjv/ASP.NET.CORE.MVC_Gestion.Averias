
using GestionIncidencias.Models;

namespace GestionIncidencias.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly IServicioCorreo servicioCorreo;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context, IServicioCorreo servicioCorreo)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.servicioCorreo = servicioCorreo;
        }

        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        // Post para realizar la acción de registro de usuario
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(RegistroViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var usuario = new IdentityUser() { Email = modelo.Email, UserName = modelo.Usuario };

            var resultado = await userManager.CreateAsync(usuario, password: modelo.Password);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Confirmar", "Usuarios");
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(modelo);
            }

        }

        [AllowAnonymous]
        public IActionResult Login(string mensaje = null)
        {
            if (mensaje is not null) // se pasará un string a mensaje si se ha producido un error.
            {
                ViewData["mensaje"] = mensaje;
            }
            return View();
        }

        // Post de Incio de Sesión de Usuario
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            var resultado = await signInManager.PasswordSignInAsync(modelo.Usuario,
                modelo.Password, modelo.Recuerdame, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre de usuario o password incorrecto");
                return View(modelo);
            }
        }

        // Post para cerrar la sesión de Usuario
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Get de inicio de sesión con cuenta Microsoft
        [AllowAnonymous]
        [HttpGet]
        public ChallengeResult LoginExterno(string proveedor, string urlRetorno = null)
        {
            var urlRedireccion = Url.Action("RegistrarUsuarioExterno", values: new { urlRetorno });
            var propiedades = signInManager.ConfigureExternalAuthenticationProperties(proveedor, urlRedireccion);
            return new ChallengeResult(proveedor, propiedades);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegistrarUsuarioExterno(string urlRetorno = null, string remoteError = null)
        {
            urlRetorno = urlRetorno ?? Url.Content("~/"); // Sobre urlRetorno se asigna su propio valor, o al root de la aplicación si el valor que tiene es nulo.

            var mensaje = ""; // Para mostrar el mensaje de error al usuario

            // Asignamos el error del servidor recogido a la variable mensaje
            if (remoteError is not null)
            {
                mensaje = $"Error del proveedor externo: {remoteError}";
                return RedirectToAction("login", routeValues: new { mensaje }); // Nos reenvía a Login, pero con un mensaje de error.
            }

            var info = await signInManager.GetExternalLoginInfoAsync(); // Info recoge los datos del login satisfactorio

            // En este mensaje igualmente se reenvía al login pasando el mensaje de error.
            if (info is null)
            {
                mensaje = "Error cargando los datos del login externo";
                return RedirectToAction("login", routeValues: new { mensaje }); // Nos reenvía a Login, pero con un mensaje de error.
            }

            var resultadoLoginExterno = await signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            // La cuenta ya existe. Si fué un exito, se redirige a la urlRetorno
            if (resultadoLoginExterno.Succeeded)
            {
                return LocalRedirect(urlRetorno);
            }


            // Microsft ha certificado su usuario, a partir de aquí pasamos a registrarlo en la BBDD
            string email = "";

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                email = info.Principal.FindFirstValue(ClaimTypes.Email);
            }
            else
            {
                mensaje = "Error leyendo el email del usuario del proveedor";
                return RedirectToAction("login", routeValues: new { mensaje }); // Nos reenvía a Login, pero con un mensaje de error.
            }

            // Registramos el email, y como usuario la parte izquierda a la @ del email
            var usuario = new IdentityUser { Email = email, UserName = email.Split("@").FirstOrDefault() };

            var resultadoCrearUsuario = await userManager.CreateAsync(usuario);

            // Error si no se ha podido crear el usuario en Identity
            if (!resultadoCrearUsuario.Succeeded)
            {
                mensaje = resultadoCrearUsuario.Errors.First().Description;
                return RedirectToAction("login", routeValues: new { mensaje });
            }

            var resultadoAgregarLogin = await userManager.AddLoginAsync(usuario, info);

            if (resultadoAgregarLogin.Succeeded)
            {
                return RedirectToAction("Confirmar", "Usuarios");
            }

            // Si después de todo se ha llegado aquí, es que todo ha fallado, por algún motivo no controlado.
            mensaje = "Ha ocurrido un error durante el login";
            return RedirectToAction("login", routeValues: new { mensaje }); // Nos reenvía a Login, pero con un mensaje de error.
        }

        // Muestra página avisando que el usuario se ha registrado, pero que un administrador tiene que activarlo.
        [AllowAnonymous]
        public IActionResult Confirmar()
        {
            return View();
        }

        // Acción de mostrar vista con listado de usuarios.
        [HttpGet]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Listado(string mensaje = null)
        {
            var usuarios = await context.Users.Select(u => new UsuarioViewModel
            {
                NombreUsuario = u.UserName,
                Email = u.Email,
                Activado = u.EmailConfirmed,
                Permisos = string.Join(",", userManager.GetRolesAsync(u).Result.ToArray()) // Para conseguir los permisos
            }).ToListAsync();

            var modelo = new UsuariosListadoViewModel();
            modelo.Usuarios = usuarios;
            modelo.Mensaje = mensaje;

            return View(modelo);

        }

        // Acción de Hacer Administrador
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> HacerAdmin(string nombreUsuario)
        {
            var usuario = await context.Users.Where(u => u.UserName == nombreUsuario).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await userManager.AddToRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { mensaje = "Se han asignado permisos de administrador a " + nombreUsuario });
        }

        // Acción de Quitar Administrador
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> QuitarAdmin(string nombreUsuario)
        {
            var usuario = await context.Users.Where(u => u.UserName == nombreUsuario).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await userManager.RemoveFromRoleAsync(usuario, Constantes.RolAdmin);

            return RedirectToAction("Listado", routeValues: new { mensaje = "Se han eliminado permisos de administrador de " + nombreUsuario });
        }

        // Acción de Activar Usuario
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Activar(string nombreUsuario)
        {
            var usuario = await context.Users.Where(u => u.UserName == nombreUsuario).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            usuario.EmailConfirmed = true;

            await context.SaveChangesAsync();

            return RedirectToAction("Listado", routeValues: new { mensaje = "Se ha activado el usuario " + nombreUsuario });
        }

        // Acción de Desactivar Usuario
        [HttpPost]
        [Authorize(Roles = Constantes.RolAdmin)]
        public async Task<IActionResult> Desactivar(string nombreUsuario)
        {
            var usuario = await context.Users.Where(u => u.UserName == nombreUsuario).FirstOrDefaultAsync();

            if (usuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            usuario.EmailConfirmed = false;

            await context.SaveChangesAsync();

            return RedirectToAction("Listado", routeValues: new { mensaje = "Se ha desactivado el usuario " + nombreUsuario });
        }

        // Mostrar formulario de contraseña olvidada
        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordOlvidado()
        {
            return View();
        }

        // Post del formulario de contraseña olvidada
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordOlvidado(PasswordOlvidadoModel passwordOlvidadoModel)
        {
            if (!ModelState.IsValid)
                return View(passwordOlvidadoModel);

            var usuario = await userManager.FindByEmailAsync(passwordOlvidadoModel.Email);

            if (usuario is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
            var texto = "Ha sido solicitado un cambio de contraseña para la aplicación 'Gestión de Averías', haga click en el siguiente enlace para acceder al formulario de cambio de contraseña (válido duratne 15 minutos).\n";
            var respuesta = Url.Action("ReiniciarPassword", "Usuarios", new { token, email = usuario.Email }, Request.Scheme);

            servicioCorreo.EnviarCorreo(new MensajeCorreo(usuario.Email, "Aplicación Gestión Averías: Recuperación de Constraseña", texto + respuesta));

            return RedirectToAction("PasswordOlvidadoConfirmacion", "Usuarios");

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordOlvidadoConfirmacion()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ReiniciarPassword(string token, string email)
        {
            var modelo = new ReiniciarPasswordModel { Token = token, Email = email };
            return View(modelo);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ReiniciarPassword (ReiniciarPasswordModel reiniciarPasswordModel)
        {
            if (!ModelState.IsValid)
            {
                return View(reiniciarPasswordModel);
            }

            var usuario = await userManager.FindByEmailAsync(reiniciarPasswordModel.Email);
            if (usuario == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var resultado = await userManager.ResetPasswordAsync(usuario, reiniciarPasswordModel.Token, reiniciarPasswordModel.Password);
            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View();
            }

            return RedirectToAction("PasswordReseteadoConfirmacion", "Usuarios");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordReseteadoConfirmacion()
        {
            return View();
        }
    }
}
