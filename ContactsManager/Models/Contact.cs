using ContactsManager.Extentions;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ContactsManager.Models
{
    /// <summary>
    /// Represents Contacts table (its columns) and maps the data from the database to the .NET Core
    /// </summary>
    [Table("Contact")]
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required"), StringLength(40, MinimumLength = 2, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required"), StringLength(40, MinimumLength = 2, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        public string Surname { get; set; }

        [Array(AllowableValues = new[] { "M", "F" }, ErrorMessage = "{0} must be either 'M' or 'F'.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        public string Phone { get; set; }

        [RangeDate(ErrorMessage = "{0} must be between {1:dd/MM/yyyy} and {2:dd/MM/yyyy}")]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Career is required"), StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must contain from  {2} to {1} characters.")]
        public string Career { get; set; }

        [MaxLength(300, ErrorMessage = "Comment must not contain more than 300 characters.")]
        public string Comment { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ArrayAttribute : ValidationAttribute
    {
        public string[] AllowableValues { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (AllowableValues?.Contains(value?.ToString()) == true)
            {
                return ValidationResult.Success;
            }

            var msg = $"Please enter one of the allowable values: {string.Join(", ", (AllowableValues ?? new string[] { "No allowable values found" }))}.";
            return new ValidationResult(msg);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RangeDateAttribute : RangeAttribute
    {
        public RangeDateAttribute()
            : base(typeof(DateTime),
            DateTime.Now.AddYears(-100).ToShortDateString(), DateTime.Today.ToShortDateString())
        { }
    }
}
