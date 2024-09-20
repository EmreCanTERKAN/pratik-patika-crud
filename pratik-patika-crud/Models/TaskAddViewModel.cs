using System.ComponentModel.DataAnnotations;

namespace pratik_patika_crud.Models
{
    public class TaskAddViewModel
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        [MinLength(10,ErrorMessage ="İçerik 10 karakterden az olamaz..")]
        public string? Content { get; set; }
        public int OwnerId { get; set; }
    }
}
