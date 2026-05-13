using System.ComponentModel.DataAnnotations;
namespace FullTractor.Application.DTOs.Category.Request;

public class CreateCategoryRequest
{
    [Required(ErrorMessage = "Category is required to have a name.")]
    [MinLength(5, ErrorMessage = "Category name has to have more than 5 characters.")]
    [MaxLength(25, ErrorMessage = "Category name has to have less than 25 characters.")]
    [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Category name has to be only letters")]
    public required string Name { get; set; }
}