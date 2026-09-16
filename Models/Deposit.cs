using System.ComponentModel.DataAnnotations;

namespace ASP.NetLearning.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount {  get; set; }

        [Required]
        [StringLength(50)]
        public string Form { get; set; }
    }
}
