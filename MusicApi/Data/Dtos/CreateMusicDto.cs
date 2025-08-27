using System.ComponentModel.DataAnnotations;

namespace MusicApi.Data.Dtos
{
    public class CreateMusicDto
    {
        
        [Required(ErrorMessage = "Enter the name of the song")]
        public string? NameMusic { get; set; }

        [Required(ErrorMessage = "Enter the name of the artist")]
        public string? Artist { get; set; }

        [Required(ErrorMessage = "Enter the musical genre")]
        public string? Genre { get; set; }
    }
}
