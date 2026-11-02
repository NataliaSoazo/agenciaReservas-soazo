using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;

namespace agenciaReservas_soazo.Controllers;

public class ImagenInmuebleController : Controller
{
    private readonly ILogger<ImagenInmuebleController> _logger;   
    private readonly RepositorioImagenInmueble _repoImagen
    private readonly IWebHostEnvironment environment;
    private const string ImagenPorDefecto = "/AnonimoInmueble.png";

    public ImagenInmuebleController(
        ILogger<ImagenInmuebleController> logger,
        RepositorioImagenInmueble repoImagen,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _repoImagen = repoImagen;
        this.environment = environment;
    }

    // AGREGAR O ACTUALIZAR IMAGEN PRINCIPAL
    [Authorize]   
    [HttpPost]
    [Authorize]
[HttpPost]
public async Task<IActionResult> AgregarFotoPrincipal(int InmuebleId, IFormFile archivo)
{
    try
    {
        if (archivo == null || archivo.Length == 0)
        {
            TempData["Error"] = "Debe seleccionar una imagen.";
            return RedirectToAction("Detalles", "Inmueble",
                new { id = InmuebleId });
        }
        string wwwPath = environment.WebRootPath;
        string carpetaInmueble = Path.Combine(
            wwwPath,
            InmuebleId.ToString()
        );
        if (!Directory.Exists(carpetaInmueble))
        {
            Directory.CreateDirectory(carpetaInmueble);
        }
        var imagenes = _repoImagen.BuscarPorInmueble(InmuebleId)
            .OrderBy(i => i.Id)
            .ToList();
        var imagenPrincipal = imagenes.FirstOrDefault();
        string extension = Path.GetExtension(archivo.FileName);
        string fileName = "ImagenDestacada" + extension;
        string fullPath = Path.Combine(carpetaInmueble, fileName);
        // Eliminar portada anterior
        if (imagenPrincipal != null &&
            !string.IsNullOrEmpty(imagenPrincipal.ImgURL) &&
            imagenPrincipal.ImgURL != ImagenPorDefecto)
        {
            string rutaAnterior = Path.Combine(
                wwwPath,
                imagenPrincipal.ImgURL.TrimStart('/')
                    .Replace("/", Path.DirectorySeparatorChar.ToString())
            );
            if (System.IO.File.Exists(rutaAnterior))
            {
                System.IO.File.Delete(rutaAnterior);
            }
        }
        // Guardar nueva portada
        using (FileStream stream = new FileStream(
            fullPath, FileMode.Create))
        {
            await archivo.CopyToAsync(stream);
        }

        string url = $"/{InmuebleId}/{fileName}";

        if (imagenPrincipal != null)
        {
            imagenPrincipal.ImgURL = url;
            _repoImagen.Modificacion(imagenPrincipal);
        }
        else
        {
            imagenPrincipal = new ImagenInmueble
            {
                InmuebleId = InmuebleId,
                ImgURL = url
            };
            _repoImagen.Alta(imagenPrincipal);
        }
        TempData["Mensaje"] =
            "Imagen principal guardada correctamente.";
    }
    catch (Exception ex)
    {
        _logger.LogError(ex,
            "Error al guardar la imagen principal del inmueble {Id}",
            InmuebleId);
        TempData["Error"] =
            "Ocurrió un error al guardar la imagen principal.";
    }

    return RedirectToAction("Detalles", "Inmueble",
        new { id = InmuebleId });
}
    
