using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agenciaReservas_soazo.Models;
public class Inmueble

    {
    [Display(Name = "CÓDIGO")]
    public int Id { get; set; }
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Ingrese una dirección válida")]
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "DIRECCIÓN")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    public string? Tipo { get; set; }
    
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "USO")]
    public string? Uso { get; set; }
    [Display(Name = "CUPO")]
    [Range(1, 10, ErrorMessage = "El cupo debe ser mayor a 0")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public int Cupo { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "PRECIO APROX.")]
    [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe ser un número válido con hasta dos decimales.")]
    public double? Precio { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(20, MinimumLength = 9, ErrorMessage = "Ingrese una latitud válida")]
    [Display(Name = "LATITUD")]
    public string? Latitud { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [StringLength(20, MinimumLength = 9, ErrorMessage = "Ingrese una longitud válida")]
    [Display(Name = "LONGITUD")]
    public string? Longitud { get; set; }
    [Required(ErrorMessage = "Campo obligatorio")]
    [Display(Name = "DISPONIBLE")]
    public string? Disponible { get; set; }
    [Display(Name = "DUEÑO")]
    [Required(ErrorMessage = "Campo obligatorio")]
    public int PropietarioId { get; set; }
    [ForeignKey(nameof(PropietarioId))]
    public Propietario? Duenio { get; set; }
}

        public class TipoInmueble{
             [Display(Name = "CÓDIGO")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Campo obligatorio")]
            [Display(Name = "TIPO")]
            public string Tipo { get; set; }
        }
        public class UsoInmueble{
             [Display(Name = "CÓDIGO")]
            public int Id { get; set; }
            [Required(ErrorMessage = "Campo obligatorio")]
            [Display(Name = "USO")]
            public string Uso { get; set; }
        }


    