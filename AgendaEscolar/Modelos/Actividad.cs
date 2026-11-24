using SQLite;

namespace AgendaEscolar.Modelos
{
    public class Actividad
    {
        // Identificador único de cada actividad
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // Nombre de la actividad escolar
        public string Nombre { get; set; } = string.Empty;

        // Materia a la que pertenece
        public string Materia { get; set; } = string.Empty;

        // Día en que se dejó la actividad
        public DateTime FechaAsignacion { get; set; }

        // Fecha límite para entregar
        public DateTime FechaEntrega { get; set; }

        // Explicación de lo que se debe realizar
        public string Descripcion { get; set; } = string.Empty;

        // Indica si la actividad ya fue terminada
        public bool Completada { get; set; } = false;
    }
}
