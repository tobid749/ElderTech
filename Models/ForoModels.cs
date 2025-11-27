namespace Eldertech.Models
{
    public class ForoMensaje
    {
        public int IDMensaje { get; set; }
        public string NombreUsuario { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Avatar { get; set; }
    }

    public class ForoRespuesta
    {
        public int IDRespuesta { get; set; }
        public int IDMensaje { get; set; }
        public string NombreUsuario { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Avatar { get; set; }
    }

    public class ForoMensajeDetalleViewModel
    {
        public ForoMensaje Mensaje { get; set; }
        public List<ForoRespuesta> Respuestas { get; set; }
    }
}
