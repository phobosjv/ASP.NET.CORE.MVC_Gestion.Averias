
var builder = WebApplication.CreateBuilder(args);

// A�adimos un filtro global para que un usuario no autenticado no pueda navegar por la, excepto por las p�ginas autorizadas con AllowAnonymous
var politicaUsuariosAutenticados = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();


// Add services to the container.
builder.Services.AddControllersWithViews(opciones =>
{
    opciones.Filters.Add(new AuthorizeFilter(politicaUsuariosAutenticados)); // Le pasamos la pol�tica creada m�s arriba.
});

// A�adir Servicio DBContext con SQLite. El connectionString debe estar definido en appsettings.json (Final o Development)
builder.Services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlite("name=AbsConnection"));

// A�adir Servicio para que el usuario pueda hacer loggin.
// A�adimos tambi�n el modo de autenticaci�n de microsoft
// Necesitamos Paquete nuget Microsoft.AspNetCore.Authentication.MicrosoftAccount, y luego click
// derecho sobre proyecto, "administrar secretos de usuario". All� crearemos las dos claves usadas a continuaci�n.
builder.Services.AddAuthentication().AddMicrosoftAccount(opciones =>
{
    opciones.ClientId = builder.Configuration["MicrosoftClientId"];
    opciones.ClientSecret = builder.Configuration["MicrosoftSecretId"];
});

// A�ade el Servicio Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
{
    opciones.SignIn.RequireConfirmedAccount = true;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opciones =>
{
    opciones.TokenLifespan = TimeSpan.FromMinutes(15);
}
);

// A�ade el Servicio Correo
builder.Services.AddTransient<IServicioCorreo, ServicioCorreo>();

// A�ade el Servicio Usuarios para obtener la Id
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuarios>();

// Indicar a Identity que vamos a utilizar Vistas Personalizadas para el Registro, Login, etc.
builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, opciones =>
{
    opciones.LoginPath = "/usuarios/login";
    opciones.AccessDeniedPath = "/usuarios/login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // A�adido al agregar Identity
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
