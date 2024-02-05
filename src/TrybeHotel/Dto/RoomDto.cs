namespace TrybeHotel.Dto {
     public class RoomDto {
          public int roomId { get; set; }
          public string name { get; set; } = null!;
          public int capacity { get; set; }
          public string image { get; set; } = null!;
          public HotelDto hotel { get; set; } = null!;
     }
}