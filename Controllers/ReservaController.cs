using Microsoft.AspNetCore.Mvc;
using agenciaReservas_soazo.Models;

namespace agenciaReserva_soazo.Controllers;

public class ReservaController : Controller
{
    private readonly ILogger<ReservaController> _logger;

    public ReservaController(ILogger<ReservaController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioReserva rr = new RepositorioReserva();
        IList<Reserva> lista = new List<Reserva>();

        try
        {
            lista = rr.GetReservas();

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

            ViewBag.Error = "Ocurrió un error al obtener la lista de reservas.";

            return View(lista);
        }
    }

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
        ViewBag.Inquilinos = repoInquilino.GetInquilinos();

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
        return View(new Reserva());
    }

    [HttpPost]
    public IActionResult Guardar(Reserva reserva)
    {
        try
        {
            RepositorioReserva rr = new RepositorioReserva();

            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                TempData["Error"] =
                    "La fecha de inicio debe ser anterior a la fecha de culminación.";

                // Si Id es 0, vuelve al formulario de alta.
                // Si Id tiene valor, vuelve a editar esa reserva.
                return RedirectToAction(
                    nameof(Editar),
                    new { id = reserva.Id }
                );
            }

            bool disponible = rr.InmuebleDisponible(
                reserva.IdInmueble,
                reserva.FechaDesde,
                reserva.FechaHasta
            );

            if (!disponible)
            {
                TempData["Error"] =
                    "El inmueble no está disponible para esas fechas.";

                return RedirectToAction(
                    nameof(Editar),
                    new { id = reserva.Id }
                );
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

            return RedirectToAction(nameof(Index));
        }
    }

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

            TempData["Mensaje"] =
                "La reserva ha sido anulada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al anular la reserva");

            TempData["Error"] =
                "No se pudo completar la anulación.";

            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Detalles(int id)
    {
        RepositorioReserva rr = new RepositorioReserva();

        ViewBag.UserRole = User.Claims
            .FirstOrDefault(c => c.Type == "Rol")?.Value;

        Reserva? reserva = rr.GetReserva(id);

        if (reserva == null)
        {
            TempData["Error"] = "Reserva no encontrada.";
            return RedirectToAction(nameof(Index));
        }

        return View(reserva);
    }

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

            TempData["Error"] =
                "Ocurrió un error al obtener las reservas vigentes.";

            return View("Index", lista);
        }
    }

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
            _logger.LogError(
                ex,
                "Error al obtener las reservas del inmueble"
            );

            TempData["Error"] =
                "Ocurrió un error al obtener las reservas.";

            return View("Index", lista);
        }
    }
}



