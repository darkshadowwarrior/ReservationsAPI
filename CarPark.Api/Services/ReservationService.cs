using CarPark.Api.Managers;
using CarPark.Api.Models;

namespace CarPark.Api.Services
{
    public interface IReservationService
    {
        PriceResponse GetParkingPriceForDateRange(PriceRequest request);
        ReservationResponse ReserveParking(ReservationRequest request);
        ReservationCancellationResponse CancelParking(ReservationCancellationRequest request);
        AvailabilityResponse GetAvailableParking(AvailabilityRequest request);
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
            var result = _manager.GetParkingPriceForDateRange(request.From, request.To);

            return new PriceResponse()
            {
                From = request.From,
                To = request.To,
                Price = result
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
                response.Status =
                    "Unable to reserved booking due to insufficient spaces available for the give date range";
            }

            return response;
        }

        public ReservationCancellationResponse CancelParking(ReservationCancellationRequest request)
        {
            var response = new ReservationCancellationResponse()
            {
                Name = request.Name,
            };

            try
            {
                _manager.CancelParking(request.Name);
                response.Status = "Cancelled";
            }
            catch (Exception)
            {
                response.Status = "Unable to cancel reservation";
            }

            return response;

        }

        public AvailabilityResponse GetAvailableParking(AvailabilityRequest request)
        {
            return new AvailabilityResponse
            {
                Spaces = _manager.GetAvailableParking(request.From, request.To),
                From = request.From,
                To = request.To
            };
        }
    }
}
