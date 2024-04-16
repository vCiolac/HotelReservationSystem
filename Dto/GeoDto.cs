namespace TrybeHotel.Dto
{
   public class GeoDto
   {
      public string? Address { get; set; }
      public string? City { get; set; }
      public string? State { get; set; }
   }

   public class GeoDtoResponse
   {
      public string? lat { get; set; }
      public string? lon { get; set; }
   }

   public class GeoDtoHotelResponse
   {
      public int HotelId { get; set; }
      public string? Name { get; set; }
      public string? Address { get; set; }
      public string? CityName { get; set; }
      public string? State { get; set; }
      public int Distance { get; set; }
   }
   public class SearchResponse
   {
      public int PlaceId { get; set; }
      public string? Licence { get; set; }
      public string? OsmType { get; set; }
      public int OsmId { get; set; }
      public List<string>? Boundingbox { get; set; }
      public string? Lat { get; set; }
      public string? Lon { get; set; }
      public string? DisplayName { get; set; }
      public string? Class { get; set; }
      public string? Type { get; set; }
      public double Importance { get; set; }
   }
}