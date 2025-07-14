using System.ComponentModel.DataAnnotations;

namespace OIT_Reservation.Models
{
    public class RoomType
    {
        public int RoomTypeID { get; set; }

        [Required(ErrorMessage = "RoomTypeCode is required")]
        [StringLength(300, ErrorMessage = "RoomTypeCode max length is 300")]
        public string RoomTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(300, ErrorMessage = "Description max length is 300")]
        public string Description { get; set; } = null!;

        public string? Remarks { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
