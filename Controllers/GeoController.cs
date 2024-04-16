using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;


namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("geo")]
    public class GeoController : Controller
    {
        private readonly IHotelRepository _repository;
        private readonly IGeoService _geoService;


        public GeoController(IHotelRepository repository, IGeoService geoService)
        {
            _repository = repository;
            _geoService = geoService;
        }

        // 11. Desenvolva o endpoint GET /geo/status
        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetStatus()
        {
            var status = await _geoService.GetGeoStatus();
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        // 12. Desenvolva o endpoint GET /geo/address
        [HttpGet]
        [Route("address")]
        public async Task<IActionResult> GetHotelsByLocation([FromBody] GeoDto address)
        {
            try{
                var hotels = await _geoService.GetHotelsByGeo(address, _repository);
                if (hotels == null)
                {
                    return NotFound();
                }
                return Ok(hotels);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }


}