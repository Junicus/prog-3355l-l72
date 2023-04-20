using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Lab61.Data;

public class PuntajesDbContext : DbContext
{
    public DbSet<Pueblo> Pueblos => Set<Pueblo>();
    public DbSet<ProfesorEntity> Profesores => Set<ProfesorEntity>();
    public DbSet<EstudianteEntity> Estudiantes => Set<EstudianteEntity>();
    public DbSet<ActividadEntity> Actividades => Set<ActividadEntity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseJet(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=puntajes.accdb");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pueblo>(p =>
        {
            p.Property(e => e.Name).HasColumnName("Pueblo");
        });

        modelBuilder.Entity<ProfesorEntity>(p =>
        {
            p.ToTable("Profesor");
        });

        modelBuilder.Entity<EstudianteEntity>(p =>
        {
            p.Property(e => e.Id).HasColumnName("IdEstudiante");
            p.Property(e => e.ZipCode).HasColumnName("Zip Code");
        });

        modelBuilder.Entity<ActividadEntity>(p =>
        {
            p.Property(e => e.Id).HasColumnName("IdActividad");
            p.Property(e => e.Estudiante).HasColumnName("IdEstudiante");
        });

        base.OnModelCreating(modelBuilder);
    }
}

public class Pueblo
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class EstudianteEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    public string Pueblo { get; set; }
    public string ZipCode { get; set; }
}

public class ProfesorEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public int Edad { get; set; }
    public string Pueblo { get; set; }
}

public class ActividadEntity
{
    public int Id { get; set; }
    public string Estudiante { get; set; }
    public string Descripcion { get; set; }
    public DateTime FechaActividad { get; set; }
    public int Puntaje { get; set; }
}