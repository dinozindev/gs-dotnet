using API_GlobalSolution.Models;
using Microsoft.EntityFrameworkCore;

namespace API_GlobalSolution;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    
    public DbSet<Bairro> Bairros { get; set; }
    
    public DbSet<TipoDesastre> TipoDesastres { get; set; }
    
    public DbSet<Endereco> Enderecos { get; set; }
    
    public DbSet<Sensor> Sensores { get; set; }
    
    public DbSet<Alerta> Alertas { get; set; }
    
    public DbSet<Postagem> Postagens { get; set; }
    
    public DbSet<Comentario> Comentarios { get; set; }
    
    public DbSet<ConfirmaPostagem>  ConfirmaPostagens { get; set; }
    
    public string DbPath { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    
    modelBuilder.Entity<Usuario>()
        .HasIndex(u => u.EmailUsuario)
        .IsUnique();

    modelBuilder.Entity<Usuario>()
        .HasIndex(u => u.TelefoneUsuario)
        .IsUnique();

    modelBuilder.Entity<Usuario>()
        .Property(u => u.TipoUsuario)
        .HasDefaultValue("Usuário");

    modelBuilder.Entity<Usuario>()
        .HasCheckConstraint("chk_tipo_usuario", "tipo_usuario IN ('Usuário', 'Funcionário')");
    
    modelBuilder.Entity<Bairro>()
        .HasIndex(b => b.NomeBairro)
        .IsUnique();

    modelBuilder.Entity<Bairro>()
        .HasCheckConstraint("chk_zona_bairro", "zona_bairro IN ('Zona Sul', 'Zona Norte', 'Zona Leste', 'Zona Oeste', 'Zona Central')");
    
    modelBuilder.Entity<TipoDesastre>()
        .HasCheckConstraint("chk_nome_desastre", 
            "nome_desastre IN ('Enchente', 'Inundação', 'Deslizamento', 'Queimada', 'Incêndio Florestal', 'Tempestade', 'Vendaval')");
    
    modelBuilder.Entity<Endereco>()
        .HasOne(e => e.Bairro)
        .WithMany()
        .HasForeignKey(e => e.BairroId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Sensor>()
        .HasOne(s => s.Bairro)
        .WithMany()
        .HasForeignKey(s => s.BairroId)
        .OnDelete(DeleteBehavior.NoAction); 
    
    modelBuilder.Entity<Alerta>()
        .HasOne(a => a.Sensor)
        .WithMany()
        .HasForeignKey(a => a.SensorId)
        .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder.Entity<Alerta>()
        .HasOne(a => a.TipoDesastre)
        .WithMany()
        .HasForeignKey(a => a.TipoDesastreId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Alerta>()
        .Property(a => a.StatusAlerta)
        .HasDefaultValue("Ativo");

    modelBuilder.Entity<Alerta>()
        .HasCheckConstraint("chk_status_alerta", "status_alerta IN ('Ativo', 'Resolvido')");

    modelBuilder.Entity<Alerta>()
        .HasCheckConstraint("chk_nivel_risco", "nivel_risco IN ('Atenção', 'Alerta', 'Emergência')");
    
    modelBuilder.Entity<Postagem>()
        .HasOne(p => p.Usuario)
        .WithMany()
        .HasForeignKey(p => p.UsuarioId)
        .OnDelete(DeleteBehavior.NoAction);
    
    modelBuilder.Entity<Postagem>()
        .HasOne(p => p.Endereco)
        .WithMany()
        .HasForeignKey(p => p.EnderecoId)
        .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder.Entity<Postagem>()
        .HasOne(p => p.TipoDesastre)
        .WithMany()
        .HasForeignKey(p => p.TipoDesastreId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Postagem>()
        .Property(p => p.TipoPostagem)
        .HasDefaultValue("Usuário");

    modelBuilder.Entity<Postagem>()
        .Property(p => p.StatusPostagem)
        .HasDefaultValue("Ativo");

    modelBuilder.Entity<Postagem>()
        .HasCheckConstraint("chk_tipo_postagem", "tipo_postagem IN ('Usuário', 'Governo')");

    modelBuilder.Entity<Postagem>()
        .HasCheckConstraint("chk_status_postagem", "status_postagem IN ('Ativo', 'Resolvido', 'Descartado')");
    
    modelBuilder.Entity<Comentario>()
        .HasOne(c => c.Postagem)
        .WithMany(p => p.Comentarios)
        .HasForeignKey(c => c.PostagemId)
        .OnDelete(DeleteBehavior.Cascade);
    
    modelBuilder.Entity<Comentario>()
        .HasOne(c => c.Usuario)
        .WithMany()
        .HasForeignKey(c => c.UsuarioId)
        .OnDelete(DeleteBehavior.NoAction);
    
    modelBuilder.Entity<ConfirmaPostagem>()
        .HasKey(cp => new { cp.UsuarioId, cp.PostagemId });

    modelBuilder.Entity<ConfirmaPostagem>()
        .HasOne(cp => cp.Usuario)
        .WithMany()
        .HasForeignKey(cp => cp.UsuarioId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<ConfirmaPostagem>()
        .HasOne(cp => cp.Postagem)
        .WithMany()
        .HasForeignKey(cp => cp.PostagemId)
        .OnDelete(DeleteBehavior.Cascade);
}

}