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
    private readonly RepositorioReserva _reserva;
    private readonly RepositorioImagenInmueble _ImagenInmueble;


    public InmuebleController(
        ILogger<InmuebleController> logger,
        RepositorioInmueble repoInmueble,
        RepositorioPropietario repoPropietario,
        RepositorioTipoInmueble repoTipo,
        RepositorioUsoInmueble repoUso,
        RepositorioReserva reserva,
        RepositorioImagenInmueble imagenInmueble)
    {
        _logger = logger;
        _repoInmueble = repoInmueble;
        _repoPropietario = repoPropietario;
        _repoTipo = repoTipo;
        _repoUso = repoUso;
        _reserva = reserva;
        _ImagenInmueble = imagenInmueble;
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
        try
        {
            var i = _repoInmueble.GetInmueble(id);
            ViewBag.Imagenes = _ImagenInmueble
            .BuscarPorInmueble(id)
            .OrderBy(i => i.Id)
            .ToList();


        return View(i);
        }
        catch (System.Exception ex)
        {
            
            _logger.LogError(ex, "Error al obtener los datos");
            TempData["Error"] = "Ocurrió un error.";
            return View(Index);
        }
    }

    [Authorize]   
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
    
    [Authorize]
    public IActionResult sinReservasXdias(int cantidadDias)
    {
    ViewBag.UserRole = User.Claims
        .FirstOrDefault(c => c.Type == "Rol")?.Value;

    try
    {
        if (cantidadDias < 1)
        {
            TempData["Error"] = "La cantidad de días debe ser mayor que cero.";
            return RedirectToAction("Index");
        }

        DateTime fechaDesde = DateTime.Today.AddDays(-cantidadDias);
        DateTime fechaHasta = DateTime.Now;

        var inmueblesSinReservas = _repoInmueble.SinReservasXdias(
            fechaDesde,
            fechaHasta
        );

        TempData["Mensaje"] = "Búsqueda exitosa.";

        return View("Index", inmueblesSinReservas);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al buscar inmuebles sin reservas");

        TempData["Error"] =
            "Ocurrió un error al buscar los inmuebles sin reservas.";

        return RedirectToAction("Index");
    }
    }

    [Authorize]   
    public IActionResult MasReservados365()
    {
    ViewBag.UserRole = User.Claims
        .FirstOrDefault(c => c.Type == "Rol")?.Value;

    try
    {
        DateTime fechaDesde = DateTime.Today.AddDays(-365);
        DateTime fechaHasta = DateTime.Now;

        var resultado = _repoInmueble.MasReservados365(fechaDesde,fechaHasta);
        return View("Index", resultado);
        }
        catch (Exception ex)
       {
        _logger.LogError(ex, "Error al obtener los inmuebles más reservados");

        TempData["Error"] = "Ocurrió un error al obtener los inmuebles más reservados.";

        return RedirectToAction("Index");
    }
}

public IActionResult DispuestosxDueño()
    {
       ViewBag.UserRole = User.Claims
        .FirstOrDefault(c => c.Type == "Rol")?.Value;

    try
    {

        var resultado = _repoInmueble.ObtenerDisponibles();
        return View("Index", resultado);
        }
        catch (Exception ex)
       {
        _logger.LogError(ex, "Error al obtener los inmuebles dispuestos por sus propietarios");

        TempData["Error"] = "Ocurrió un error al obtener los inmuebles dispuestos por sus propietarios.";

        return RedirectToAction("Index");
    }
    }
}