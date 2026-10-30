using System.Data;
using MySql.Data.MySqlClient;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
namespace agenciaReservas_soazo.Repositorios;

public class RepositorioInmueble:RepositorioBase
{
    //readonly string connectionString = "Server=localhost;Database=agenciareservas;User=root;Password=;";
    private readonly RepositorioReserva _repoReserva;

        public RepositorioInmueble(
            IConfiguration configuration,
            RepositorioReserva repoReserva
        ) : base(configuration)
        {
            _repoReserva = repoReserva;
        }
    public IList<Inmueble> ObtenerTodos()
    {
        var inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"SELECT i.{nameof(Inmueble.Id)},{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Cupo)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.Disponible)}, {nameof(Inmueble.Latitud)},{nameof(Inmueble.Longitud)},{nameof(Inmueble.PropietarioId)},
					 p.{nameof(Propietario.Nombre)}, p.{nameof(Propietario.Apellido)}
					FROM Inmuebles i INNER JOIN Propietarios p ON  {nameof(Inmueble.PropietarioId)} = p. {nameof(Propietario.Id)} ORDER BY disponible DESC LIMIT 20 OFFSET 0";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                        }
                    });
                }
                connection.Close();
            }
        }
        return inmuebles;
    }
    public IList<Inmueble> GetInmueblesPaginados(int pagina, int cantidadPorPagina)
{
    var inmuebles = new List<Inmueble>();

    int offset = (pagina - 1) * cantidadPorPagina;

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = @$"
            SELECT 
                i.{nameof(Inmueble.Id)},
                i.{nameof(Inmueble.Direccion)},
                i.{nameof(Inmueble.Tipo)},
                i.{nameof(Inmueble.Uso)},
                i.{nameof(Inmueble.Cupo)},
                i.{nameof(Inmueble.Precio)},
                i.{nameof(Inmueble.Porcentual)},
                i.{nameof(Inmueble.Disponible)},
                i.{nameof(Inmueble.Latitud)},
                i.{nameof(Inmueble.Longitud)},
                i.{nameof(Inmueble.PropietarioId)},
                p.{nameof(Propietario.Id)},
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}
            FROM Inmuebles i
            INNER JOIN Propietarios p 
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}
            ORDER BY i.{nameof(Inmueble.Disponible)} DESC
            LIMIT @cantidad OFFSET @offset";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@cantidad", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Porcentual = reader.GetInt32(nameof(Inmueble.Porcentual)),
                        Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),

                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        }
                    });
                }
            }
        }
    }

    return inmuebles;
}
    public int GetCantidadInmuebles()
{
    int cantidad = 0;

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = "SELECT COUNT(*) FROM Inmuebles";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            connection.Open();

            cantidad = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    return cantidad;
}

    public int AltaInmueble(Inmueble inmueble)
    {
        int id = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO inmuebles ({nameof(Inmueble.Direccion)}, {nameof(Inmueble.Cupo)}, {nameof(Inmueble.Uso)},  {nameof(Inmueble.Precio)},{nameof(Inmueble.Porcentual)}, {nameof(Inmueble.Disponible)}, {nameof(Inmueble.PropietarioId)}, {nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Tipo)})
                                     VALUES (@{nameof(Inmueble.Direccion)}, @{nameof(Inmueble.Cupo)}, @{nameof(Inmueble.Uso)},  @{nameof(Inmueble.Precio)},@{nameof(Inmueble.Porcentual)}, @{nameof(Inmueble.Disponible)}, @{nameof(Inmueble.PropietarioId)}, @{nameof(Inmueble.Latitud)}, @{nameof(Inmueble.Longitud)}, @{nameof(Inmueble.Tipo)});            
             SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Tipo)}", inmueble.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Cupo)}", inmueble.Cupo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Porcentual)}", inmueble.Porcentual);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Disponible)}", inmueble.Disponible);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", inmueble.PropietarioId);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                inmueble.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Inmueble? GetInmueble(int id)
    {
        Inmueble? inmueble = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"SELECT i.{nameof(Inmueble.Id)},{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Cupo)},{nameof(Inmueble.Precio)},{nameof(Inmueble.Porcentual)}, {nameof(Inmueble.Porcentual)},{nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Disponible)},{nameof(Inmueble.PropietarioId)},
					 p.{nameof(Propietario.Nombre)}, p.{nameof(Propietario.Apellido)}
					FROM Inmuebles i INNER JOIN Propietarios p ON  {nameof(Inmueble.PropietarioId)} = p. {nameof(Propietario.Id)} 
               WHERE i.{nameof(Inmueble.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Porcentual = reader.GetInt32(nameof(Inmueble.Porcentual)),
                            Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(nameof(Propietario.Id)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            }
                        };
                    }
                }
            }
        }
        return inmueble;
    }

    public int ModificarInmueble(Inmueble inmueble)
    {
        int id = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"UPDATE inmuebles SET 
				{nameof(Inmueble.Direccion)} = @{nameof(Inmueble.Direccion)}, 
                {nameof(Inmueble.Tipo)} = @{nameof(Inmueble.Tipo)},
                {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                {nameof(Inmueble.Cupo)} = @{nameof(Inmueble.Cupo)},
                {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)},
                {nameof(Inmueble.Porcentual)} = @{nameof(Inmueble.Porcentual)},
                {nameof(Inmueble.Latitud)} = @{nameof(Inmueble.Latitud)},
                {nameof(Inmueble.Longitud)} = @{nameof(Inmueble.Longitud)},
                {nameof(Inmueble.Disponible)} = @{nameof(Inmueble.Disponible)},
                {nameof(Inmueble.PropietarioId)} = @{nameof(Inmueble.PropietarioId)}
			    WHERE {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id)}", inmueble.Id);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Tipo)}", inmueble.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Cupo)}", inmueble.Cupo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Porcentual)}", inmueble.Porcentual);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Disponible)}", inmueble.Disponible);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", inmueble.PropietarioId);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                inmueble.Id = id;
                connection.Close();
            }
        }
        return 0;
    }

    public int EliminarInmueble(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"DELETE from inmuebles WHERE {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }
    //obtiene los tipos de inmueble
    public IList<TipoInmueble> ObtenerTipos()
    {
        var tipos = new List<TipoInmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"SELECT {nameof(TipoInmueble.Tipo)}
					FROM tipoinmuebles DESC ";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipos.Add(new TipoInmueble
                    {

                        Tipo = reader.GetString(nameof(TipoInmueble.Tipo)),


                    });
                }
                connection.Close();
            }
        }
        return tipos;
    }
    //Busca los inmuebles que le corresponden a un propietario
    public IList<Inmueble> ObtenerPorPropietario(int propietarioId)
{
    var res = new List<Inmueble>();

    using (var connection = new MySqlConnection(connectionString))
    {
        var sql = @$"
            SELECT
                i.{nameof(Inmueble.Id)},
                i.{nameof(Inmueble.Direccion)},
                i.{nameof(Inmueble.Uso)},
                i.{nameof(Inmueble.Disponible)},
                i.{nameof(Inmueble.PropietarioId)},
                p.{nameof(Propietario.Id)} AS PropietarioIdResultado,
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}

            FROM Inmuebles i

            INNER JOIN Propietarios p
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}

            WHERE i.{nameof(Inmueble.PropietarioId)} = @propietarioId";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            command.Parameters.Add(
                new MySqlParameter("@propietarioId", MySqlDbType.Int32)
                {
                    Value = propietarioId
                });

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var inmueble = new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),

                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32("PropietarioIdResultado"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        }
                    };

                    res.Add(inmueble);
                }
            }
        }
    }

    return res;
}
    
    // obtiene los inmuebles cuyos propietarios habilitan su comercialización 
   public IList<Inmueble> ObtenerDisponibles()
{
    var inmuebles = new List<Inmueble>();

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = $@"
            SELECT
                i.{nameof(Inmueble.Id)},
                i.{nameof(Inmueble.Direccion)},            
                i.{nameof(Inmueble.Precio)},
                i.{nameof(Inmueble.PropietarioId)},
                p.{nameof(Propietario.Id)},
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}
            FROM Inmuebles i
            INNER JOIN Propietarios p
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}
            WHERE i.Disponible = 'SI'
            ORDER BY i.Id DESC";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@Disponible", "SI");
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        }
                    });
                }
            }
        }
    }

    return inmuebles;
}

    /*Solamente inmuebles con Disponible = "SI".
    Excluye reservas anuladas.
    Detecta reservas que se superponen con el período solicitado.
    Devuelve únicamente inmuebles que no tienen reservas que se superpongan.*/
    public List<Inmueble> obtenerDisponiblesPorFecha(DateTime fechaInicio, DateTime fechaFin)
    {
        var inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @$"
            SELECT
                i.{nameof(Inmueble.Id)},
                i.{nameof(Inmueble.Direccion)},
                i.{nameof(Inmueble.Tipo)},
                i.{nameof(Inmueble.Uso)},
                i.{nameof(Inmueble.Cupo)},
                i.{nameof(Inmueble.Precio)},
                i.{nameof(Inmueble.Disponible)},
                i.{nameof(Inmueble.Latitud)},
                i.{nameof(Inmueble.Longitud)},
                i.{nameof(Inmueble.PropietarioId)},
                p.{nameof(Propietario.Id)} AS PropietarioIdResultado,
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}

            FROM Inmuebles i

            INNER JOIN Propietarios p
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}

            WHERE
                i.{nameof(Inmueble.Disponible)} = 'SI'

                AND NOT EXISTS (
                    SELECT 1
                    FROM Reservas r
                    WHERE
                        r.{nameof(Reserva.IdInmueble)} = i.{nameof(Inmueble.Id)}
                        AND r.{nameof(Reserva.Anulado)} = 0
                        AND @FechaInicio < DATE(r.{nameof(Reserva.FechaHasta)})
                        AND @FechaFin > DATE(r.{nameof(Reserva.FechaDesde)})
                )

            ORDER BY i.{nameof(Inmueble.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;

                command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                command.Parameters.AddWithValue("@FechaFin", fechaFin.Date);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                            Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),

                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32("PropietarioIdResultado"),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido))
                            }
                        });
                    }
                }
            }
        }

        return inmuebles;
    }


    //obtiene los inmuebles mas reservados en los ultimos 365 dias, de forma  desc
