using System.Data;
using MySql.Data.MySqlClient;
using agenciaReservas_soazo.Models;

namespace agenciaReservas_soazo.Repositorios;

public class RepositorioImagenInmueble : RepositorioBase
{
    public RepositorioImagenInmueble(IConfiguration configuration)
        : base(configuration)
    {
    }

    public int Alta(ImagenInmueble p)
    {
        int res = -1;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"INSERT INTO ImagenInmueble
                (InmuebleId, ImgURL)
                VALUES (@inmuebleId, @imgURL)";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@inmuebleId", p.InmuebleId);
                command.Parameters.AddWithValue("@imgURL", p.ImgURL);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }

        return res;
    }

    public int Baja(int id)
    {
        int res = -1;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"DELETE FROM ImagenInmueble
                           WHERE Id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }

        return res;
    }

    public int Modificacion(ImagenInmueble p)
    {
        int res = -1;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE ImagenInmueble
                           SET ImgURL = @imgURL
                           WHERE Id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", p.Id);
                command.Parameters.AddWithValue("@imgURL", p.ImgURL);

                connection.Open();
                res = command.ExecuteNonQuery();
            }
        }

        return res;
    }

    public ImagenInmueble? ObtenerPorId(int id)
    {
        ImagenInmueble? res = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
                    i.Id,
                    i.InmuebleId,
                    i.ImgURL
                    FROM ImagenInmueble i
                    WHERE i.Id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = new ImagenInmueble
                        {
                            Id = reader.GetInt32(nameof(ImagenInmueble.Id)),
                            InmuebleId = reader.GetInt32(nameof(ImagenInmueble.InmuebleId)),
                            ImgURL = reader.GetString(nameof(ImagenInmueble.ImgURL))
                        };
                    }
                }
            }
        }

        return res;
    }

    public IList<ImagenInmueble> ObtenerLista(
        int paginaNro = 1,
        int tamPagina = 10)
    {
        List<ImagenInmueble> res = new List<ImagenInmueble>();

        int offset = (paginaNro - 1) * tamPagina;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
                    i.Id,
                    i.InmuebleId,
                    i.ImgURL
                    FROM ImagenInmueble i
                    ORDER BY i.Id
                    LIMIT @tamPagina OFFSET @offset";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;

                command.Parameters.AddWithValue("@tamPagina", tamPagina);
                command.Parameters.AddWithValue("@offset", offset);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(new ImagenInmueble
                        {
                            Id = reader.GetInt32(nameof(ImagenInmueble.Id)),
                            InmuebleId = reader.GetInt32(nameof(ImagenInmueble.InmuebleId)),
                            ImgURL = reader.GetString(nameof(ImagenInmueble.ImgURL))
                        });
                    }
                }
            }
        }

        return res;
    }

    public int ObtenerCantidad()
    {
        int res = 0;

        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT COUNT(Id)
                           FROM ImagenInmueble";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
            }
        }

        return res;
    }

    public IList<ImagenInmueble> BuscarPorInmueble(int inmuebleId)
{
    List<ImagenInmueble> res = new List<ImagenInmueble>();

    using (var connection = new MySqlConnection(connectionString))
    {
        string sql = @"SELECT
                i.Id,
                i.InmuebleId,
                i.ImgURL
                FROM ImagenInmueble i
                WHERE i.InmuebleId = @inmuebleId
                ORDER BY i.Id";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@inmuebleId", inmuebleId);

            connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    res.Add(new ImagenInmueble
                    {
                        Id = reader.GetInt32(nameof(ImagenInmueble.Id)),
                        InmuebleId = reader.GetInt32(nameof(ImagenInmueble.InmuebleId)),
                        ImgURL = reader.GetString(nameof(ImagenInmueble.ImgURL))
                    });
                }
            }
        }
    }

    return res;
}
}