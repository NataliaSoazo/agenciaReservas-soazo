using MySql.Data.MySqlClient;

namespace agenciaReservas_soazo.Models;

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
            string sql = $@"
                SELECT 
                    r.{nameof(Reserva.Id)},
                    r.{nameof(Reserva.Fecha)},
                    r.{nameof(Reserva.FechaDesde)},
                    r.{nameof(Reserva.FechaHasta)},
                    r.{nameof(Reserva.Monto)},
                    r.{nameof(Reserva.IdInquilino)},
                    r.{nameof(Reserva.IdInmueble)},
                    r.{nameof(Reserva.Anulado)}
                FROM reservas r
                ORDER BY r.{nameof(Reserva.Id)} ASC;";

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

                            IdInmueble = reader.GetInt32(nameof(Reserva.IdInmueble)),

                            Anulado = reader.GetBoolean(nameof(Reserva.Anulado))
                        });
                    }
                }
            }
        }

        return reservas;
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
                        {nameof(Reserva.Monto)},{nameof(Reserva.IdInquilino)},{nameof(Reserva.IdInmueble)},{nameof(Reserva.Anulado)}
                    )
                    VALUES
                    (
                        @{nameof(Reserva.Fecha)}, @{nameof(Reserva.FechaDesde)}, @{nameof(Reserva.FechaHasta)},
                        @{nameof(Reserva.Monto)},@{nameof(Reserva.IdInquilino)},@{nameof(Reserva.IdInmueble)},
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
                    r.{nameof(Reserva.Anulado)}
                FROM reservas r
                INNER JOIN Inquilinos p ON i.{nameof(Reserva.IdInquilino)} = p.{nameof(Inquilino.Id)}
                INNER JOIN Inmuebles m ON i.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}
                ORDER BY i.{nameof(Reserva.Id)} ASC;";
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
            string sql = $@"
                UPDATE reservas SET

                    {nameof(Reserva.Fecha)} =@{nameof(Reserva.Fecha)},

                    {nameof(Reserva.FechaDesde)} =@{nameof(Reserva.FechaDesde)},

                    {nameof(Reserva.FechaHasta)} =@{nameof(Reserva.FechaHasta)},

                    {nameof(Reserva.Monto)} =@{nameof(Reserva.Monto)},

                    {nameof(Reserva.IdInquilino)} = @{nameof(Reserva.IdInquilino)},

                    {nameof(Reserva.IdInmueble)} =@{nameof(Reserva.IdInmueble)}

                WHERE {nameof(Reserva.Id)} =@{nameof(Reserva.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue(
                    $"@{nameof(Reserva.Id)}",reserva.Id);

                command.Parameters.AddWithValue(
                    $"@{nameof(Reserva.Fecha)}",reserva.Fecha);

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

                int filasAfectadas = command.ExecuteNonQuery();

                connection.Close();

                return filasAfectadas;
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
        DateTime fechaDesde,DateTime fechaHasta)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"
                SELECT COUNT(*)
                FROM reservas
                WHERE {nameof(Reserva.IdInmueble)} = @idInmueble

                AND {nameof(Reserva.Anulado)} = false

                AND @fechaDesde < {nameof(Reserva.FechaHasta)}

                AND @fechaHasta > {nameof(Reserva.FechaDesde)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@idInmueble",idInmueble);

                command.Parameters.AddWithValue("@fechaDesde",fechaDesde);

                command.Parameters.AddWithValue("@fechaHasta",fechaHasta);

                connection.Open();

                int cantidad = Convert.ToInt32(
                    command.ExecuteScalar()
                );

                return cantidad == 0;
            }
        }
    }
}