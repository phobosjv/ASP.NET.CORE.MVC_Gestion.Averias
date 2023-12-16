namespace GestionIncidencias.Servicios
{
    static public class Constantes
    {
        public const string RolAdmin = "admin";

        public static string asignarColor(int categoriaId)
        {
            string[] Colores = {"#FF00FF", "#FF0000", "#FFFF00", "#00FF00", "#00FFFF",
                                   "#000080", "#800080", "#800000", "#808000", "#008000",
                                   "#008080", "#C0C0C0", "#6495ED", "#8B008B", "#8B0000",
                                   "#FFD700", "#556B2F", "#1E90FF", "#FFE4B5", "#0000FF"};
            return Colores[categoriaId % 20];
        }
    }
}
