using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Repositorios;
using agenciaReservas_soazo.Models;
using Microsoft.AspNetCore.Authorization;
namespace agenciaReservas_soazo.Controllers;
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;
    private readonly RepositorioInquilino _repositorio;


    public InquilinoController(ILogger<InquilinoController> logger, RepositorioInquilino repositorio)
    {

        _logger = logger;
        _repositorio = repositorio;
    }
    
 /*   public IActionResult Index()
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        IList<Inquilino> lista = new List<Inquilino>();
        try
        {
            lista = rp.GetInquilinos();
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de inquilinos");
            TempData["Error"] = "Ocurrio un error al obtener la lista de inquilinos";
            return View(lista);
        }
    }*/
 [Authorize]
public IActionResult Index(int pagina = 1)
{

    IList<Inquilino> lista = new List<Inquilino>();

    try
    {
        int cantidadPorPagina = 10;

        // Cantidad total de registros
        int totalInquilinos = _repositorio.GetCantidadInquilinos();

        // Obtener los registros correspondientes a la página
        lista =  _repositorio.GetInquilinosPaginados(pagina, cantidadPorPagina);
        // Cantidad total de páginas
        int totalPaginas = (int)Math.Ceiling(
            (double)totalInquilinos / cantidadPorPagina
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
        _logger.LogError(ex, "Error al obtener la lista de inquilinos");
        TempData["Error"] = "Ocurrió un error al obtener la lista de inquilinos";
        return View(lista);
    }
}
 [Authorize]
public IActionResult Editar(int id)
    {
        if (id > 0)
        {
            var inquilino =  _repositorio.GetInquilino(id);
            return View(inquilino);
        }
        else
        {
            return View();
        }
    }
 [Authorize]
public IActionResult Guardar(Inquilino inquilino)
    {
        try
        {
            inquilino.Nombre = inquilino.Nombre.ToUpper();
            inquilino.Apellido = inquilino.Apellido.ToUpper();
            inquilino.Email = inquilino.Email.ToUpper();
            inquilino.Domicilio = inquilino.Domicilio.ToUpper();
            inquilino.Ciudad = inquilino.Ciudad.ToUpper();

            if (inquilino.Id > 0)
            {
                 _repositorio.ModificarInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido modificado";

            }
            else
            {
                 _repositorio.AltaInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido guardado";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrió un error al guardar el inquilino, verifique posibles valores únicos duplicados";
            return RedirectToAction(nameof(Index));
        }
    }
[Authorize(Policy = "Administrador")]    
public IActionResult Eliminar(int id)
    {
        try
        {
             _repositorio.EliminarInquilino(id);
            TempData["Mensaje"] = "El inquilino ha sido eliminado";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar el inquilino");
            TempData["Error"] = "Ocurrio un error al intentar eliminar el inquilino";
            return RedirectToAction(nameof(Index));
        }
    }

 [Authorize]    
public IActionResult Detalles(int id)
    {
       try
       {
         var inquilino =  _repositorio.GetInquilino(id);
        return View(inquilino);
       }
       catch (System.Exception ex)
       {
         _logger.LogError(ex, "Error al eliminar el inquilino");
            TempData["Error"] = "Ocurrio un error al intentar eliminar el inquilino";
            return RedirectToAction(nameof(Index));
       }
    }


 [Authorize]
    // GET: inquilino/Busqueda
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
    //  [Route("[controller]/Buscar/{q}", Name = "Buscar")]
 [Authorize]
    public IActionResult BuscarInquilino(string buscar)
    {
        try
        {
            IList<Inquilino> inquilino = _repositorio.BuscarPorNombre(buscar);
            return View("Index", inquilino);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrio un error al buscar el inquilino";
            return RedirectToAction(nameof(Index));
        }
    }
}