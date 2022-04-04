using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class User
    {
        public int Id { get; set; }

    
        [Required(ErrorMessage = "{0} is required")]
        [Remote(action: "VerificaryUser", controller: "User")]                                                    
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string StandarEmail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} is required")]
        public string Pass { get; set; }

    }
}
