namespace GestionIncidencias.Models
{
    public class ElementoGraficoLineas
    {
        public string Label { get; set; }
        public int[] Data { get; set; }
        public bool Fill { get; set; }
        public string BorderColor { get; set; }
        public float Tension { get; set; }
        public ElementoGraficoLineas()
        {
            Fill = false;
            Tension = 0.1f;
        }
    }
}
