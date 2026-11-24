using AgendaEscolar.Datos;

namespace AgendaEscolar
{
    public partial class App : Application
    {
        // Servicio de base de datos disponible para toda la aplicación
        public static ServicioBaseDatos BaseDatos { get; private set; } = null!;

        public App()
        {
            InitializeComponent();

            // Ubicación del archivo de SQLite
            string rutaBaseDatos = Path.Combine(
                FileSystem.AppDataDirectory,
                "agenda_escolar.db3"
            );

            // Crear el servicio de base de datos
            BaseDatos = new ServicioBaseDatos(rutaBaseDatos);

            // Mostrar la navegación principal
            MainPage = new AppShell();
        }
    }
}