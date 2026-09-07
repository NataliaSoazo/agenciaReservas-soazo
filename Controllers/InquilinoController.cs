using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Repositorios;
using agenciaReservas_soazo.Models;
namespace agenciaReservas_soazo.Controllers;
public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;


    public InquilinoController(ILogger<InquilinoController> logger)
    {

        _logger = logger;
    }
    
    public IActionResult Index()
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
    }
    
    public IActionResult Editar(int id)
    {
        if (id > 0)
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            var inquilino = rp.GetInquilino(id);
            return View(inquilino);
        }
        else
        {
            return View();
        }
    }
   
    public IActionResult Guardar(Inquilino inquilino)
    {
        try
        {
            inquilino.Nombre = inquilino.Nombre.ToUpper();
            inquilino.Apellido = inquilino.Apellido.ToUpper();
            inquilino.Email = inquilino.Email.ToUpper();
            inquilino.Domicilio = inquilino.Domicilio.ToUpper();
            inquilino.Ciudad = inquilino.Ciudad.ToUpper();
            RepositorioInquilino rp = new RepositorioInquilino();

            if (inquilino.Id > 0)
            {
                rp.ModificarInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido modificado";

            }
            else
            {
                rp.AltaInquilino(inquilino);
                TempData["Mensaje"] = "El inquilino ha sido guardado";
            }
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            TempData["Error"] = "Ocurrió un error al guardar el inquilino";
            return RedirectToAction(nameof(Index));
        }
    }
    
    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            rp.EliminarInquilino(id);
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

    
    public IActionResult Detalles(int id)
    {
        RepositorioInquilino rp = new RepositorioInquilino();
        var inquilino = rp.GetInquilino(id);
        return View(inquilino);
    }

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
    
    public IActionResult BuscarInquilino(string buscar)
    {
        try
        {
            RepositorioInquilino rp = new RepositorioInquilino();
            IList<Inquilino> inquilino = rp.BuscarPorNombre(buscar);
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