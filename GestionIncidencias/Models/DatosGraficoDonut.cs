namespace GestionIncidencias.Models
{
    public class DatasetDonut
    {
        public string Label { get; set; }
        public int[] Data { get; set; }
        public string[] BackgroundColor { get; set; }
        public int HoverOffset { get; set; }
        public DatasetDonut() 
        {
            HoverOffset = 4;
        }
        public DatasetDonut(string label, int[] data, string[] backgroundcolor)
        {
            HoverOffset = 4;
        }
    }
    public class DatosGraficoDonut
    {
        public string[] Labels { get; set; }
        public DatasetDonut[] Datasets { get; set; }
    }
    
}
