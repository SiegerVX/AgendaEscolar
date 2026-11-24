using SQLite;
using AgendaEscolar.Modelos;

namespace AgendaEscolar.Datos
{
    public class ServicioBaseDatos
    {
        private readonly SQLiteAsyncConnection _baseDatos;

        // Recibe la ubicación donde se guardará el archivo SQLite
        public ServicioBaseDatos(string rutaBaseDatos)
        {
            _baseDatos = new SQLiteAsyncConnection(rutaBaseDatos);
        }

        // Inicializa la base de datos y crea la tabla si no existe
        public async Task InicializarAsync()
        {
            await _baseDatos.CreateTableAsync<Actividad>();
        }

        // CREATE: Agregar una actividad
        public async Task<int> AgregarActividadAsync(Actividad actividad)
        {
            return await _baseDatos.InsertAsync(actividad);
        }

        // READ: Consultar todas las actividades
        public async Task<List<Actividad>> ObtenerActividadesAsync()
        {
            return await _baseDatos.Table<Actividad>().ToListAsync();
        }

        // READ: Consultar una actividad por su identificador
        public async Task<Actividad?> ObtenerActividadPorIdAsync(int id)
        {
            return await _baseDatos.Table<Actividad>()
                                   .Where(a => a.Id == id)
                                   .FirstOrDefaultAsync();
        }

        // UPDATE: Modificar una actividad
        public async Task<int> ActualizarActividadAsync(Actividad actividad)
        {
            return await _baseDatos.UpdateAsync(actividad);
        }

        // DELETE: Eliminar una actividad
        public async Task<int> EliminarActividadAsync(Actividad actividad)
        {
            return await _baseDatos.DeleteAsync(actividad);
        }
    }
}