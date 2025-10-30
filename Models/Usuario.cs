namespace Eldertech.Models
{
    public class Usuario
    {
        public int IDUsuario { get; set; }
        public string UsuarioNombre { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public DateTime FechaNacimiento { get; set; }
    }
}
