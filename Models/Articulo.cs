namespace Eldertech.Models
{
    public class Articulo
    {
        public int IDArticulo { get; set; }
        public DateTime Fecha { get; set; }
        public byte[]? Foto { get; set; }
        public string? FotoContentType { get; set; }
        public string Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Texto { get; set; }
        public string? Video { get; set; }
        public string? AutorNombre { get; set; }
    }
}
