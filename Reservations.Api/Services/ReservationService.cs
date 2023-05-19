using Reservations.Api.Managers;
using Reservations.Api.Models;

namespace Reservations.Api.Services
{
    public interface IReservationService
    {
        PriceResponse GetParkingPriceForDateRange(PriceRequest request);
        ReservationResponse ReserveParking(ReservationRequest request);
        ReservationCancellationResponse CancelReservation(ReservationCancellationRequest request);
        AvailabilityResponse GetSpaceAvailabilities(AvailabilityRequest request);
        ReservationResponse AmendReservation(ReservationRequest request);
    }

    public class ReservationService : IReservationService
    {
        private readonly IReservationManager _manager;

        public ReservationService(IReservationManager manager)
        {
            _manager = manager;
        }

        public PriceResponse GetParkingPriceForDateRange(PriceRequest request)
        {
            var result = _manager.GetParkingPricesForDateRange(request.From, request.To);

            return new PriceResponse()
            {
                From = request.From,
                To = request.To,
                Prices = result
            };
        }

        public ReservationResponse ReserveParking(ReservationRequest request)
        {
            ReservationResponse response = new()
            {
                From = request.From,
                To = request.To,
                Name = request.Name,
            };

            try
            {
                _manager.ReserveParking(request.From, request.To, request.Name);
                response.Status = "Reserved";
            }
            catch (Exception)
            {
                response.Status = $"Unable to reserved reservation due to insufficient spaces available for the given date range {request.From} - {request.To}";
            }

            return response;
        }

        public ReservationCancellationResponse CancelReservation(ReservationCancellationRequest request)
        {
            var response = new ReservationCancellationResponse()
            {
                Name = request.Name,
            };

            try
            {
                _manager.CancelReservation(request.Name);
                response.Status = "Cancelled";
            }
            catch (Exception)
            {
                response.Status = "Unable to find reservation!";
            }

            return response;

        }

        public AvailabilityResponse GetSpaceAvailabilities(AvailabilityRequest request)
        {
            return new AvailabilityResponse
            {
                Spaces = _manager.GetSpaceAvailabilities(request.From, request.To),
                From = request.From,
                To = request.To
            };
        }

        public ReservationResponse AmendReservation(ReservationRequest request)
        {
            ReservationResponse response = new()
            {
                From = request.From,
                To = request.To,
                Name = request.Name,
            };

            try
            {
                _manager.AmendReservation(request.From, request.To, request.Name);
                response.Status = "Reservation Amended";
            }
            catch (Exception)
            {
                response.Status =
                    $"Unable to amend reservation due to insufficient spaces available for the given date range {request.From} - {request.To}";
            }

            return response;
        }
    }
}
