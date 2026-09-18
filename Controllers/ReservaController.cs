using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;
using agenciaReservas_soazo.Repositorios;
using Microsoft.AspNetCore.Authorization;
namespace agenciaReserva_soazo.Controllers;

public class ReservaController : Controller
{
    private readonly ILogger<ReservaController> _logger;

    public ReservaController(ILogger<ReservaController> logger)
    {
        _logger = logger;
    }
    [Authorize]
    public IActionResult Index(int pagina = 1)
    {
        RepositorioReserva rr = new RepositorioReserva();
        IList<Reserva> lista = new List<Reserva>();

        try
        {
            int cantidadPorPagina = 10;

            // Cantidad total de registros
            int totalReservas = rr.GetCantidadReservas();

            // Obtener los registros correspondientes a la página
            lista = rr.GetReservasPaginadas(pagina, cantidadPorPagina);
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

        RepositorioInmueble repoInmueble = new RepositorioInmueble();
        RepositorioInquilino repoInquilino = new RepositorioInquilino();
        RepositorioReserva repoReserva = new RepositorioReserva();

        ViewBag.Inmuebles = repoInmueble.ObtenerTodos();
        ViewBag.Inquilinos = repoInquilino.getInquilinos();

        // Editar una reserva existente
        if (id > 0)
        {
            Reserva? reserva = repoReserva.GetReserva(id);

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
            FechaDesde = DateTime.Today,
            FechaHasta = DateTime.Today
        });
    }
    [Authorize]
    [HttpPost]
    public IActionResult Guardar(Reserva reserva)
    {
        try
        {
            RepositorioReserva rr = new RepositorioReserva();
            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                TempData["Error"] = "La fecha de inicio debe ser anterior a la fecha de culminación.";

                return RedirectToAction(nameof(Editar), new { id = reserva.Id }
                );
            }
            // Al editar, excluimos la reserva actual de la búsqueda.
            bool disponible = rr.InmuebleDisponible(
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
                rr.ModificarReserva(reserva);
                TempData["Mensaje"] =
                    "La reserva se modificó correctamente.";
            }
            else
            {
                reserva.Anulado = false;
                rr.AltaReserva(reserva);
                TempData["Mensaje"] =
                    "La reserva se creó correctamente.";
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
            RepositorioReserva rr = new RepositorioReserva();
            Reserva? reserva = rr.GetReserva(id);
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

            rr.AnularReserva(id);

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
            RepositorioReserva rr = new RepositorioReserva();
            RepositorioUsuario ru = new RepositorioUsuario();

            ViewBag.UserRole = User.Claims.FirstOrDefault(c => c.Type == "Rol")?.Value;

            // 1. Obtener la reserva primero
            Reserva? reserva = rr.GetReserva(id);

            // 2. Validar inmediatamente si existe
            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Index));
            }

            // 3. Buscar el usuario creador de forma segura
            var usuarioAlta = ru.getUsuario(reserva.IdAlta);
            ViewBag.UsuarioC = usuarioAlta;

            // 4. Buscar el usuario que dio de baja (si aplica)
            if (reserva.IdBaja.HasValue && reserva.IdBaja.Value > 0)
            {
                var usuarioBaja = ru.getUsuario(reserva.IdBaja.Value);
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
    public IActionResult VerVigentes()
    {
        RepositorioReserva rr = new RepositorioReserva();
        IList<Reserva> lista = new List<Reserva>();
        ViewBag.UserRole = User.Claims
            .FirstOrDefault(c => c.Type == "Rol")?.Value;
        try
        {
            lista = rr.GetReservas()
                .Where(x =>
                    x.FechaHasta > DateTime.Now &&
                    !x.Anulado
                )
                .ToList();

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
        RepositorioReserva rr = new RepositorioReserva();
        IList<Reserva> lista = new List<Reserva>();
        try
        {
            lista = rr.GetReservas()
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
}



