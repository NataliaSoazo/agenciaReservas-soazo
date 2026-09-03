using System.Data;
using MySql.Data.MySqlClient;

namespace agenciaReservas_soazo.Models;
public class RepositorioInmueble
{
    readonly string ConnectionString = "Server=localhost;Database=agenciareservas;User=root;Password=;";

    public RepositorioInmueble()
    {

    }
    public IList<Inmueble> ObtenerTodos()
    {
        var inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(ConnectionString))
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
                        Precio = reader.GetDouble(nameof(Inmueble.Precio)),
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

    public int AltaInmueble(Inmueble inmueble)
    {
        int id = 0;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"INSERT INTO inmuebles ({nameof(Inmueble.Direccion)}, {nameof(Inmueble.Cupo)}, {nameof(Inmueble.Uso)},  {nameof(Inmueble.Precio)}, {nameof(Inmueble.Disponible)}, {nameof(Inmueble.PropietarioId)}, {nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Tipo)})
                                     VALUES (@{nameof(Inmueble.Direccion)}, @{nameof(Inmueble.Cupo)}, @{nameof(Inmueble.Uso)},  @{nameof(Inmueble.Precio)}, @{nameof(Inmueble.Disponible)}, @{nameof(Inmueble.PropietarioId)}, @{nameof(Inmueble.Latitud)}, @{nameof(Inmueble.Longitud)}, @{nameof(Inmueble.Tipo)});            
             SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Tipo)}", inmueble.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Cupo)}", inmueble.Cupo);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
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
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = @$"SELECT i.{nameof(Inmueble.Id)},{nameof(Inmueble.Direccion)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Cupo)},{nameof(Inmueble.Precio)}, {nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Disponible)},{nameof(Inmueble.PropietarioId)},
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
                            Precio = reader.GetDouble(nameof(Inmueble.Precio)),
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
        using (var connection = new MySqlConnection(ConnectionString))
        {
            string sql = @$"UPDATE inmuebles SET 
				{nameof(Inmueble.Direccion)} = @{nameof(Inmueble.Direccion)}, 
                {nameof(Inmueble.Tipo)} = @{nameof(Inmueble.Tipo)},
                {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                {nameof(Inmueble.Cupo)} = @{nameof(Inmueble.Cupo)},
                {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)},
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
        using (var connection = new MySqlConnection(ConnectionString))
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
    public IList<TipoInmueble> ObtenerTipos()
    {
        var tipos = new List<TipoInmueble>();
        using (var connection = new MySqlConnection(ConnectionString))
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

    public IList<Inmueble> ObtenerPorPropietario(int propietarioId)
    {
        var res = new List<Inmueble>();

        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble.Id)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Disponible)}, {nameof(Inmueble.PropietarioId)}
                    FROM inmuebles
                    WHERE {nameof(Inmueble.PropietarioId)} = @propietarioId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add(new MySqlParameter("propietarioId", MySqlDbType.Int32) { Value = propietarioId });
                command.CommandType = CommandType.Text;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.Id))),
                            Direccion = reader.GetString(reader.GetOrdinal(nameof(Inmueble.Direccion))),
                            Uso = reader.GetString(reader.GetOrdinal(nameof(Inmueble.Uso))),
                            Disponible = reader.GetString(reader.GetOrdinal(nameof(Inmueble.Disponible))),
                            PropietarioId = reader.GetInt32(reader.GetOrdinal(nameof(Inmueble.PropietarioId)))
                        };
                        res.Add(inmueble);
                    }
                }
            }
        }

        return res;
    }

    


}