namespace agenciaReservas_soazo.Repositorios
{
    //Conexión
    public abstract class RepositorioBase
    {
        protected readonly string connectionString;
        // comentario de prueba en rama joseBranch

        protected RepositorioBase(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró  la cadena de conexión 'DefaultConnection' en appsettings.json."
                );
        }
    }
}
