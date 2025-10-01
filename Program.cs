using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";        // Página de login
        options.AccessDeniedPath = "/Account/AccessDenied"; // Mejor usar AccessDenied
    });

// --- AGREGAR AUTORIZACIÓN Y POLICY ---
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EliminarEmpleados", policy =>
        policy.RequireAssertion(context =>
        {
            // Solo administradores pueden eliminar empleados
            return context.User.IsInRole("Administrador");
        })
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
