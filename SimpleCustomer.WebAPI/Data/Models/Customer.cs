using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleCustomer.WebAPI.Data.Models
{
    [Table("Customers")]
    public class Customer
    {
        [Key] [Required]
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string PermanentAddress { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
