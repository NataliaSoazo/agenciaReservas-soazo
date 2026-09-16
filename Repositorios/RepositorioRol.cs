using agenciaReservas_soazo.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace agenciaReservas_soazo.Repositorios;
public class RepositorioRol
{
    readonly string ConnectionString = "Server=localhost;Database=proyecto-bruno-soazo;User=root;Password=;";

    public RepositorioRol()
    {

    }
    
 public IList<Rol>ObtenerRoles()
		{
			 var roles = new List<Rol>();
			 using (var connection = new MySqlConnection(ConnectionString))
			{
				string sql = @$"SELECT {nameof(Rol.Numero)}, {nameof(Rol.rol)}
					FROM Roles  ";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						roles.Add(new Rol
						{
							
                            Numero =reader.GetInt32(nameof(Rol.Numero)),
                            rol =reader.GetString(nameof(Rol.rol)),
							
						});	
					}
					connection.Close();
				}
			}
			return roles;
		}
    
}