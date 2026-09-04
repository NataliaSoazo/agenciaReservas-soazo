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
            else if (TempData.ContainsKey("Error"))
            {
                ViewBag.Error = TempData["Error"];
            }

            return View(lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error al obtener la lista de reservas");

            TempData["Error"] ="Ocurrió un error al obtener la lista de reservas";

            ViewBag.Error = TempData["Error"];

            return View(lista);
        }
    }


    
    public IActionResult Editar(int id)
    {
        RepositorioReserva repoReserva =
            new RepositorioReserva();

        var reservas = repoReserva.GetReservas();

        RepositorioInmueble repoInmueble = new RepositorioInmueble();

        ViewBag.inmuebles = repoInmueble.ObtenerTodos();

        RepositorioInquilino repoInquilino = new RepositorioInquilino();

        ViewBag.Inquilinos = repoInquilino.GetInquilinos();

        // EDITAR
        if (id > 0)
        {
            var reserva =
                repoReserva.GetReserva(id);

            if (reserva == null)
            {
                TempData["Error"] =
                    "Reserva no encontrada.";

                return RedirectToAction(nameof(Index));
            }

            if (reserva.Anulado == true)
            {
                TempData["Error"] =
                    "No se puede modificar una reserva anulada.";

                return RedirectToAction(nameof(Index));
            }

            return View(reserva);
        }


        // CREAR
        return View();
    }


    
    [HttpPost]
   
    public IActionResult Guardar(Reserva reserva)
    {
        try
        {
            RepositorioReserva rr =new RepositorioReserva();

            if (reserva.FechaDesde >= reserva.FechaHasta)
            {
                TempData["Error"] ="La fecha de inicio debe ser anterior a la fecha de culminación.";

                return RedirectToAction(nameof(Editar));
            }


            // VALIDAR DISPONIBILIDAD DEL INMUEBLE
            bool disponible =
                rr.InmuebleDisponible(
                    reserva.IdInmueble,
                    reserva.FechaDesde,
                    reserva.FechaHasta
                );

            if (!disponible)
            {
                TempData["Error"] ="El inmueble no está disponible para esas fechas.";

                return RedirectToAction(nameof(Editar));
            }


            // MODIFICAR
            if (reserva.Id > 0)
            {
                rr.ModificarReserva(reserva);

                TempData["Mensaje"] =
                    "La reserva se modificó correctamente.";

                return RedirectToAction(nameof(Index));
            }


            // ALTA
            reserva.Anulado = false;

            rr.AltaReserva(reserva);

            TempData["Mensaje"] =
                "La reserva se creó correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar la reserva");

            TempData["Error"] ="No se pudo completar la operación.";

            return RedirectToAction(nameof(Index));
        }
    }

    public IActionResult Eliminar(int id)
    {
        try
        {
            RepositorioReserva rr = new RepositorioReserva();

            var reserva = rr.GetReserva(id);

            if (reserva == null)
            {
                TempData["Error"] ="Reserva no encontrada.";

                return RedirectToAction(nameof(Index));
            }

            if (reserva.Anulado)
            {
                TempData["Error"] ="La reserva ya está anulada.";

                return RedirectToAction(nameof(Index));
            }

            rr.AnularReserva(id);

            TempData["Mensaje"] ="La reserva ha sido anulada correctamente.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error al anular la reserva");

            TempData["Error"] =
                "No se pudo completar la anulación.";

            return RedirectToAction(nameof(Index));
        }
    }


   
    public IActionResult Detalles(int id)
    {
        RepositorioReserva rr =
            new RepositorioReserva();

        var userRole = User.Claims
            .FirstOrDefault(c => c.Type == "Rol")?.Value;

        ViewBag.UserRole = userRole;

        var reserva =
            rr.GetReserva(id);

        if (reserva == null)
        {
            TempData["Error"] =
                "Reserva no encontrada.";

            return RedirectToAction(nameof(Index));
        }

        return View(reserva);
    }


    public IActionResult VerVigentes()
    {
        RepositorioReserva rr =
            new RepositorioReserva();

        IList<Reserva> lista =
            new List<Reserva>();

        var userRole = User.Claims
            .FirstOrDefault(c => c.Type == "Rol")?.Value;

        ViewBag.UserRole = userRole;

        try
        {
            lista = rr.GetReservas();

            lista = lista
                .Where(x =>
                    x.FechaHasta > DateTime.Now &&
                    x.Anulado == false
                )
                .ToList();

            return View("Index", lista);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al obtener las reservas vigentes"
            );

            TempData["Error"] =
                "Ocurrió un error al obtener las reservas vigentes";

            return View(lista);
        }
    }


   
    public IActionResult ListarReservasInmueble(int id)
    {
        RepositorioReserva rr =
            new RepositorioReserva();

        IList<Reserva> lista =
            new List<Reserva>();

        try
        {
            lista = rr.GetReservas();

            lista = lista
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

            return View(lista);
        }
    }
}