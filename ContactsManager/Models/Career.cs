using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManager.Models
{
    /// <summary>
    /// Represents Career table (its columns) and maps the data from the database to the .NET Core
    /// </summary>
    [Table("Career")]
    public class Career
    {
        [Key]
        [Required(ErrorMessage = "Name is required"), StringLength(40, MinimumLength = 2, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        public string Name { get; set; }
    }
}
