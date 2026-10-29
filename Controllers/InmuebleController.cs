using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreGeneratedDocument;

namespace agenciaReservas_soazo.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;
    private readonly RepositorioInmueble _repoInmueble;
    private readonly RepositorioPropietario _repoPropietario;
    private readonly RepositorioTipoInmueble _repoTipo;
    private readonly RepositorioUsoInmueble _repoUso;

    public InmuebleController(
        ILogger<InmuebleController> logger,
        RepositorioInmueble repoInmueble,
        RepositorioPropietario repoPropietario,
        RepositorioTipoInmueble repoTipo,
        RepositorioUsoInmueble repoUso)
    {
        _logger = logger;
        _repoInmueble = repoInmueble;
        _repoPropietario = repoPropietario;
        _repoTipo = repoTipo;
        _repoUso = repoUso;
    }

    [Authorize]
    public IActionResult Index(int pagina = 1)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;

        IList<Inmueble> lista = new List<Inmueble>();

        try
        {
            int cantidadPorPagina = 10;

            int totalInmuebles = _repoInmueble.GetCantidadInmuebles();

            lista = _repoInmueble.GetInmueblesPaginados(
                pagina,
                cantidadPorPagina
            );

            int totalPaginas = (int)Math.Ceiling(
                (double)totalInmuebles / cantidadPorPagina
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
                "Error al obtener la lista de inmuebles"
            );

            TempData["Error"] =
                "Ocurrió un error al obtener la lista de inmuebles";

            return View(lista);
        }
    }

    [Authorize]
    public IActionResult Editar(int id)
    {
        ViewBag.Propietarios = _repoPropietario.GetPropietarios();
        ViewBag.TipoInmuebles = _repoTipo.ObtenerTipos();
        ViewBag.UsoInmuebles = _repoUso.ObtenerUsos();

        if (id > 0)
        {
            var inmueble = _repoInmueble.GetInmueble(id);
            return View(inmueble);
        }

        return View();
    }

    [Authorize]
    public IActionResult Guardar(Inmueble inmueble)
    {
        try
        {
            inmueble.Direccion = inmueble.Direccion.ToUpper();
            inmueble.Uso = inmueble.Uso.ToUpper();
            inmueble.Disponible = inmueble.Disponible.ToUpper();

            if (inmueble.Cupo <= 0)
            {
                TempData["Error"] =
                    "La cantidad de ambientes debe ser mayor a cero.";

                return RedirectToAction(nameof(Index));
            }

            if (inmueble.Id > 0)
            {
                _repoInmueble.ModificarInmueble(inmueble);

                TempData["Mensaje"] =
                    "El inmueble se modificó correctamente.";
            }
            else
            {
                _repoInmueble.AltaInmueble(inmueble);

                TempData["Mensaje"] =
                    "Se agregó el inmueble correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");

            TempData["Error"] =
                "No se pudo completar la operación.";

            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            _repoInmueble.EliminarInmueble(id);

            TempData["Mensaje"] =
                "El inmueble ha sido eliminado correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");

            TempData["Error"] =
                "No se pudo completar la eliminación. " + ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

    [Authorize]
    public IActionResult Detalles(int id)
    {
        var i = _repoInmueble.GetInmueble(id);

        return View(i);
    }

    public IActionResult BuscarPropietarios(string buscar)
    {
        try
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
            ViewBag.UserRole = userRole;

            IList<Propietario> propietarios =
                _repoPropietario.BuscarPorNombre(buscar);

            IList<Inmueble> inmuebles = new List<Inmueble>();

            foreach (var propietario in propietarios)
            {
                var inmueblesPropietario =
                    _repoInmueble.ObtenerPorPropietario(propietario.Id);

                inmuebles = inmuebles
                    .Concat(inmueblesPropietario)
                    .ToList();
            }

            return View("Index", inmuebles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");

            TempData["Error"] =
                "No se pudo completar la búsqueda. " + ex.Message;

            return RedirectToAction(nameof(Index));
        }
    }

[Authorize]
public IActionResult DisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        var inmueblesDisponibles  = _repoInmueble.obtenerDisponiblesPorFecha(fechaInicio, fechaFin);
        return View("Index", inmueblesDisponibles);
    }
}