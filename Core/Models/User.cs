namespace Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }  // Este campo almacenará el password en formato hash
        public bool IsActive { get; set; }
    }
}
