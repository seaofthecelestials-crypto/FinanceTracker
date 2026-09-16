using System.ComponentModel.DataAnnotations;

namespace ASP.NetLearning.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength (100)]
        public string Email { get; set; }

        [Required]
        [StringLength (200)]
        public string Subject { get; set; }

        [Required]
        [StringLength (1000)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
