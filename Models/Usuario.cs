using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace agenciaReservas_soazo.Models;

public class Usuario
{
	[Key]
	[Display(Name = "CÓDIGO")]
	public int Id { get; set; }
	
	[Display(Name = "NOMBRE")]
	public string? Nombre { get; set; }
	
	[Display(Name = "APELLIDO")]
	public string? Apellido { get; set; }
	
	[Display(Name = "E- MAIL")]
	public string? Correo { get; set; }
	[DataType(DataType.Password)]
	[Display(Name = "CLAVE")]
	public string? Clave { get; set; }
	[Display(Name = "AVATAR")]
	public string? AvatarURL { get; set; }
	[NotMapped]//Para EF
	[Display(Name = "AVATAR")]
	public IFormFile? AvatarFile { get; set; }
	[Display(Name = "ROL")]
	public int Rol { get; set; }
	[Display(Name = "ROL")]
	public Rol? Datos { get; set; }
	
}

public class Rol
{
	public int Numero { get; set; }
	public string rol { get; set; }
}

public class Log
{
	public string Usuario { get; set; }
	public string Clave { get; set; }
}
