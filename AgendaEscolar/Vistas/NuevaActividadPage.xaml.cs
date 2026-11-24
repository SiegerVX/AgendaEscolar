using AgendaEscolar.Modelos;

namespace AgendaEscolar.Vistas
{
    public partial class NuevaActividadPage : ContentPage
    {
        public NuevaActividadPage()
        {
            InitializeComponent();

            // Establecer la fecha actual como fecha de asignación
            SelectorFechaAsignacion.Date = DateTime.Today;

            // Establecer inicialmente la fecha de entrega para mañana
            SelectorFechaEntrega.Date = DateTime.Today.AddDays(1);
        }

        // Guardar una nueva actividad
        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            try
            {
                // Obtener los datos escritos en el formulario
                string nombre = EntradaNombre.Text?.Trim() ?? "";
                string materia = EntradaMateria.Text?.Trim() ?? "";
                string descripcion = EntradaDescripcion.Text?.Trim() ?? "";

                // Validar los campos obligatorios
                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(materia) ||
                    string.IsNullOrWhiteSpace(descripcion))
                {
                    await DisplayAlert(
                        "Datos incompletos",
                        "Completa el nombre, la materia y la descripción.",
                        "Aceptar"
                    );

                    return;
                }

                // Validar que la fecha de entrega no sea anterior
                // a la fecha de asignación
                if (SelectorFechaEntrega.Date < SelectorFechaAsignacion.Date)
                {
                    await DisplayAlert(
                        "Fecha incorrecta",
                        "La fecha de entrega no puede ser anterior a la fecha de asignación.",
                        "Aceptar"
                    );

                    return;
                }

                // Crear el objeto con los datos del formulario
                Actividad nuevaActividad = new Actividad
                {
                    Nombre = nombre,
                    Materia = materia,
                    FechaAsignacion = SelectorFechaAsignacion.Date,
                    FechaEntrega = SelectorFechaEntrega.Date,
                    Descripcion = descripcion,
                    Completada = false
                };

                // Asegurar que la base de datos esté inicializada
                await App.BaseDatos.InicializarAsync();

                // Guardar la actividad en SQLite
                await App.BaseDatos.AgregarActividadAsync(nuevaActividad);

                await DisplayAlert(
                    "Actividad guardada",
                    "La actividad se registró correctamente.",
                    "Aceptar"
                );

                // Regresar a la pantalla principal
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudo guardar la actividad: " + ex.Message,
                    "Aceptar"
                );
            }
        }

        // Cancelar el registro y regresar
        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}