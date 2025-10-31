namespace Eldertech.Models
{
    public class Usuario
    {
        public int IDUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
