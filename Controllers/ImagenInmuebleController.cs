using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;

namespace agenciaReservas_soazo.Controllers;

public class ImagenInmuebleController : Controller
{
    private readonly ILogger<ImagenInmuebleController> _logger;
    private readonly RepositorioImagenInmueble _repoImagen;

    public ImagenInmuebleController(
        ILogger<ImagenInmuebleController> logger,
        RepositorioImagenInmueble repoImagen)
    {
        _logger = logger;
        _repoImagen = repoImagen;
    }

    [Authorize]
    public IActionResult Index(int pagina = 1)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;

        IList<ImagenInmueble> lista = new List<ImagenInmueble>();

        try
        {
            int cantidadPorPagina = 10;

            int totalImagenes = _repoImagen.ObtenerCantidad();

            lista = _repoImagen.ObtenerLista(
                pagina,
                cantidadPorPagina
            );

            int totalPaginas = (int)Math.Ceiling(
                (double)totalImagenes / cantidadPorPagina
            );

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }

            if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al obtener la lista de imágenes"
            );

            TempData["Error"] =
                "Ocurrió un error al obtener la lista de imágenes";

            return View(lista);
        }
    }

    [Authorize]
[HttpPost]
public IActionResult AgregarImagen(int InmuebleId, IFormFile archivo)
{
    try
    {
        if (archivo == null || archivo.Length == 0)
        {
            TempData["Error"] = "Debe seleccionar una imagen.";

            return RedirectToAction(
                "Detalles",
                "Inmueble",
                new { id = InmuebleId }
            );
        }

        string carpeta = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "img",
            "inmuebles",
            InmuebleId.ToString()
        );

        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        string extension = Path.GetExtension(archivo.FileName);
        string nombreArchivo = Guid.NewGuid().ToString() + extension;

        string rutaCompleta = Path.Combine(
            carpeta,
            nombreArchivo
        );

        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        {
            archivo.CopyTo(stream);
        }

        string rutaBD =
            $"/img/inmuebles/{InmuebleId}/{nombreArchivo}";

        var imagen = new ImagenInmueble
        {
            InmuebleId = InmuebleId,
            ImgURL = rutaBD
        };

        _repoImagen.Alta(imagen);

        TempData["Mensaje"] = "La foto se agregó correctamente.";

        return RedirectToAction(
            "Detalles",
            "Inmueble",
            new { id = InmuebleId }
        );
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al agregar la imagen");

        TempData["Error"] = "No se pudo subir la foto.";

        return RedirectToAction(
            "Detalles",
            "Inmueble",
            new { id = InmuebleId }
        );
    }
}
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            _repoImagen.Baja(id);

            TempData["Mensaje"] =
                "La imagen ha sido eliminada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar la imagen");

            TempData["Error"] =
                "No se pudo completar la eliminación. " + ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize]
    public IActionResult Detalles(int id)
{
    var inmueble = _repoImagen.ObtenerPorId(id);

    if (inmueble == null)
    {
        return NotFound();
    }

    var imagenes = _repoImagen.BuscarPorInmueble(id);

    ViewBag.Imagenes = imagenes;

    return View(inmueble);
}

    [Authorize]
    public IActionResult BuscarPorInmueble(int inmuebleId)
    {
        try
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;

            var imagenes = _repoImagen.BuscarPorInmueble(inmuebleId);

            return View("Index", imagenes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar imágenes del inmueble");

            TempData["Error"] =
                "No se pudo completar la búsqueda. " + ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}