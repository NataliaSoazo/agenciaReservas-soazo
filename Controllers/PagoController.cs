using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using AspNetCoreGeneratedDocument;
namespace agenciaReservas_soazo.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly RepositorioPago _repositorio;
    private readonly RepositorioReserva _reserva;
    private readonly RepositorioUsuario _usuario;

    public PagoController(ILogger<HomeController> logger, RepositorioPago repositorio, RepositorioReserva reserva, RepositorioUsuario usuario)
    {
        _logger = logger;
       _repositorio = repositorio;
        _reserva = reserva;
        _usuario = usuario;
    }
    [Authorize]
    public IActionResult Index(int pagina = 1)
    {
        IList<Pago> lista = new List<Pago>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            int cantidadPorPagina = 10;

        // Cantidad total de registros
        int totalReservas = _repositorio.CantidadPagos();

        // Obtener los registros correspondientes a la página
        lista = _repositorio.GetPagosPaginados(pagina, cantidadPorPagina);
        // Cantidad total de páginas
        int totalPaginas = (int)Math.Ceiling(
            (double)totalReservas / cantidadPorPagina
        );

        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;

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
            _logger.LogError(ex, "Error al obtener la lista de pagos");
            TempData["Error"] = "Ocurrio un error al obtener la lista de pagos";
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
    }
    [Authorize]
    public IActionResult Editar(int id)
    {   
       try
       {
        var pago = _repositorio.GetPago(id);
         return RedirectToAction("Editar", pago);
       }
       catch (System.Exception ex)
       {
        _logger.LogError(ex, "Error al cargar pago");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
       }
    }

    [Authorize]
   public IActionResult AgregarPago(int id)
    {   
        if (id ==0){
            return View();
        }
        var reserva = _reserva.GetReserva(id);
        var dias =( reserva.FechaHasta- reserva.FechaDesde).Days;
        var APagar = reserva.DatoInmueble.Precio *dias;
        var porcentual = (APagar * reserva.DatoInmueble.Porcentual)/100;
        List<string> concepto = new List<string>{"RESERVA INICIAL","PAGO TOTAL"};
        IList<Pago> lista = new List<Pago>();
        lista = _repositorio.GetPagos();
            lista = lista.Where(x => x.IdReserva == id).ToList();
        ViewBag.Concepto = concepto;    
        ViewBag.Reserva = reserva;
        ViewBag.TotalAPagar= APagar;
        ViewBag.PrecioPorcentaje = porcentual;
        ViewBag.Pagos = lista;
        var pago = new Pago();

        return View(pago);
    }
   [Authorize]
    public IActionResult Guardar(Pago pago, int? IdReserva)
    {
        if (IdReserva.HasValue)
        {
            pago.IdReserva = IdReserva.Value;
        }
        var usuario = _usuario.ObtenerPorEmail(User.Identity.Name);
        try
        {

            if (pago.Id > 0)
            {
                _repositorio.ModificarPago(pago);
                   return RedirectToAction(nameof(AgregarPago), pago.IdReserva);
            }
            else
            {
                pago.IdAlta = usuario.Id;
                pago.IdBaja = 0;
                _repositorio.AltaPago(pago);
                TempData["Mensaje"] = "Pago cargado correctamente.";
                return RedirectToAction(nameof(AgregarPago),new { id = pago.IdReserva }
        );
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al cargar pago");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
     [Authorize]
    public IActionResult EditarPago(int id, string Concepto)
    {
        try
        {
            var pago = _repositorio.GetPago(id);
        if (pago == null)
        {
            TempData["Error"] = "Pago no encontrado.";
            return NotFound();
        }
        pago.Concepto = Concepto;
        _repositorio.ModificarPago(pago);
        TempData["Mensaje"] = "Concepto modificado.";
        return RedirectToAction(nameof(AgregarPago),new { id = pago.IdReserva });
        }
        catch (System.Exception ex)
        {
             _logger.LogError(ex, "Error al cargar pago");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
            
        }
    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id) //Es un anulado logico
    {
        try
        {
            var usuario = _usuario.ObtenerPorEmail(User.Identity.Name);
             var pago = _repositorio.GetPago(id);
            if(pago.Anulado== true)
            {
            TempData["Mensaje"] = "Pago anulado previamente.";
            return RedirectToAction(nameof(Index));
            }
            _repositorio.EliminarPago(id, usuario.Id);        
            TempData["Mensaje"] = "El pago ha sido anulado correctamente.";
            return RedirectToAction(nameof(AgregarPago),new { id = pago.IdReserva });
        }
        catch
        {
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize]
    public IActionResult Detalles(int id)
    {
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        var p = _repositorio.GetPago(id);
        var usuarioC = _usuario.getUsuario(p.IdAlta);
        ViewBag.UsuarioC = usuarioC;
        ViewBag.Pago = p;
        if (p.IdBaja.HasValue)
        {
            var usuarioT = _usuario.getUsuario(p.IdBaja.Value);
            ViewBag.UsuarioT = usuarioT;
        }
        return View(p);
    }
    [Authorize]
    public IActionResult PagosReserva(int id)
    {
        IList<Pago> lista = new List<Pago>();
        var userRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;
        ViewBag.UserRole = userRole;
        try
        {
            lista = _repositorio.GetPagos();
            lista = lista.Where(x => x.IdReserva == id).ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Ocurrio un error al obtener la lista de pagos";
               return RedirectToAction(nameof(Index));
        }
    }
// UNA VEZ PAGADA LA MULTA SE CONCRETA LA CANCELACION DE LA RESERVA
public IActionResult ConcretarCancelacion(Pago pago, int IdReserva)
    {
        var usuario = _usuario.ObtenerPorEmail(User.Identity.Name);
        try
        {
            if(pago == null)
            {
                TempData["Error"] = "No se puede cancelar por pago inexistente.";
                return View();
            }
                pago.IdAlta = usuario.Id;
                pago.IdBaja = 0;
                _repositorio.AltaPago(pago);
                _reserva.CancelarReserva(IdReserva);
                TempData["Mensaje"] = "Se ha concretado la cancelación de reserva y su multa.";
                return RedirectToAction(nameof(AgregarPago),new { id = pago.IdReserva });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Errores en la operación");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Index));

        }
    }
}
