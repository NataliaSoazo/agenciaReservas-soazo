using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;


namespace agenciaReservas_soazo.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index(int pagina = 1)
{
    var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
    RepositorioInmueble rp = new RepositorioInmueble();
    IList<Inmueble> lista = new List<Inmueble>();
    try
    {
        int cantidadPorPagina = 10;
        // Cantidad total de inmuebles
        int totalInmuebles = rp.GetCantidadInmuebles();
        // Obtener los inmuebles correspondientes a la página
        lista = rp.GetInmueblesPaginados(
            pagina,
            cantidadPorPagina
        );
        // Calcular cantidad de páginas
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

   /* public IActionResult DisponiblesPorFechas(DateTime fechaInicio, DateTime fechaFin)
    {
        RepositorioContrato rc = new RepositorioContrato();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        var inmueblesDisponibles = rc.obtenerInmDisp(fechaInicio, fechaFin);
        return View("Index", inmueblesDisponibles);
    }*/
     [Authorize]
    public IActionResult Editar(int id)

    {
        RepositorioPropietario repoPropietario = new RepositorioPropietario();
        ViewBag.Propietarios = repoPropietario.GetPropietarios();
        RepositorioTipoInmueble repoTipo = new RepositorioTipoInmueble();
        ViewBag.TipoInmuebles = repoTipo.ObtenerTipos();
        RepositorioUsoInmueble repoUso = new RepositorioUsoInmueble();
        ViewBag.UsoInmuebles = repoUso.ObtenerUsos();


        if (id > 0)
        {
            RepositorioInmueble rp = new RepositorioInmueble();
            var inmueble = rp.GetInmueble(id);
            return View(inmueble);
        }
        else
        {
            return View();
        }
    }
     [Authorize]
    public IActionResult Guardar(Inmueble inmueble)
    {
        try
        {
            inmueble.Direccion = inmueble.Direccion.ToUpper();
            inmueble.Uso = inmueble.Uso.ToUpper();
            inmueble.Disponible = inmueble.Disponible.ToUpper();
            RepositorioInmueble rp = new RepositorioInmueble();
            if (inmueble.Cupo <= 0)
            {
                TempData["Error"] = "La cantidad de ambientes debe ser mayor a cero.";
                return RedirectToAction(nameof(Index));
            }
            if (inmueble.Id > 0)
            {
                rp.ModificarInmueble(inmueble);
                TempData["Mensaje"] = "El inmueble se  modificó correctamente.";

            }
            else
            {
                rp.AltaInmueble(inmueble);
                TempData["Mensaje"] = "Se agregó el inmueble correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex )
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioInmueble rp = new RepositorioInmueble();
            rp.EliminarInmueble(id);
            TempData["Mensaje"] = "El inmueble ha sido eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch(Exception ex )
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "No se pudo completar la eliminación. "+ ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
     [Authorize]
    public IActionResult Detalles(int id)
    {
        RepositorioInmueble rp = new RepositorioInmueble();
        var i = rp.GetInmueble(id);
        return View(i);
    }

 
    public IActionResult BuscarPropietarios(string buscar)
    {
        try
        {
            var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        RepositorioPropietario rp = new RepositorioPropietario();
        RepositorioInmueble ri = new RepositorioInmueble();

        IList<Propietario> propietarios = rp.BuscarPorNombre(buscar);
        IList<Inmueble> inmuebles = new List<Inmueble>();

        foreach (var propietario in propietarios)
        {
            var inmueblesPropietario = ri.ObtenerPorPropietario(propietario.Id);
            inmuebles = inmuebles.Concat(inmueblesPropietario).ToList();
        }

        return View("Index", inmuebles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "No se pudo completar la eliminación. "+ ex.Message;
            return RedirectToAction(nameof(Index));
            throw;
          } 
     }
}