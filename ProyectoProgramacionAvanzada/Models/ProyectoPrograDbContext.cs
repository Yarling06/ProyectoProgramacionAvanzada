using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoProgramacionAvanzada.Models;

public partial class ProyectoPrograDbContext : DbContext
{
    public ProyectoPrograDbContext()
    {
    }

    public ProyectoPrograDbContext(DbContextOptions<ProyectoPrograDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadosTarea> EstadosTareas { get; set; }

    public virtual DbSet<LogsEjecucion> LogsEjecucions { get; set; }

    public virtual DbSet<PrioridadesTarea> PrioridadesTareas { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=Yeray1;Database=ProyectoPrograDB;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadosTarea>(entity =>
        {
            entity.HasKey(e => e.EstadoID).HasName("PK__EstadosT__FEF86B603D2331A8");

            entity.ToTable("EstadosTarea");

            entity.HasIndex(e => e.NombreEstado, "UQ__EstadosT__6CE50615135F16DF").IsUnique();

            entity.Property(e => e.EstadoID).HasColumnName("EstadoID");
            entity.Property(e => e.NombreEstado).HasMaxLength(50);
        });

        modelBuilder.Entity<LogsEjecucion>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogsEjec__5E5499A8CC80DE29");

            entity.ToTable("LogsEjecucion");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.EstadoID).HasColumnName("EstadoID");
            entity.Property(e => e.FechaLog)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TareaId).HasColumnName("TareaID");

            entity.HasOne(d => d.Estado).WithMany(p => p.LogsEjecucion)
                .HasForeignKey(d => d.EstadoID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogsEjecucion_EstadosTarea");

            entity.HasOne(d => d.Tarea).WithMany(p => p.LogsEjecucions)
                .HasForeignKey(d => d.TareaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogsEjecucion_Tareas");
        });

        modelBuilder.Entity<PrioridadesTarea>(entity =>
        {
            entity.HasKey(e => e.PrioridadID).HasName("PK__Priorida__393917CE87210E18");

            entity.ToTable("PrioridadesTarea");

            entity.HasIndex(e => e.NivelPrioridad, "UQ__Priorida__9A68DDA10AB71BC5").IsUnique();

            entity.Property(e => e.PrioridadID).HasColumnName("PrioridadID");
            entity.Property(e => e.NivelPrioridad).HasMaxLength(20);
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK_Tareas");

            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired(); // Campo obligatorio
            entity.Property(e => e.Descripcion).IsRequired();             // Campo obligatorio
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.FechaEjecucion).HasColumnType("datetime");
            entity.Property(e => e.FechaFinalizacion).HasColumnType("datetime");

            entity.HasOne(d => d.Estado)
                  .WithMany(p => p.Tareas)
                  .HasForeignKey(d => d.EstadoID)
                  .OnDelete(DeleteBehavior.ClientSetNull) // Prevenir eliminaciones en cascada
                  .HasConstraintName("FK_Tareas_EstadosTarea");

            entity.HasOne(d => d.Prioridad)
                  .WithMany(p => p.Tareas)
                  .HasForeignKey(d => d.PrioridadID)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Tareas_PrioridadesTarea");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