    // AGREGAR OTRAS IMAGENES
    [HttpPost]
    [Authorize]
[HttpPost]
public async Task<IActionResult> OtrasImagenes(int InmuebleId, List<IFormFile> archivos)
{
    try
    {
        if (archivos == null || archivos.Count == 0)
        {
            TempData["Error"] = "Debe seleccionar al menos una imagen.";
            return RedirectToAction("Detalles", "Inmueble",
                new { id = InmuebleId });
        }

        string wwwPath = environment.WebRootPath;
        string carpetaInmueble = Path.Combine(
            wwwPath,
            InmuebleId.ToString()
        );
        if (!Directory.Exists(carpetaInmueble))
        {
            Directory.CreateDirectory(carpetaInmueble);
        }
        int guardadas = 0;
        int duplicadas = 0;
        foreach (var archivo in archivos)
        {
            if (archivo == null || archivo.Length == 0)
                continue;

            string fileName = Path.GetFileName(archivo.FileName);
            string url = $"/{InmuebleId}/{fileName}";
            // Verificar si ya existe en la base de datos
            var existe = _repoImagen.BuscarPorInmueble(InmuebleId)
                .Any(x => x.ImgURL == url);
            if (existe)
            {
                duplicadas++;
                continue;
            }
            string fullPath = Path.Combine(carpetaInmueble, fileName);
            // Evitar sobrescribir un archivo existente
            if (System.IO.File.Exists(fullPath))
            {
                duplicadas++;
                continue;
            }
            using (FileStream stream = new FileStream(
                fullPath, FileMode.CreateNew))
            {
                await archivo.CopyToAsync(stream);
            }
            ImagenInmueble nuevaImagen = new ImagenInmueble
            {
                InmuebleId = InmuebleId,
                ImgURL = url
            };
            _repoImagen.Alta(nuevaImagen);
            guardadas++;
        }
        if (duplicadas > 0 && guardadas > 0)
        {
            TempData["Mensaje"] =
                $"Se guardaron {guardadas} imágenes correctamente. " +
                $"Se detectaron {duplicadas} imágenes duplicadas.";
        }
        else if (duplicadas > 0)
        {
            TempData["Error"] =
                $"No se guardaron imágenes. Se detectaron {duplicadas} imágenes duplicadas.";
        }
        else
        {
            TempData["Mensaje"] =
                $"Se guardaron {guardadas} imágenes correctamente.";
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex,"Error al guardar las imágenes del inmueble {Id}", InmuebleId);
        TempData["Error"] = "Ocurrió un error al guardar.";
    }
    return RedirectToAction("Detalles", "Inmueble",
        new { id = InmuebleId });
}
    [Authorize]
    [HttpPost]
public IActionResult EliminarImagen(int id)
{
    try
    {
        // Buscar imagen por su ID
        var imagen = _repoImagen.ObtenerPorId(id);
        if (imagen != null)
        {
            int inmuebleId = imagen.InmuebleId;
            // Obtener ruta física del archivo
            if (!string.IsNullOrEmpty(imagen.ImgURL) &&
                imagen.ImgURL != ImagenPorDefecto)
            {
                string wwwPath = environment.WebRootPath;
                string rutaArchivo = Path.Combine(
                    wwwPath,
                    imagen.ImgURL.TrimStart('/')
                        .Replace("/", Path.DirectorySeparatorChar.ToString())
                );
                // Eliminar archivo físico
                if (System.IO.File.Exists(rutaArchivo))
                {
                    System.IO.File.Delete(rutaArchivo);
                }
            }
            // Eliminar registro de la base de datos
            _repoImagen.Baja(id);
            // Si se eliminó la portada, registrar imagen por defecto
            var imagenesRestantes = _repoImagen.BuscarPorInmueble(inmuebleId);
            if (imagenesRestantes == null || !imagenesRestantes.Any())
            {
                ImagenInmueble imagenPorDefecto = new ImagenInmueble
                {
                    InmuebleId = inmuebleId,
                    ImgURL = ImagenPorDefecto
                };
                _repoImagen.Alta(imagenPorDefecto);
            }
            TempData["Mensaje"] = "La imagen se eliminó correctamente.";
            return RedirectToAction("Detalles", "Inmueble",
                new { id = inmuebleId });
        }

        TempData["Error"] = "No se encontró la imagen.";
        return RedirectToAction("Index", "Inmueble");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al eliminar la imagen {Id}", id);
        TempData["Error"] = "Ocurrió un error al eliminar la imagen.";
        return RedirectToAction("Index", "Inmueble");
    }
}
}
