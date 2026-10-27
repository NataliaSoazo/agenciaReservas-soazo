
using Microsoft.AspNetCore.Authentication.Cookies;
using agenciaReservas_soazo.Repositorios;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<RepositorioPropietario>();
builder.Services.AddScoped<RepositorioInquilino>();
builder.Services.AddScoped<RepositorioInmueble>();
builder.Services.AddScoped<RepositorioPago>();
builder.Services.AddScoped<RepositorioReserva>();
builder.Services.AddScoped<RepositorioUsoInmueble>();
builder.Services.AddScoped<RepositorioTipoInmueble>();
builder.Services.AddScoped<RepositorioUsuario>();
builder.Services.AddScoped<RepositorioRol>();
builder.Services.AddScoped<RepositorioImagenInmueble>();

// Add services to the container.
builder.Services.AddControllersWithViews();
// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // Redirigir para login
        options.LogoutPath = "/Home/Logout"; // Redirigir para logout
        options.AccessDeniedPath = "/Home/Index"; // Redirigir para acceso denegado
    });

// Autorización con política de administrador
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => 
        policy.RequireClaim("Rol", "ADMINISTRADOR"));
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
// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Loguin}/{id?}");

app.Run();
