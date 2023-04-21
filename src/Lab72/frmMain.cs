using Lab61.Data;

namespace Lab62;

public partial class frmMain : Form
{
    private readonly PuntajesDbContext _context;
    private Dictionary<string, (int, int)> _ranges;

    public frmMain()
    {
        _context = new PuntajesDbContext();
        _ranges = new Dictionary<string, (int, int)>
        {
            { "De 40 a 31", (40, 31) },
            { "De 30 a 21", (30, 21) },
            { "De 20 a 11", (20, 11) },
            { "De 10 a 0", (10, 0) },
        };

        InitializeComponent();
    }

    private void btnCargar_Click(object sender, EventArgs e)
    {
        CargarDatos();
    }

    private void CargarDatos()
    {
        var (max, min) = _ranges[cbCalificacionRange.SelectedValue!.ToString()!];
        var actividades = _context.Actividades.Where(p =>
            p.Descripcion == cbTarea.SelectedValue!.ToString() && (p.Puntaje >= min && p.Puntaje <= max)).ToList();
        lvData.Items.Clear();
        lvData.Items.AddRange(actividades.Select(p => new ListViewItem(new[]
        {
            p.FechaActividad.ToString("dd/MM/yyyy"), p.Estudiante, p.Descripcion, p.Puntaje.ToString(), p.Id.ToString()
        })).ToArray());
    }

    private void btnLimpiar_Click(object sender, EventArgs e)
    {
        lvData.Items.Clear();
        tbIdActividad.Text = string.Empty;
        tbEstudiante.Text = string.Empty;
        tbActividad.Text = string.Empty;
        tbPuntuacion.Text = string.Empty;
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        CargarOpciones();
    }

    private void CargarOpciones()
    {
        var tareas = _context.Actividades.Select(p => p.Descripcion).Distinct().ToList();
        cbTarea.DataSource = tareas;

        var ranges = _ranges.Keys.ToList();
        cbCalificacionRange.DataSource = ranges;
    }

    private void lvData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
        var item = new ActividadEntity
        {
            Id = int.Parse(e.Item?.SubItems[4].Text ?? string.Empty),
            Estudiante = e.Item?.SubItems[1].Text ?? string.Empty,
            Descripcion = e.Item?.SubItems[2].Text ?? string.Empty,
            Puntaje = int.Parse(e.Item?.SubItems[3].Text ?? string.Empty),
        };

        tbIdActividad.Text = item.Id.ToString();
        tbEstudiante.Text = item.Estudiante;
        tbActividad.Text = item.Descripcion;
        tbPuntuacion.Text = item.Puntaje.ToString();
    }

    private void btnSalir_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void btnActualizar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tbIdActividad.Text))
        {
            MessageBox.Show("Favor de seleccionar la actividad", "Information", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        if (string.IsNullOrEmpty(tbEstudiante.Text) || string.IsNullOrEmpty(tbActividad.Text) ||
            string.IsNullOrEmpty(tbPuntuacion.Text))
        {
            MessageBox.Show("Todos los campos deben estar llenos", "Warning", MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        if (int.TryParse(tbPuntuacion.Text, out int puntaje) && (puntaje < 0 || puntaje > 40))
        {
            MessageBox.Show("La puntuación debe ser un numero entre 0 y 40", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        var idActividad = int.Parse(tbIdActividad.Text);
        var actividad = _context.Actividades.Single(a => a.Id == idActividad);
        actividad.Descripcion = tbActividad.Text;
        actividad.Estudiante = tbEstudiante.Text;
        actividad.Puntaje = puntaje;
        _context.SaveChanges();

        CargarOpciones();
        CargarDatos();
    }
}
