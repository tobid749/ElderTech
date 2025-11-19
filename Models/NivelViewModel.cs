using System.Collections.Generic;

namespace Eldertech.Models
{
    public class NivelViewModel
{
    public int NumeroNivel { get; set; }
    public string TituloApp { get; set; }
    public string Pregunta { get; set; }

    public List<Opcion> Opciones { get; set; }

    public int PorcentajeCompletado { get; set; }
    public int IndexPregunta { get; set; }
    public int CorrectasHastaAhora { get; set; }
}


    public class Opcion
    {
        public string Texto { get; set; }
        public bool EsCorrecta { get; set; }
    }
}
