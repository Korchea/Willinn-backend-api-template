namespace Core.Models
{
    public class User
    {
        // Identificador único del usuario
        public int Id { get; set; }

        // Nombre del usuario (requerido)
        public required string Name { get; set; }

        // Correo electrónico del usuario (requerido)
        public required string Email { get; set; }

        // Contraseña del usuario en formato hash (requerido)
        public required string Password { get; set; }

        // Indica si el usuario está activo (por defecto será true o false según el estado)
        public bool IsActive { get; set; }
    }
}
