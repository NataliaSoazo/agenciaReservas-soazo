using System.ComponentModel.DataAnnotations;

namespace agenciaReservas_soazo.Models;

public class Propietario
{
    [Display(Name = "CÓDIGO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public int Id { get; set; }
    [Display(Name = "NOMBRE")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Ingrese un nombre  válido")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Nombre { get; set; }
    [Display(Name = "APELLIDO")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Ingrese un apellido válido")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Apellido { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "E-MAIL")]
    [StringLength(50, MinimumLength = 13, ErrorMessage = "Ingrese un mail válido")]
    public string? Email { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(50, MinimumLength = 7, ErrorMessage = "Ingrese un DNI válido")]
    [Display(Name = "DNI")]
    public string? Dni { get; set; }
    [Display(Name = "TELEFONO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(50, MinimumLength = 10, ErrorMessage = "Ingrese un teléfono válido")]
    public string? Telefono { get; set; }
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Ingrese un domicilio válido")]
    [Display(Name = "DOMICILIO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Domicilio { get; set; }
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Ingrese una ciudad  válida")]
    [Display(Name = "CIUDAD")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Ciudad { get; set; }
    public string? RequestId { get; set; }

    
      public override string ToString()
    {
    return this.Id + " " + this.Apellido + " " + this.Nombre;
    }
}