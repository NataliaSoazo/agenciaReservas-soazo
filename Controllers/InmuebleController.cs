using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;



namespace agenciaReservas_soazo.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult Index()
    {
        RepositorioInmueble ri = new RepositorioInmueble();
        IList<Inmueble> lista = new List<Inmueble>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            lista = ri.ObtenerTodos();
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            else if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }
            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de inmuebles");
            TempData["Error"] = "Ocurrió un error al obtener los inmuebles.";
            ViewBag.Error = TempData["Error"];
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
        catch (System.Exception)
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
  
    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioInmueble rp = new RepositorioInmueble();
            rp.EliminarInmueble(id);
            TempData["Mensaje"] = "El inmueble ha sido eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["Error"] = "No se pudo completar la eliminación.";
            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Detalles(int id)
    {
        RepositorioInmueble rp = new RepositorioInmueble();
        var i = rp.GetInmueble(id);
        return View(i);
    }

 
    public IActionResult BuscarPropietarios(string buscar)
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
}