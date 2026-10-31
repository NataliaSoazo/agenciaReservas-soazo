using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreGeneratedDocument;
namespace agenciaReserva_soazo.Controllers;

public class ReservaController : Controller
{
    private readonly ILogger<ReservaController> _logger;
    private readonly RepositorioReserva _reserva;
    private readonly RepositorioPago _pago;
    private readonly RepositorioUsuario _usuario;
    private readonly RepositorioInquilino _inquilino;
    private readonly RepositorioInmueble _inmueble;

    public ReservaController(ILogger<ReservaController> logger,RepositorioPago pago, RepositorioReserva reserva, RepositorioUsuario usuario, RepositorioInmueble inmueble, RepositorioInquilino inquilino)
    {
        _logger = logger;
        _pago = pago;
        _reserva = reserva;
        _usuario = usuario;
        _inmueble = inmueble;
        _inquilino = inquilino;
        
    }
    [Authorize]
    public IActionResult Index(int pagina = 1)
    {
        IList<Reserva> lista = new List<Reserva>();

        try
        {
            int cantidadPorPagina = 10;

            // Cantidad total de registros
            int totalReservas =  _reserva.GetCantidadReservas();

            // Obtener los registros correspondientes a la página
            lista =  _reserva.GetReservasPaginadas(pagina, cantidadPorPagina);
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

            if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de reservas");
            TempData["Error"] = "Ocurrió un error al obtener la lista de reservas";
            return View(lista);
        }
    }
    [Authorize]
    public IActionResult Editar(int id)
    {
        if (TempData.ContainsKey("Error"))
        {
            ViewBag.Error = TempData["Error"];
        }
        ViewBag.Inmuebles = _inmueble.ObtenerDisponibles();
        ViewBag.Inquilinos = _inquilino.getInquilinos();

        // Editar una reserva existente
        if (id > 0)
        {
            Reserva? reserva = _reserva.GetReserva(id);

            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            if (reserva.Anulado)
            {
                TempData["Error"] = "No se puede modificar una reserva anulada.";
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        // Crear una nueva reserva
        return View(new Reserva
        {
            Fecha = DateTime.Today,
        });
    }
    [Authorize]
    [HttpPost]
    public IActionResult Guardar(Reserva reserva)
    {
        try
        {
            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                TempData["Error"] = "La fecha de inicio debe ser anterior a la fecha de culminación.";

                return RedirectToAction(nameof(Editar), new { id = reserva.Id }
                );
            }
            // Al editar, excluimos la reserva actual de la búsqueda.
            bool disponible = _reserva.InmuebleDisponible(
                reserva.IdInmueble,
                reserva.FechaDesde,
                reserva.FechaHasta,
                reserva.Id
            );

            if (!disponible)
            {
                TempData["Error"] = "El inmueble no está disponible para esas fechas.";

                return RedirectToAction(nameof(Editar), new { id = reserva.Id });
            }
            if (reserva.Id > 0)
            {
                _reserva.ModificarReserva(reserva);
                TempData["Mensaje"] =
                    "La reserva se modificó correctamente.";
            }
            else
            {
                var usuario = _usuario.ObtenerPorEmail(User.Identity.Name);
                reserva.IdAlta = usuario.Id;
                reserva.Anulado = false;

                var nuevareserva = _reserva.AltaReserva(reserva);
                TempData["Mensaje"] = "La reserva se creó correctamente.";
                return RedirectToAction("AgregarPago", "Pago", new { id = reserva.Id });
            }

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar la reserva");
            TempData["Error"] = "No se pudo completar la operación.";
            return RedirectToAction(nameof(Editar), new { id = reserva.Id }
            );
        }
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
        try
        {
            Reserva? reserva = _reserva.GetReserva(id);
            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }
            if (reserva.Anulado)
            {
                TempData["Error"] = "La reserva ya está anulada.";
                return RedirectToAction(nameof(Index));
            }
            var usuario = _usuario.ObtenerPorEmail(User.Identity.Name);
            _reserva.AnularReserva(id, usuario.Id);

            TempData["Mensaje"] = "La reserva ha sido anulada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al anular la reserva");
            TempData["Error"] = "No se pudo completar la anulación.";
            return RedirectToAction(nameof(Index));
        }
    }
    [Authorize]
    public IActionResult Detalles(int id)
    {
        try
        {

            ViewBag.UserRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;

            // 1. Obtener la reserva primero
            Reserva? reserva = _reserva.GetReserva(id);

            // 2. Validar inmediatamente si existe
            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            // 3. Buscar el usuario creador de forma segura
            var usuarioAlta = _usuario.getUsuario(reserva.IdAlta);
            ViewBag.UsuarioC = usuarioAlta;

            // 4. Buscar el usuario que dio de baja (si aplica)
            if (reserva.IdBaja.HasValue && reserva.IdBaja.Value > 0)
            {
                var usuarioBaja = _usuario.getUsuario(reserva.IdBaja.Value);
                ViewBag.UsuarioT = usuarioBaja;
            }

            return View(reserva);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los detalles de la reserva");
            TempData["Error"] = "Ocurrió un error al obtener los detalles de la reserva";
            return RedirectToAction(nameof(Index)); // Redirigir en lugar de return View()
        }
    }
    [Authorize]
    public IActionResult VerVigentes(DateTime fechaDesde, DateTime fechaHasta)
{
    IList<Reserva> lista = new List<Reserva>();

    ViewBag.UserRole = User.Claims
        .FirstOrDefault(c => c.Type == "Rol")?.Value;

    try
    {
        lista = _reserva.GetReservasVigentes(fechaDesde, fechaHasta);

        return View("Index", lista);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener las reservas vigentes");

        TempData["Error"] = "Ocurrió un error al obtener las reservas vigentes.";

        return View("Index", lista);
    }
}
    [Authorize]
    public IActionResult ListarReservasInmueble(int id)
    {
        IList<Reserva> lista = new List<Reserva>();
        try
        {
            lista = _reserva.GetReservas()
                .Where(x => x.IdInmueble == id)
                .ToList();
            return View("Index", lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error al obtener las reservas del inmueble");
            TempData["Error"] ="Ocurrió un error al obtener las reservas.";
            return View("Index", lista);
        }
    }

    public IActionResult Renovar(int id)
{
    try
    {
        ViewBag.Inmuebles = _inmueble.ObtenerDisponibles();
        ViewBag.Inquilinos = _inquilino.getInquilinos();
        var reserva = _reserva.GetReserva(id);

        // Crear una nueva reserva
        return View(new Reserva
        {   Id = 0,
            Fecha = DateTime.Today,
            FechaDesde = reserva.FechaHasta,
            IdInquilino = reserva.IdInquilino,
            IdInmueble = reserva.IdInmueble,
            DatoInmueble = reserva.DatoInmueble,
            Arrendatario = reserva.Arrendatario
        });
    }
    catch (Exception)
    {
         TempData["Error"] ="No se pudo renovar la reserva.";
        return RedirectToAction(nameof(Index));
    }
}


    public IActionResult TerminanEnXdias(DateTime fecha)
    {
    IList<Reserva> lista = new List<Reserva>();

    ViewBag.UserRole = User.Claims
        .FirstOrDefault(c => c.Type == "Rol")?.Value;

    try
    {
        lista = _reserva.TerminanEnXdias(fecha);

        return View("Index", lista);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al obtener las reservas que terminan en esa fecha");

        TempData["Error"] = "Ocurrió un error al obtener las reservas.";

        return View("Index", lista);
    }
    }


    [HttpPost]
    public IActionResult SolicitarCancelacion(int id, DateTime fechaCulminacion)
        {
        try
        {
            var reserva =  _reserva.GetReserva(id);


        if (reserva == null)
            {return NotFound();}

        if (reserva.Anulado)
            {
            TempData["Error"] = "La reserva está anulada, no se puede cancelar.";
            return RedirectToAction("Index");
            }

        int diasOriginales = (reserva.FechaHasta.Date - reserva.FechaDesde.Date).Days;
        int diasCumplidos = (fechaCulminacion.Date - reserva.FechaDesde.Date).Days;

        if (diasOriginales <= 0 ||
            diasCumplidos <= 0 ||
            diasCumplidos >= diasOriginales)
        {
            TempData["Error"] = "La fecha de terminación no es válida.";
            return RedirectToAction("Index");
        }

        double porcentajeCumplido =(double)diasCumplidos / diasOriginales;

        double alquilerRestante =reserva.Monto * (diasOriginales - diasCumplidos)/ diasOriginales;

        int porcentajeMulta =porcentajeCumplido < 0.50 ? 50 : 25;

        double multa = Math.Round(alquilerRestante * porcentajeMulta / 100, 2);

        ViewBag.Reserva = reserva;
        ViewBag.FechaCulminacion = fechaCulminacion;
        ViewBag.DiasOriginales = diasOriginales;
        ViewBag.DiasCumplidos = diasCumplidos;
        ViewBag.AlquilerRestante = alquilerRestante;
        ViewBag.PorcentajeMulta = porcentajeMulta;
        ViewBag.TotalMulta = multa;
        return View("PagarMulta");
        }
        catch (System.Exception ex)
        {
         _logger.LogError(ex, "Error al obtener las reservas que terminan en esa fecha");

        TempData["Error"] = "Ocurrió un error al obtener las reservas.";

        return View("Index");
        }
    }
}



