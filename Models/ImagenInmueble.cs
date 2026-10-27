using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace agenciaReservas_soazo.Models;

    public class ImagenInmueble
    {
        [Key]
        [Display(Name ="Código Int.")]
        public int Id { get; set; }

        public int InmuebleId { get; set; }

        [Display(Name = "Imagen del inmueble")]
        public string? ImgURL { get; set; }
        [NotMapped]
        
        public IFormFile? Archivo { get; set; } = null;

    }
