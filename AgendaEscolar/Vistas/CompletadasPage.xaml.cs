
using AgendaEscolar.Modelos;

namespace AgendaEscolar.Vistas
{
    public partial class CompletadasPage : ContentPage
    {
        public CompletadasPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarActividadesCompletadasAsync();
        }

        private async Task CargarActividadesCompletadasAsync()
        {
            try
            {
                await App.BaseDatos.InicializarAsync();

                List<Actividad> actividades =
                    await App.BaseDatos.ObtenerActividadesAsync();

                var actividadesCompletadas = actividades
                    .Where(a => a.Completada)
                    .OrderByDescending(a => a.FechaEntrega)
                    .ToList();

                ListaCompletadas.ItemsSource = actividadesCompletadas;
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

        private async void OnActividadSeleccionada(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Actividad actividad)
            {
                ListaCompletadas.SelectedItem = null;

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