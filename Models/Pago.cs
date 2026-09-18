using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace agenciaReservas_soazo.Models
{
    public class Pago
    {
        [Display(Name = "CÓDIGO")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "FECHA")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "ID RESERVA")]
        public int IdReserva { get; set; }
        [Display(Name = "RESERVA")]
        public Reserva? DatosReserva { get; set; }


        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "MODO DE PAGO")]
        public string? Modo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "CONCEPTO DEL PAGO")]
        public string? Concepto { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El importe debe ser un número válido con hasta dos decimales.")]
        [Display(Name = "IMPORTE")]
        public decimal Importe {get; set;}

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "ANULADO")]
        public bool Anulado { get; set; }

        [Display(Name = "USUARIO QUE CREÓ EL PAGO")]
        public int IdAlta { get; set; }

        [Display(Name = "USUARIO QUE ANULÓ EL PAGO")]
        public int? IdBaja { get; set; } 

    }
}