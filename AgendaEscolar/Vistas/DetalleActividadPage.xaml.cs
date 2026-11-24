
using AgendaEscolar.Modelos;

namespace AgendaEscolar.Vistas
{
    public partial class DetalleActividadPage : ContentPage, IQueryAttributable
    {
        private int _actividadId;

        public DetalleActividadPage()
        {
            InitializeComponent();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out object? valor) &&
                int.TryParse(valor?.ToString(), out int id))
            {
                _actividadId = id;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarActividadAsync();
        }

        private async Task CargarActividadAsync()
        {
            try
            {
                await App.BaseDatos.InicializarAsync();

                Actividad? actividad =
                    await App.BaseDatos.ObtenerActividadPorIdAsync(_actividadId);

                if (actividad == null)
                {
                    await DisplayAlert(
                        "Aviso",
                        "No se encontró la actividad.",
                        "Aceptar"
                    );

                    await Shell.Current.GoToAsync("..");
                    return;
                }

                EntradaNombre.Text = actividad.Nombre;
                EntradaMateria.Text = actividad.Materia;
                SelectorFechaAsignacion.Date = actividad.FechaAsignacion;
                SelectorFechaEntrega.Date = actividad.FechaEntrega;
                EntradaDescripcion.Text = actividad.Descripcion;

                CasillaCompletada.IsChecked = actividad.Completada;
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudo cargar la actividad: " + ex.Message,
                    "Aceptar"
                );
            }
        }

        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            try
            {
                string nombre = EntradaNombre.Text?.Trim() ?? "";
                string materia = EntradaMateria.Text?.Trim() ?? "";
                string descripcion = EntradaDescripcion.Text?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(nombre) ||
                    string.IsNullOrWhiteSpace(materia) ||
                    string.IsNullOrWhiteSpace(descripcion))
                {
                    await DisplayAlert(
                        "Datos incompletos",
                        "Completa todos los campos.",
                        "Aceptar"
                    );

                    return;
                }

                if (SelectorFechaEntrega.Date < SelectorFechaAsignacion.Date)
                {
                    await DisplayAlert(
                        "Fecha incorrecta",
                        "La fecha de entrega no puede ser anterior a la fecha de asignación.",
                        "Aceptar"
                    );

                    return;
                }

                Actividad? actividad =
                    await App.BaseDatos.ObtenerActividadPorIdAsync(_actividadId);

                if (actividad == null)
                {
                    await DisplayAlert(
                        "Aviso",
                        "La actividad ya no existe.",
                        "Aceptar"
                    );

                    return;
                }

                actividad.Nombre = nombre;
                actividad.Materia = materia;
                actividad.FechaAsignacion = SelectorFechaAsignacion.Date;
                actividad.FechaEntrega = SelectorFechaEntrega.Date;
                actividad.Descripcion = descripcion;
                actividad.Completada = CasillaCompletada.IsChecked;

                await App.BaseDatos.ActualizarActividadAsync(actividad);

                await DisplayAlert(
                    "Cambios guardados",
                    actividad.Completada
                        ? "La actividad se guardó como completada."
                        : "La actividad se actualizó correctamente.",
                    "Aceptar"
                );

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudieron guardar los cambios: " + ex.Message,
                    "Aceptar"
                );
            }
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Eliminar actividad",
                "¿Estás seguro de que deseas eliminar esta actividad?",
                "Eliminar",
                "Cancelar"
            );

            if (!confirmar)
                return;

            try
            {
                Actividad? actividad =
                    await App.BaseDatos.ObtenerActividadPorIdAsync(_actividadId);

                if (actividad != null)
                {
                    await App.BaseDatos.EliminarActividadAsync(actividad);
                }

                await DisplayAlert(
                    "Actividad eliminada",
                    "La actividad se eliminó correctamente.",
                    "Aceptar"
                );

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await DisplayAlert(
                    "Error",
                    "No se pudo eliminar la actividad: " + ex.Message,
                    "Aceptar"
                );
            }
        }

        private async void OnRegresarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}