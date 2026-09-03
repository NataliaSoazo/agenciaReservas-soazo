
using System.Data;
using MySql.Data.MySqlClient;


namespace agenciaReservas_soazo.Models;

public class RepositorioUsoInmueble
{
    readonly string ConnectionString = "Server=localhost;Database=agenciareservas;User=root;Password=;";

    public RepositorioUsoInmueble()
    {

    }

   
    public int AltaUsoInmueble(UsoInmueble UsoInmueble)
    {
        try
        {
            int id = 0;
            using (var connection = new MySqlConnection(ConnectionString))
            {
                var sql = @$"INSERT INTO Usoinmuebles ({nameof(UsoInmueble.Uso)})
                                            VALUES (@{nameof(UsoInmueble.Uso)});            
             SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(UsoInmueble.Uso)}", UsoInmueble.Uso);
                    
                    connection.Open();
                    id = Convert.ToInt32(command.ExecuteScalar());
                    UsoInmueble.Id = id;
                    connection.Close();
                }
            }
            return id;
        }
        catch (Exception ex)
        {
            // Registrar el error o manejarlo adecuadamente
            Console.WriteLine($"Error al insertar el Usoinmueble: {ex.Message}");
            // Puedes relanzar la excepción o manejarla según sea necesario
            throw;
        }
    }

    public UsoInmueble? getUsoInmueble(int id)
    {
        UsoInmueble? Usoinmueble = null;
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(UsoInmueble.Id)},{nameof(UsoInmueble.Uso)}
            FROM Usoinmuebles WHERE {nameof(UsoInmueble.Id)} = @{nameof(UsoInmueble.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(UsoInmueble.Id)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Usoinmueble = new UsoInmueble
                        {
                            Id = reader.GetInt32(nameof(UsoInmueble.Id)),
                            Uso = reader.GetString(nameof(UsoInmueble.Uso)),
                        
                        };
                    }
                }
            }

        }
        return Usoinmueble;
    }

    public int ModificarUsoInmueble(UsoInmueble Usoinmueble)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"UPDATE Usoinmuebles 
            SET {nameof(UsoInmueble.Uso)} = @{nameof(UsoInmueble.Uso)},
          
            WHERE {nameof(UsoInmueble.Id)} = @{nameof(UsoInmueble.Id)} ";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(UsoInmueble.Id)}", Usoinmueble.Id);
                command.Parameters.AddWithValue($"@{nameof(UsoInmueble.Uso)}", Usoinmueble.Uso);
                

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public int EliminarUsoInmueble(int id)
    {
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"DELETE from Usoinmuebles WHERE {nameof(UsoInmueble.Id)} = @{nameof(UsoInmueble.Id)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(UsoInmueble.Id)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public IList<UsoInmueble> BuscarPorUso(string buscar)
    {
        var res = new List<UsoInmueble>();
        if (string.IsNullOrWhiteSpace(buscar))
        {
            return res; 
        }
        buscar = "%" + buscar + "%";


        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = @$"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Uso)}
                    WHERE {nameof(UsoInmueble.Uso)} LIKE @buscar";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.Add(new MySqlParameter("buscar", MySqlDbType.VarChar) { Value = buscar });
                command.CommandType = CommandType.Text;
                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new UsoInmueble
                        {
                            Id = reader.GetInt32(reader.GetOrdinal(nameof(UsoInmueble.Id))),
                            Uso = reader.GetString(reader.GetOrdinal(nameof(UsoInmueble.Uso))),
                        };
                        res.Add(p);
                    }
                }
            }
        }

        return res;
    }

 public IList<UsoInmueble>ObtenerUsos()
		{
			 var Usos = new List<UsoInmueble>();
			 using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = @$"SELECT {nameof(UsoInmueble.Id)}, {nameof(UsoInmueble.Uso)}
					FROM Usoinmuebles ORDER BY Uso ASC ";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usos.Add(new UsoInmueble
						{
							Id = reader.GetInt32(nameof(UsoInmueble.Id)),
                            Uso =reader.GetString(nameof(UsoInmueble.Uso)),
                            
							
						});	
					}
					connection.Close();
				}
			}
			return Usos;
		}
    
}