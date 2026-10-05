using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;


namespace agenciaReservas_soazo.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;
    private readonly RepositorioPropietario _propietario;


    public PropietarioController(ILogger<PropietarioController> logger, RepositorioPropietario propietario)
    {

        _logger = logger;
        _propietario = propietario;
    }
     [Authorize]
    public IActionResult Index(int pagina = 1)

    {

        IList<Propietario> lista = new List<Propietario>();

        try
        {
            int cantidadPorPagina = 10;
            // Cantidad total de registros
            int totalPropietarios = _propietario.GetCantidadPropietarios();

            // Obtener solamente los registros de esta página
            lista = _propietario.GetPropietariosPaginados(pagina, cantidadPorPagina);

            // Cantidad total de páginas
            int totalPaginas = (int)Math.Ceiling(
                (double)totalPropietarios / cantidadPorPagina
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
            _logger.LogError(ex, "Error al obtener la lista de Propietarios");
            TempData["Error"] = "Ocurrio un error al obtener la lista de Propietarios";
            return View(lista);
        }
    }

    public IActionResult Editar(int id)
    {
        try
        {
            if (id > 0)
        {
            var propietario = _propietario.getPropietario(id);
            return View(propietario);
        }
        else
        {
            return View();
        }
        }
        catch (System.Exception ex)
        {
            
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrió un error";
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize]
    public IActionResult Guardar(Propietario propietario)
    {
        try
        {
            propietario.Nombre = propietario.Nombre.ToUpper();
            propietario.Apellido = propietario.Apellido.ToUpper();
            propietario.Email = propietario.Email.ToUpper();
            propietario.Domicilio = propietario.Domicilio.ToUpper();
            propietario.Ciudad = propietario.Ciudad.ToUpper();

            if (propietario.Id > 0)
            {
                _propietario.ModificarPropietario(propietario);
                TempData["Mensaje"] = "El propietario ha sido modificado";

            }
            else
            {
                _propietario.AltaPropietario(propietario);
                TempData["Mensaje"] = "El propietario ha sido guardado";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrió un error al guardar el propietario, verifique posibles valores únicos duplicados";
            return RedirectToAction(nameof(Index));
        }

    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            _propietario.EliminarPropietario(id);
            TempData["Mensaje"] = "El propietario ha sido eliminado";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrio un error al eliminar el propietario";
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize]
    public IActionResult Detalles(int id)
    {
        var propietario = _propietario.getPropietario(id);
        return View(propietario);
    }
    [Authorize]
    // GET: Propietario/Busqueda
    public IActionResult Busqueda()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            throw;
        }
    }
    //[Route("[controller]/Buscar/{q}", Name = "Buscar")]
    [Authorize]
    public IActionResult BuscarPropietario(string buscar)
    {
        try
        {
            IList<Propietario> p = _propietario.BuscarPorNombre(buscar);
            return View("Index",p );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrio un error al buscar el propietario";
            return RedirectToAction(nameof(Index));
        }
    }
}