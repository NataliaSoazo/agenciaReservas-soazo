using System.Data;
using MySql.Data.MySqlClient;
using agenciaReservas_soazo.Models;
namespace agenciaReservas_soazo.Repositorios;

public class RepositorioReserva
{
    readonly string ConnectionString = "Server=localhost;Database=agenciareservas;User=root;Password=;";

    public RepositorioReserva()
    {

    }


        public IList<Reserva> GetReservas()
    {
        var reservas = new List<Reserva>();

        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT i.{nameof(Reserva.Id)},{nameof(Reserva.Fecha)}, {nameof(Reserva.FechaDesde)}, {nameof(Reserva.FechaHasta)}, {nameof(Reserva.Monto)}, {nameof(Reserva.IdInquilino)}, {nameof(Reserva.Anulado)},
                      p.{nameof(Inquilino.Nombre)}, p.{nameof(Inquilino.Apellido)}, {nameof(Reserva.IdInmueble)}, m.{nameof(Inmueble.Direccion)}
                FROM Reservas i 
                
                INNER JOIN Inquilinos p ON i.{nameof(Reserva.IdInquilino)} = p.{nameof(Inquilino.Id)}
                INNER JOIN Inmuebles m ON i.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}
                WHERE i.{nameof(Reserva.Anulado)} = 1
                ORDER BY i.{nameof(Reserva.Id)} ASC;";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservas.Add(new Reserva
                        {
                            Id = reader.GetInt32(nameof(Reserva.Id)),

                            Fecha = reader.GetDateTime(nameof(Reserva.Fecha)),
                            FechaDesde = reader.GetDateTime(nameof(Reserva.FechaDesde)),
                            FechaHasta = reader.GetDateTime(nameof(Reserva.FechaHasta)),

                            Monto = reader.GetDouble(nameof(Reserva.Monto)),

                            IdInquilino = reader.GetInt32(nameof(Reserva.IdInquilino)),
                            Arrendatario = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),

                            },
                            IdInmueble = reader.GetInt32(nameof(Reserva.IdInmueble)),
                            DatoInmueble = new Inmueble
                            {

                                Direccion = reader.GetString(nameof(Inmueble.Direccion))

                            },

                            Anulado = reader.GetBoolean(nameof(Reserva.Anulado))
                        });
                    }
                }
            }
        }

        return reservas;
    }
    public IList<Reserva> GetReservasPaginadas(int pagina, int cantidadPorPagina)
{
    var reservas = new List<Reserva>();

    int offset = (pagina - 1) * cantidadPorPagina;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"SELECT i.{nameof(Reserva.Id)},{nameof(Reserva.Fecha)}, {nameof(Reserva.FechaDesde)}, {nameof(Reserva.FechaHasta)}, {nameof(Reserva.Monto)}, {nameof(Reserva.IdInquilino)}, {nameof(Reserva.Anulado)},
                      p.{nameof(Inquilino.Nombre)}, p.{nameof(Inquilino.Apellido)}, {nameof(Reserva.IdInmueble)}, m.{nameof(Inmueble.Direccion)}
                FROM Reservas i 
                
                INNER JOIN Inquilinos p ON i.{nameof(Reserva.IdInquilino)} = p.{nameof(Inquilino.Id)}
                INNER JOIN Inmuebles m ON i.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}
                WHERE i.{nameof(Reserva.Anulado)} = 0
                ORDER BY i.{nameof(Reserva.FechaDesde)} DESC
                LIMIT @cantidad OFFSET @offset;";

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
                    reservas.Add(new Reserva
                        {
                            Id = reader.GetInt32(nameof(Reserva.Id)),

                            Fecha = reader.GetDateTime(nameof(Reserva.Fecha)),
                            FechaDesde = reader.GetDateTime(nameof(Reserva.FechaDesde)),
                            FechaHasta = reader.GetDateTime(nameof(Reserva.FechaHasta)),

                            Monto = reader.GetDouble(nameof(Reserva.Monto)),

                            IdInquilino = reader.GetInt32(nameof(Reserva.IdInquilino)),
                            Arrendatario = new Inquilino
                            {
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido)),

                            },
                            IdInmueble = reader.GetInt32(nameof(Reserva.IdInmueble)),
                            DatoInmueble = new Inmueble
                            {

                                Direccion = reader.GetString(nameof(Inmueble.Direccion))

                            },

                            Anulado = reader.GetBoolean(nameof(Reserva.Anulado))
                        });
                }
            }
        }
    }

    return reservas;
}
public int GetCantidadReservas()
{    int cantidad = 0;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = "SELECT COUNT(*) FROM Reservas";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.CommandType = CommandType.Text;

            connection.Open();

            cantidad = Convert.ToInt32(command.ExecuteScalar());
        }
    }

    return cantidad;
}

    
    public int AltaReserva(Reserva reserva)
    {
        int id = 0;

        try
        {
            using (var connection = new MySqlConnection(ConnectionString))
            {
                string sql = $@"
                    INSERT INTO reservas
                    (
                        {nameof(Reserva.Fecha)},{nameof(Reserva.FechaDesde)},{nameof(Reserva.FechaHasta)},
                        {nameof(Reserva.Monto)},{nameof(Reserva.IdInquilino)},{nameof(Reserva.IdInmueble)},{nameof(Reserva.Anulado)}, {nameof(Reserva.IdAlta)}, {nameof(Pago.IdBaja)}
                    )
                    VALUES
                    (
                        @{nameof(Reserva.Fecha)}, @{nameof(Reserva.FechaDesde)}, @{nameof(Reserva.FechaHasta)},
                        @{nameof(Reserva.Monto)},@{nameof(Reserva.IdInquilino)},@{nameof(Reserva.IdInmueble)},@{nameof(Pago.IdAlta)}, @{nameof(Pago.IdBaja)}
                        false
                    );

                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(Reserva.Fecha)}",reserva.Fecha);

                    command.Parameters.AddWithValue(
                        $"@{nameof(Reserva.FechaDesde)}",reserva.FechaDesde);

                    command.Parameters.AddWithValue(
                        $"@{nameof(Reserva.FechaHasta)}",reserva.FechaHasta);

                    command.Parameters.AddWithValue(
                        $"@{nameof(Reserva.Monto)}",reserva.Monto);

                    command.Parameters.AddWithValue(
                        $"@{nameof(Reserva.IdInquilino)}",reserva.IdInquilino);

                    command.Parameters.AddWithValue(
                        $"@{nameof(Reserva.IdInmueble)}",reserva.IdInmueble);

                    connection.Open();

                    id = Convert.ToInt32(command.ExecuteScalar());

                    reserva.Id = id;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Error al insertar reserva: {ex.Message}"
            );

            throw;
        }

        return id;
    }


    public Reserva? GetReserva(int id)
{
    Reserva? reserva = null;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"
            SELECT 
                r.{nameof(Reserva.Id)},
                r.{nameof(Reserva.Fecha)},
                r.{nameof(Reserva.FechaDesde)},
                r.{nameof(Reserva.FechaHasta)},
                r.{nameof(Reserva.Monto)},
                r.{nameof(Reserva.IdInquilino)},
                r.{nameof(Reserva.IdInmueble)},
                r.{nameof(Reserva.IdAlta)},
                r.{nameof(Reserva.IdBaja)},
                r.{nameof(Reserva.Anulado)},
                p.{nameof(Inquilino.Nombre)},
                p.{nameof(Inquilino.Apellido)},
                m.{nameof(Inmueble.Direccion)},
                m.{nameof(Inmueble.Precio)},
                m.{nameof(Inmueble.Porcentual)}
            FROM reservas r
            INNER JOIN Inquilinos p 
                ON r.{nameof(Reserva.IdInquilino)} = p.{nameof(Inquilino.Id)}
            INNER JOIN Inmuebles m 
                ON r.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}
            WHERE r.{nameof(Reserva.Id)} = @id;";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id", id);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    reserva = new Reserva
                    {
                        Id = reader.GetInt32(nameof(Reserva.Id)),
                        Fecha = reader.GetDateTime(nameof(Reserva.Fecha)),
                        FechaDesde = reader.GetDateTime(nameof(Reserva.FechaDesde)),
                        FechaHasta = reader.GetDateTime(nameof(Reserva.FechaHasta)),
                        Monto = reader.GetDouble(nameof(Reserva.Monto)),
                        IdInquilino = reader.GetInt32(nameof(Reserva.IdInquilino)),
                        IdInmueble = reader.GetInt32(nameof(Reserva.IdInmueble)),
                        Anulado = reader.GetBoolean(nameof(Reserva.Anulado)),
                        IdAlta = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdAlta))),
                        IdBaja = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdBaja))),
                        Arrendatario = new Inquilino
                        {
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido))
                        },

                        DatoInmueble = new Inmueble
                        {
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Porcentual= reader.GetInt32(nameof(Inmueble.Porcentual))
                        }
                    };
                }
            }
        }
    }

    return reserva;
}

        public int ModificarReserva(Reserva reserva)
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = @"
            UPDATE reservas
            SET
                Fecha = @Fecha,
                FechaDesde = @FechaDesde,
                FechaHasta = @FechaHasta,
                Monto = @Monto,
                IdInquilino = @IdInquilino,
                IdInmueble = @IdInmueble
            WHERE Id = @Id;";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Id", reserva.Id);
            command.Parameters.AddWithValue("@Fecha", reserva.Fecha);
            command.Parameters.AddWithValue("@FechaDesde", reserva.FechaDesde);
            command.Parameters.AddWithValue("@FechaHasta", reserva.FechaHasta);
            command.Parameters.AddWithValue("@Monto", reserva.Monto);
            command.Parameters.AddWithValue("@IdInquilino", reserva.IdInquilino);
            command.Parameters.AddWithValue("@IdInmueble", reserva.IdInmueble);

            connection.Open();

            return command.ExecuteNonQuery();
        }
    }
}

    public int AnularReserva(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"
                UPDATE reservas
                SET {nameof(Reserva.Anulado)} = true
                WHERE {nameof(Reserva.Id)} = @id;";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                int filasAfectadas = command.ExecuteNonQuery();

                connection.Close();

                return filasAfectadas;
            }
        }
    }


    public bool ValidarReserva(Reserva reserva)
    {
        if (reserva.FechaDesde < reserva.FechaHasta)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   public bool InmuebleDisponible(
    int idInmueble,
    DateTime fechaDesde,
    DateTime fechaHasta,
    int idReserva = 0)
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = @"
            SELECT COUNT(*)
            FROM reservas
            WHERE IdInmueble = @idInmueble
              AND Anulado = false
              AND FechaDesde < @fechaHasta
              AND FechaHasta > @fechaDesde
              AND Id <> @idReserva;";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@idInmueble", idInmueble);
            command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
            command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
            command.Parameters.AddWithValue("@idReserva", idReserva);

            connection.Open();

            int cantidad = Convert.ToInt32(command.ExecuteScalar());

            return cantidad == 0;
        }
    }
}
}