using AgendaEscolar.Modelos;

namespace AgendaEscolar.Vistas
{
    public partial class InicioPage : ContentPage
    {
        public InicioPage()
        {
            InitializeComponent();
        }

        // Se ejecuta cuando aparece la pantalla
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await CargarActividadesAsync();
        }

        // Consultar y mostrar las actividades pendientes
        private async Task CargarActividadesAsync()
        {
            try
            {
                // Inicializar la base de datos
                await App.BaseDatos.InicializarAsync();

                // Obtener todas las actividades guardadas
                List<Actividad> actividades =
                    await App.BaseDatos.ObtenerActividadesAsync();

                // Filtrar las pendientes y ordenarlas por fecha de entrega
                var actividadesPendientes = actividades
                    .Where(a => !a.Completada)
                    .OrderBy(a => a.FechaEntrega)
                    .ToList();

                // Mostrar las actividades en CollectionView
                ListaActividades.ItemsSource = actividadesPendientes;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudieron cargar las actividades: " + ex.Message,
                    "Aceptar"
                );
            }
        }

        // Abrir el formulario para registrar una actividad
        private async void OnNuevaActividadClicked(
            object sender,
            EventArgs e)
        {
            await Shell.Current.GoToAsync(
                nameof(NuevaActividadPage)
            );
        }

        // Abrir el detalle de la actividad seleccionada
        private async void OnActividadSeleccionada(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Actividad actividad)
            {
                // Quitar la selección para permitir seleccionarla nuevamente
                ListaActividades.SelectedItem = null;

                // Enviar el identificador a la pantalla de detalle
                await Shell.Current.GoToAsync(
                    nameof(DetalleActividadPage),
                    new Dictionary<string, object>
                    {
                        { "id", actividad.Id }
                    }
                );
            }
        }
    }
}