public IList<Inmueble> MasReservados365(DateTime fechaDesde, DateTime fechaHasta)
{
    var inmuebles = new List<Inmueble>();

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = @$"
            SELECT
                i.{nameof(Inmueble.Id)},
                i.{nameof(Inmueble.Direccion)},
                i.{nameof(Inmueble.Tipo)},
                i.{nameof(Inmueble.Uso)},
                i.{nameof(Inmueble.Cupo)},
                i.{nameof(Inmueble.Precio)},
                i.{nameof(Inmueble.Disponible)},
                i.{nameof(Inmueble.Latitud)},
                i.{nameof(Inmueble.Longitud)},
                i.{nameof(Inmueble.PropietarioId)},

                p.{nameof(Propietario.Id)} AS PropietarioIdResultado,
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)},

                COUNT(r.{nameof(Reserva.IdInmueble)}) AS CantidadReservas

            FROM Inmuebles i

            INNER JOIN Propietarios p
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}

            INNER JOIN Reservas r
                ON i.{nameof(Inmueble.Id)} = r.{nameof(Reserva.IdInmueble)}

            WHERE
                r.{nameof(Reserva.Anulado)} = 0
                AND r.{nameof(Reserva.FechaDesde)} >= @FechaDesde
                AND r.{nameof(Reserva.FechaDesde)} <= @FechaHasta

            GROUP BY
                i.{nameof(Inmueble.Id)},
                p.{nameof(Propietario.Id)},
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}

            ORDER BY
                CantidadReservas DESC

            LIMIT 20 OFFSET 0;";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@FechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@FechaHasta", fechaHasta);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),

                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32("PropietarioIdResultado"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        }
                    });
                }
            }
        }
    }
    return inmuebles;
}

    public IList<Inmueble> SinReservasXdias(DateTime fechaDesde, DateTime fechaHasta)
{
    var inmuebles = new List<Inmueble>();

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = $@"
            SELECT
                i.*,
                p.{nameof(Propietario.Id)} AS PropietarioIdResultado,
                p.{nameof(Propietario.Nombre)},
                p.{nameof(Propietario.Apellido)}
            FROM Inmuebles i

            LEFT JOIN Propietarios p
                ON i.{nameof(Inmueble.PropietarioId)} = p.{nameof(Propietario.Id)}

            WHERE NOT EXISTS (
                SELECT 1
                FROM Reservas r
                WHERE
                    r.{nameof(Reserva.IdInmueble)} = i.{nameof(Inmueble.Id)}
                    AND r.{nameof(Reserva.Anulado)} = 0
                    AND r.{nameof(Reserva.FechaDesde)} >= @FechaDesde
                    AND r.{nameof(Reserva.FechaDesde)} <= @FechaHasta
            )

            ORDER BY i.{nameof(Inmueble.Id)};";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            command.Parameters.AddWithValue("@FechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@FechaHasta", fechaHasta);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    inmuebles.Add(new Inmueble
                    {
                        Id = reader.GetInt32(nameof(Inmueble.Id)),
                        Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                        Tipo = reader.GetString(nameof(Inmueble.Tipo)),
                        Uso = reader.GetString(nameof(Inmueble.Uso)),
                        Cupo = reader.GetInt32(nameof(Inmueble.Cupo)),
                        Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                        Disponible = reader.GetString(nameof(Inmueble.Disponible)),
                        Latitud = reader.GetString(nameof(Inmueble.Latitud)),
                        Longitud = reader.GetString(nameof(Inmueble.Longitud)),
                        PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),

                        Duenio = new Propietario
                        {
                            Id = reader.GetInt32("PropietarioIdResultado"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido))
                        }
                    });
                }
            }
        }
    }

    return inmuebles;
}
}