namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // 1. Implemente as models da aplicação
    public class City {
        public int CityId { get; set; }

        public string? Name { get; set; }

        public virtual List<Hotel>? Hotels { get; set; }

        public string? State { get; set; }

    }
}