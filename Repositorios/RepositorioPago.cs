using System.Collections.Specialized;
using agenciaReservas_soazo.Models;
using MySql.Data.MySqlClient;

namespace agenciaReservas_soazo.Repositorios;

public class RepositorioPago
{
    readonly string ConnectionString = "Server=localhost;Database=agenciaReservas;User=root;Password=;";

    public RepositorioPago()
    {

    }

    public IList<Pago> GetPagos1()
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)}, {nameof(Pago.Fecha)},{nameof(Pago.Modo)}, {nameof(Pago.Concepto)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdReserva)}
                FROM pagos p";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Modo = reader.GetString(nameof(Pago.Modo)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Anulado = reader.GetBoolean(nameof(Pago.Anulado)),
                            IdReserva = reader.GetInt32(nameof(Pago.IdReserva)),
                        });
                    }

                }
            }
        }
        return pagos;
    }
    public IList<Pago> GetPagos()
{
    var pagos = new List<Pago>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"
            SELECT
                p.{nameof(Pago.Id)} AS PagoId,
                p.{nameof(Pago.Fecha)},
                p.{nameof(Pago.IdReserva)},
                p.{nameof(Pago.Modo)},
                p.{nameof(Pago.Concepto)},
                p.{nameof(Pago.Importe)},
                p.{nameof(Pago.Anulado)},
                p.{nameof(Pago.IdAlta)},
                p.{nameof(Pago.IdBaja)},

                r.{nameof(Reserva.Id)} AS ReservaId,
                r.{nameof(Reserva.Fecha)} AS ReservaFecha,
                r.{nameof(Reserva.FechaDesde)},
                r.{nameof(Reserva.FechaHasta)},
                r.{nameof(Reserva.Monto)},
                r.{nameof(Reserva.IdInquilino)},
                r.{nameof(Reserva.IdInmueble)},
                r.{nameof(Reserva.Anulado)} AS ReservaAnulado,

                i.{nameof(Inquilino.Id)} AS InquilinoId,
                i.{nameof(Inquilino.Nombre)},
                i.{nameof(Inquilino.Apellido)},

                m.{nameof(Inmueble.Id)} AS InmuebleId,
                m.{nameof(Inmueble.Direccion)}

            FROM Pagos p

            INNER JOIN Reservas r
                ON p.{nameof(Pago.IdReserva)} = r.{nameof(Reserva.Id)}

            INNER JOIN Inquilinos i
                ON r.{nameof(Reserva.IdInquilino)} = i.{nameof(Inquilino.Id)}

            INNER JOIN Inmuebles m
                ON r.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}

            ORDER BY p.{nameof(Pago.Id)} ASC;
        ";

        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32("PagoId"),

                        Fecha = reader.GetDateTime(nameof(Pago.Fecha)),

                        IdReserva = reader.GetInt32(nameof(Pago.IdReserva)),

                        Modo = reader.GetString(nameof(Pago.Modo)),

                        Concepto = reader.GetString(nameof(Pago.Concepto)),

                        Importe = reader.GetDecimal(nameof(Pago.Importe)),

                        Anulado = reader.GetBoolean(nameof(Pago.Anulado)),

                        IdAlta = reader.GetInt32(nameof(Pago.IdAlta)),

                        IdBaja = reader.IsDBNull(
                            reader.GetOrdinal(nameof(Pago.IdBaja)))
                            ? null
                            : reader.GetInt32(nameof(Pago.IdBaja)),

                        DatosReserva = new Reserva
                        {
                            Id = reader.GetInt32("ReservaId"),

                            Fecha = reader.GetDateTime("ReservaFecha"),

                            FechaDesde = reader.GetDateTime(
                                nameof(Reserva.FechaDesde)),

                            FechaHasta = reader.GetDateTime(
                                nameof(Reserva.FechaHasta)),

                            Monto = reader.GetDouble(
                                nameof(Reserva.Monto)),

                            IdInquilino = reader.GetInt32(
                                nameof(Reserva.IdInquilino)),

                            IdInmueble = reader.GetInt32(
                                nameof(Reserva.IdInmueble)),

                            Anulado = reader.GetBoolean(
                                "ReservaAnulado"),

                            Arrendatario = new Inquilino
                            {
                                Id = reader.GetInt32("InquilinoId"),

                                Nombre = reader.GetString(
                                    nameof(Inquilino.Nombre)),

                                Apellido = reader.GetString(
                                    nameof(Inquilino.Apellido))
                            },

                            DatoInmueble = new Inmueble
                            {
                                Id = reader.GetInt32("InmuebleId"),

                                Direccion = reader.GetString(
                                    nameof(Inmueble.Direccion))
                            }
                        }
                    });
                }
            }
        }
    }

    return pagos;
}
    public IList<Pago> ObtenerPagosPorReserva(int id)
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)},  {nameof(Pago.Fecha)}, {nameof(Pago.Modo)},{nameof(Pago.Concepto)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdReserva)}
                FROM pagos p
                 WHERE p.{nameof(Pago.IdReserva)} = @{nameof(Pago.IdReserva)};";
            using (var command = new MySqlCommand(sql, connection))
            {   command.Parameters.AddWithValue($"@{nameof(Pago.IdReserva)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            Fecha = reader.GetDateTime(nameof(Pago.Fecha)),
                            Modo = reader.GetString(nameof(Pago.Modo)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Anulado = reader.GetBoolean(nameof(Pago.Anulado)),
                            IdReserva = reader.GetInt32(nameof(Pago.IdReserva)),
                        });
                    }

                }
            }
        }
        return pagos;
    }

    public int AltaPago(Pago pago)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"INSERT INTO pagos ({nameof(Pago.Fecha)},{nameof(Pago.Modo)}, {nameof(Pago.Concepto)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdReserva)}, {nameof(Pago.IdAlta)}, {nameof(Pago.IdBaja)}) 
                        VALUES (@{nameof(Pago.Fecha)},@{nameof(Pago.Modo)}, @{nameof(Pago.Concepto)}, @{nameof(Pago.Importe)}, @{nameof(Pago.Anulado)}, @{nameof(Pago.IdReserva)}, @{nameof(Pago.IdAlta)}, @{nameof(Pago.IdBaja)});           
                        SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Modo)}", pago.Modo);
                command.Parameters.AddWithValue($"@{nameof(Pago.Concepto)}", pago.Concepto);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Anulado)}", false);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdReserva)}", pago.IdReserva);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdAlta)}", pago.IdAlta);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdBaja)}", pago.IdBaja);

                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                pago.Id = id;
                connection.Close();
            }
        }
        return id;
    }

    public Pago? GetPago(int id)
    {
        Pago? pagos = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"SELECT p.{nameof(Pago.Id)},{nameof(Pago.Fecha)},{nameof(Pago.Modo)}, {nameof(Pago.Concepto)}, {nameof(Pago.Importe)}, {nameof(Pago.Anulado)}, {nameof(Pago.IdReserva)}, {nameof(Pago.IdAlta)}, {nameof(Pago.IdBaja)}
                FROM pagos p
                WHERE p.{nameof(Pago.Id)} = @{nameof(Pago.Id)};";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pagos = new Pago
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Pago.Id))),
                            Fecha = reader.GetDateTime(reader.GetOrdinal(nameof(Pago.Fecha))),
                            Modo = reader.GetString(reader.GetOrdinal(nameof(Pago.Modo))),
                            Concepto = reader.GetString(reader.GetOrdinal(nameof(Pago.Concepto))),
                            Importe = reader.GetDecimal(reader.GetOrdinal(nameof(Pago.Importe))),
                            Anulado = reader.GetBoolean(reader.GetOrdinal(nameof(Pago.Anulado))),
                            IdReserva = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdReserva))),
                            IdAlta = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdAlta))),
                            IdBaja = reader.GetInt32(reader.GetOrdinal(nameof(Pago.IdBaja))) 
                        };
                    }
                }
            }
        }
        return pagos;
    }
    

    public int ModificarPago(Pago pago)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = $@"UPDATE pagos SET 
                            {nameof(Pago.Fecha)} = @{nameof(Pago.Fecha)},
                            {nameof(Pago.Modo)}= @{nameof(Pago.Modo)},
                            {nameof(Pago.Concepto)} = @{nameof(Pago.Concepto)},
                            {nameof(Pago.Importe)} = @{nameof(Pago.Importe)},
                            {nameof(Pago.IdReserva)} = @{nameof(Pago.IdReserva)}
                        WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Concepto)}", pago.Concepto);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.IdReserva)}", pago.IdReserva);
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", pago.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }


    public int EliminarPago(int id, int IdBaja) //Es un anulado lógico
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE pagos SET {nameof(Pago.Anulado)} = 'SI', {nameof(Pago.IdBaja)} = @IdBaja WHERE {nameof(Pago.Id)} = @id;";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id)}", id);
                command.Parameters.AddWithValue($"@IdBaja", IdBaja); 
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }
public IList<Pago> GetPagosPaginados(int pagina, int cantidadPorPagina)
{
    var pagos = new List<Pago>();

    int offset = (pagina - 1) * cantidadPorPagina;

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"
            SELECT
                p.{nameof(Pago.Id)} AS PagoId,
                p.{nameof(Pago.Fecha)},
                p.{nameof(Pago.IdReserva)},
                p.{nameof(Pago.Modo)},
                p.{nameof(Pago.Concepto)},
                p.{nameof(Pago.Importe)},
                p.{nameof(Pago.Anulado)},
                p.{nameof(Pago.IdAlta)},
                p.{nameof(Pago.IdBaja)},

                r.{nameof(Reserva.Id)} AS ReservaId,
                r.{nameof(Reserva.Fecha)} AS ReservaFecha,
                r.{nameof(Reserva.FechaDesde)},
                r.{nameof(Reserva.FechaHasta)},
                r.{nameof(Reserva.Monto)},
                r.{nameof(Reserva.IdInquilino)},
                r.{nameof(Reserva.IdInmueble)},
                r.{nameof(Reserva.Anulado)} AS ReservaAnulado,

                i.{nameof(Inquilino.Id)} AS InquilinoId,
                i.{nameof(Inquilino.Nombre)},
                i.{nameof(Inquilino.Apellido)},

                m.{nameof(Inmueble.Id)} AS InmuebleId,
                m.{nameof(Inmueble.Direccion)}

            FROM Pagos p

            INNER JOIN Reservas r
                ON p.{nameof(Pago.IdReserva)} = r.{nameof(Reserva.Id)}

            INNER JOIN Inquilinos i
                ON r.{nameof(Reserva.IdInquilino)} = i.{nameof(Inquilino.Id)}

            INNER JOIN Inmuebles m
                ON r.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}

            ORDER BY p.{nameof(Pago.Fecha)} Desc

            LIMIT @cantidadPorPagina OFFSET @offset;
        ";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@cantidadPorPagina", cantidadPorPagina);
            command.Parameters.AddWithValue("@offset", offset);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    pagos.Add(new Pago
                    {
                        Id = reader.GetInt32("PagoId"),

                        Fecha = reader.GetDateTime(
                            nameof(Pago.Fecha)),

                        IdReserva = reader.GetInt32(
                            nameof(Pago.IdReserva)),

                        Modo = reader.GetString(
                            nameof(Pago.Modo)),

                        Concepto = reader.GetString(
                            nameof(Pago.Concepto)),

                        Importe = reader.GetDecimal(
                            nameof(Pago.Importe)),

                        Anulado = reader.GetBoolean(
                            nameof(Pago.Anulado)),

                        IdAlta = reader.GetInt32(
                            nameof(Pago.IdAlta)),

                        IdBaja = reader.IsDBNull(
                            reader.GetOrdinal(nameof(Pago.IdBaja)))
                            ? null
                            : reader.GetInt32(nameof(Pago.IdBaja)),

                        DatosReserva = new Reserva
                        {
                            Id = reader.GetInt32("ReservaId"),

                            Fecha = reader.GetDateTime(
                                "ReservaFecha"),

                            FechaDesde = reader.GetDateTime(
                                nameof(Reserva.FechaDesde)),

                            FechaHasta = reader.GetDateTime(
                                nameof(Reserva.FechaHasta)),

                            Monto = reader.GetDouble(
                                nameof(Reserva.Monto)),

                            IdInquilino = reader.GetInt32(
                                nameof(Reserva.IdInquilino)),

                            IdInmueble = reader.GetInt32(
                                nameof(Reserva.IdInmueble)),

                            Anulado = reader.GetBoolean(
                                "ReservaAnulado"),

                            Arrendatario = new Inquilino
                            {
                                Id = reader.GetInt32(
                                    "InquilinoId"),

                                Nombre = reader.GetString(
                                    nameof(Inquilino.Nombre)),

                                Apellido = reader.GetString(
                                    nameof(Inquilino.Apellido))
                            },

                            DatoInmueble = new Inmueble
                            {
                                Id = reader.GetInt32(
                                    "InmuebleId"),

                                Direccion = reader.GetString(
                                    nameof(Inmueble.Direccion))
                            }
                        }
                    });
                }
            }
        }
    }

    return pagos;
}
public int CantidadPagos()
{
    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"
            SELECT COUNT(*)
            FROM Pagos;
        ";

        using (var command = new MySqlCommand(sql, connection))
        {
            connection.Open();

            return Convert.ToInt32(command.ExecuteScalar());
        }
    }
}

