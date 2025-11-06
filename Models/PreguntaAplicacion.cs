namespace Eldertech.Models
{
    public class PreguntaAplicacion
    {
        public int IDPregunta { get; set; }
        public int IDAplicacion { get; set; }
        public string Enunciado { get; set; }
        public string Opcion1 { get; set; }
        public string Opcion2 { get; set; }
        public string Opcion3 { get; set; }
        public string Opcion4 { get; set; }
        public int Correcta { get; set; } // número 1-4
    }
}
