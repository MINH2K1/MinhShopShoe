using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShopShoe.WebApi.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="User  Name not null")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