public IList<Reserva> BuscarReservasParaPago(string texto)
{
    var reservas = new List<Reserva>();

    using (var connection = new MySqlConnection(ConnectionString))
    {
        string sql = $@"
            SELECT
                r.{nameof(Reserva.Id)},
                r.{nameof(Reserva.IdInquilino)},
                r.{nameof(Reserva.IdInmueble)},

                i.{nameof(Inquilino.Nombre)},
                i.{nameof(Inquilino.Apellido)},

                m.{nameof(Inmueble.Direccion)}

            FROM Reservas r

            INNER JOIN Inquilinos i
                ON r.{nameof(Reserva.IdInquilino)} = i.{nameof(Inquilino.Id)}

            INNER JOIN Inmuebles m
                ON r.{nameof(Reserva.IdInmueble)} = m.{nameof(Inmueble.Id)}

            WHERE r.{nameof(Reserva.Anulado)} = 0
              AND (
                    CAST(r.{nameof(Reserva.Id)} AS CHAR) LIKE @texto
                    OR i.{nameof(Inquilino.Nombre)} LIKE @texto
                    OR i.{nameof(Inquilino.Apellido)} LIKE @texto
                    OR m.{nameof(Inmueble.Direccion)} LIKE @texto
                  )

            ORDER BY r.{nameof(Reserva.Id)} DESC

            LIMIT 20;
        ";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@texto", $"%{texto}%");

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    reservas.Add(new Reserva
                    {
                        Id = reader.GetInt32(nameof(Reserva.Id)),

                        IdInquilino = reader.GetInt32(
                            nameof(Reserva.IdInquilino)),

                        IdInmueble = reader.GetInt32(
                            nameof(Reserva.IdInmueble)),

                        Arrendatario = new Inquilino
                        {
                            Nombre = reader.GetString(
                                nameof(Inquilino.Nombre)),

                            Apellido = reader.GetString(
                                nameof(Inquilino.Apellido))
                        },

                        DatoInmueble = new Inmueble
                        {
                            Direccion = reader.GetString(
                                nameof(Inmueble.Direccion))
                        }
                    });
                }
            }
        }
    }

    return reservas;
}
}