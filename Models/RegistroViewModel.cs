using System.ComponentModel.DataAnnotations;

namespace SaltinasMvc.Models;

/// <summary>
/// Modelo del formulario de registro. Las anotaciones (DataAnnotations)
/// definen las reglas de validación que se comprueban en el servidor (ModelState)
/// y, con jQuery Validation, también en el navegador.
/// </summary>
public class RegistroViewModel
{
    [Required(ErrorMessage = "Escribe tu nombre completo.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    [Display(Name = "Nombre completo")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu correo electrónico.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido, por ejemplo ejemplo@correo.com.")]
    [Display(Name = "Correo electrónico")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe tu número de celular.")]
    [RegularExpression(@"^3\d{9}$", ErrorMessage = "Ingresa un celular colombiano de 10 dígitos, por ejemplo 3001234567.")]
    [Display(Name = "Teléfono")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "Crea una contraseña.")]
    [StringLength(40, MinimumLength = 6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Clave { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma tu contraseña.")]
    [Compare(nameof(Clave), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    public string ConfirmarClave { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecciona tu fecha de nacimiento.")]
    [DataType(DataType.Date)]
    [Display(Name = "Fecha de nacimiento")]
    public DateTime? FechaNacimiento { get; set; }

    [Display(Name = "Género")]
    public string? Genero { get; set; }

    [Display(Name = "Acepto los términos y condiciones")]
    public bool AceptaTerminos { get; set; }
}
