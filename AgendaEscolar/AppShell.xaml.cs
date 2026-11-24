using AgendaEscolar.Vistas;

namespace AgendaEscolar
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(
                nameof(NuevaActividadPage),
                typeof(NuevaActividadPage)
            );

            Routing.RegisterRoute(
                nameof(DetalleActividadPage),
                typeof(DetalleActividadPage)
            );
        }
    }
}