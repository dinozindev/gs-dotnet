using System.ComponentModel;
using System.Threading.RateLimiting;
using API_GlobalSolution;
using API_GlobalSolution.Dtos;
using API_GlobalSolution.Models;
using DotNetEnv;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options => 
     options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// define um limite de requisições durante um determinado período.
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
});

builder.Services.AddOpenApi();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(opt =>
    {
        opt.WithOrigins("http://localhost:8081") // url da aplicação Mobile / Web para utilização do SignalR
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

//  habilita o CORS
app.UseCors();
// limita a qtnd de requisições
app.UseRateLimiter();

app.MapHub<FeedHub>("/hub/feed");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

var usuarios = app.MapGroup("/usuarios").WithTags("Usuários");
var bairros = app.MapGroup("/bairros").WithTags("Bairros");
var tiposDesastres = app.MapGroup("/tiposDesastres").WithTags("Tipos Desastres");
var enderecos = app.MapGroup("/enderecos").WithTags("Endereços");
var sensores = app.MapGroup("/sensores").WithTags("Sensores");
var alertas = app.MapGroup("/alertas").WithTags("Alertas");
var postagens = app.MapGroup("/postagens").WithTags("Postagens");
var comentarios = app.MapGroup("/comentarios").WithTags("Comentários");
var confirmaPostagens = app.MapGroup("/confirmaPostagens").WithTags("Confirma Postagens");
var login = app.MapGroup("/login").WithTags("Login");

// valor padrão do parâmetro de rotas que exigem id
const string idParam = "/{id}";
// valor padrão do parâmetro do tipo de conexão
const string applicationString = "application/json";

// realiza o Login na Aplicação Mobile
login.MapPost("/", async (LoginDto loginDto, AppDbContext db) =>
{
    // Verifica se o e-mail foi informado e está no formato correto
    if (string.IsNullOrWhiteSpace(loginDto.EmailUsuario) ||
        !System.Text.RegularExpressions.Regex.IsMatch(loginDto.EmailUsuario, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        return Results.BadRequest("E-mail inválido.");
    }

    // Verifica se a senha foi informada
    if (string.IsNullOrWhiteSpace(loginDto.SenhaUsuario))
    {
        return Results.BadRequest("Senha é obrigatória.");
    }

    // Busca o usuário no banco
    var usuario = await db.Usuarios
        .FirstOrDefaultAsync(u => u.EmailUsuario == loginDto.EmailUsuario);

    if (usuario == null)
    {
        return Results.NotFound("Usuário não encontrado.");
    }

    // Verifica a senha
    if (usuario.SenhaUsuario != loginDto.SenhaUsuario)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(usuario);
})
.WithSummary("Realiza o Login na Aplicação Mobile")
.WithDescription("Realiza o Login na Aplicação Mobile e retorna o Usuário que foi logado.")
.Produces<Usuario>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status401Unauthorized)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

// busca todos os usuarios
usuarios.MapGet("/", async (AppDbContext db) =>
    {
        var usuariosObtidos = await db.Usuarios.ToListAsync();
        
        var usuariosDto = usuariosObtidos
            .Select(UsuarioReadDto.ToDto)
            .ToList();

        return usuariosDto.Count == 0 ? Results.NoContent() : Results.Ok(usuariosDto);
    })
    .WithSummary("Retorna a lista de todos os usuários")
    .WithDescription("Retorna a lista de todos os usuários cadastrados no sistema.")
    .Produces<List<UsuarioReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// busca usuário pelo ID
usuarios.MapGet(idParam, async ([Description("Identificador único do Usuário")] int id, AppDbContext db) =>
    {
        var usuario = await db.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null)
            return Results.NotFound("Usuário não encontrado com o ID fornecido.");

        var usuarioDto = UsuarioReadDto.ToDto(usuario);

        return Results.Ok(usuarioDto);
    })
    .WithSummary("Retorna um usuário pelo ID")
    .WithDescription("Retorna um usuário pelo ID. Retorna 200 OK se o usuario for encontrado, ou erro se não for achado.")
    .Produces<UsuarioReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// cadastra um usuário novo
usuarios.MapPost("/", async (UsuarioPostDto dto, AppDbContext db) =>
{
    var usuario = new Usuario
    {
        NomeUsuario = dto.NomeUsuario,
        EmailUsuario = dto.EmailUsuario,
        SenhaUsuario = dto.SenhaUsuario,
        TelefoneUsuario = dto.TelefoneUsuario,
        TipoUsuario = "Usuário"
    };
    
    // Verifica se o e-mail está presente e é válido
    if (string.IsNullOrWhiteSpace(usuario.EmailUsuario) ||
        !System.Text.RegularExpressions.Regex.IsMatch(usuario.EmailUsuario, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        return Results.BadRequest("E-mail inválido.");
    }

    // Verifica se o telefone está no formato correto (somente dígitos, com 10 ou 11 números)
    if (string.IsNullOrWhiteSpace(usuario.TelefoneUsuario) ||
        !System.Text.RegularExpressions.Regex.IsMatch(usuario.TelefoneUsuario, @"^\d{10,11}$"))
    {
        return Results.BadRequest("Telefone inválido. Deve conter 10 ou 11 dígitos numéricos.");
    }

    // Verifica se o nome está preenchido
    if (string.IsNullOrWhiteSpace(usuario.NomeUsuario))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }

    // Verifica se a senha está preenchida
    if (string.IsNullOrWhiteSpace(usuario.SenhaUsuario))
    {
        return Results.BadRequest("Senha é obrigatória.");
    }

    // Verifica se o e-mail já está cadastrado
    var emailExiste = await db.Usuarios.CountAsync(u => u.EmailUsuario == usuario.EmailUsuario);
    if (emailExiste > 0)
    {
        return Results.Conflict($"Já existe um usuário com o e-mail '{usuario.EmailUsuario}'.");
    }

    // Verifica se o telefone já está cadastrado
    var telefoneExiste = await db.Usuarios.CountAsync(u => u.TelefoneUsuario == usuario.TelefoneUsuario);
    if (telefoneExiste > 0)
    {
        return Results.Conflict($"Já existe um usuário com o telefone '{usuario.TelefoneUsuario}'.");
    }

    
    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();

    return Results.Created($"/usuarios/{usuario.UsuarioId}", usuario);
    
})
    .Accepts<UsuarioPostDto>(applicationString)
    .WithSummary("Cria um usuário")
    .WithDescription("Cria um usuário no sistema.")
    .Produces<Usuario>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status409Conflict)
    .Produces(StatusCodes.Status500InternalServerError);

// atualiza os dados de um usuário
usuarios.MapPut(idParam, async ([Description("Identificador único do Usuário")] int id, UsuarioPostDto dto, AppDbContext db) =>
{
    var usuarioExistente = await db.Usuarios.FindAsync(id);
    if (usuarioExistente == null)
    {
        return Results.NotFound($"Usuário com ID {id} não encontrado.");
    }
    
    usuarioExistente.NomeUsuario = dto.NomeUsuario;
    usuarioExistente.EmailUsuario = dto.EmailUsuario;
    usuarioExistente.SenhaUsuario = dto.SenhaUsuario;
    usuarioExistente.TelefoneUsuario = dto.TelefoneUsuario;
    
    // Verifica se o e-mail está presente e é válido
    if (string.IsNullOrWhiteSpace(usuarioExistente.EmailUsuario) ||
        !System.Text.RegularExpressions.Regex.IsMatch(usuarioExistente.EmailUsuario, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        return Results.BadRequest("E-mail inválido.");
    }

    // Verifica se o telefone está no formato correto (somente dígitos, com 10 ou 11 números)
    if (string.IsNullOrWhiteSpace(usuarioExistente.TelefoneUsuario) ||
        !System.Text.RegularExpressions.Regex.IsMatch(usuarioExistente.TelefoneUsuario, @"^\d{10,11}$"))
    {
        return Results.BadRequest("Telefone inválido. Deve conter 10 ou 11 dígitos numéricos.");
    }

    // Verifica se o nome está preenchido
    if (string.IsNullOrWhiteSpace(usuarioExistente.NomeUsuario))
    {
        return Results.BadRequest("Nome é obrigatório.");
    }

    // Verifica se a senha está preenchida
    if (string.IsNullOrWhiteSpace(usuarioExistente.SenhaUsuario))
    {
        return Results.BadRequest("Senha é obrigatória.");
    }

    // Verifica se o e-mail já está cadastrado por outro usuário (excluindo o atual)
    var emailExiste = await db.Usuarios.CountAsync(u => u.EmailUsuario == usuarioExistente.EmailUsuario && u.UsuarioId != usuarioExistente.UsuarioId);
    if (emailExiste > 0)
    {
        return Results.Conflict($"Já existe um usuário com o e-mail '{usuarioExistente.EmailUsuario}'.");
    }

    // Verifica se o telefone já está cadastrado por outro usuário (excluindo o atual)
    var telefoneExiste = await db.Usuarios.CountAsync(u => u.TelefoneUsuario == usuarioExistente.TelefoneUsuario && u.UsuarioId != usuarioExistente.UsuarioId);
    if (telefoneExiste > 0)
    {
        return Results.Conflict($"Já existe um usuário com o telefone '{usuarioExistente.TelefoneUsuario}'.");
    }

    await db.SaveChangesAsync();
    return Results.Ok(usuarioExistente);
})
.Accepts<UsuarioPostDto>(applicationString)
.WithSummary("Atualiza um usuário")
.WithDescription("Atualiza os dados de um usuário existente.")
.Produces<Usuario>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status409Conflict)
.Produces(StatusCodes.Status500InternalServerError);

// deleta um usuário
usuarios.MapDelete(idParam, async ([Description("Identificador único do Usuário")] int id, AppDbContext db) =>
{
    if (await db.Usuarios.FindAsync(id) is { } existingUsuario)
    {
        db.Usuarios.Remove(existingUsuario);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound("Nenhum usuário encontrado com ID fornecido.");
})
.WithSummary("Deleta um Usuário pelo ID")
.WithDescription("Deleta um Usuário pelo ID informado. Retorna 204 No Content caso deletado com sucesso, ou erro se não achado.")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError); 

// retorna todos os bairros cadastrados.
bairros.MapGet("/", async (AppDbContext db) =>
{
    var bairrosObtidos = await db.Bairros.ToListAsync();

    var bairrosDto = bairrosObtidos
        .Select(BairroReadDto.ToDto)
        .ToList();
    
    return bairrosDto.Count == 0 ? Results.NoContent() : Results.Ok(bairrosDto);
})
    .WithSummary("Retorna a lista de todos os Bairros")
    .WithDescription("Retorna a lista de todos os bairros cadastrados no sistema.")
    .Produces<List<BairroReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// Retorna bairro por ID
bairros.MapGet(idParam, async ([Description("Identificador único do Bairro")] int id, AppDbContext db) =>
{
    var bairro = await db.Bairros.FirstOrDefaultAsync(b => b.BairroId == id);
    
    if (bairro == null)
    {
        return Results.NotFound("Nenhum bairro encontrado.");
    }
    
    var bairroDto = BairroReadDto.ToDto(bairro);
    
    return Results.Ok(bairroDto);
})
    .WithSummary("Retorna um Bairro pelo ID")
    .WithDescription("Retorna um Bairro pelo ID. Retorna 200 OK se o Bairro for encontrado, ou erro se não for achado.")
    .Produces<BairroReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// Retorna todos os tipos de desastre
tiposDesastres.MapGet("/", async (AppDbContext db) =>
{
    var tiposDesastresObtidos = await db.TipoDesastres.ToListAsync();
    
    var tiposDesastresDto = tiposDesastresObtidos
        .Select(TipoDesastreReadDto.ToDto)
        .ToList();
    
    return tiposDesastresDto.Count == 0 ? Results.NoContent() : Results.Ok(tiposDesastresDto);
})
    .WithSummary("Retorna a lista de todos os tipos de desastre")
    .WithDescription("Retorna a lista de todos os tipos de desastre cadastrados no sistema.")
    .Produces<List<TipoDesastreReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna um tipo de desastre pelo ID
tiposDesastres.MapGet(idParam, async ([Description("Identificador único do Tipo de Desastre")] int id, AppDbContext db) =>
    {
        var tipoDesastreObtido = await db.TipoDesastres.FirstOrDefaultAsync(t => t.TipoDesastreId == id);
        if (tipoDesastreObtido == null)
        {
            return Results.NotFound("Nenhum tipo de desastre encontrado.");
        } 
        
        var tipoDesastreDto = TipoDesastreReadDto.ToDto(tipoDesastreObtido);
        
        return Results.Ok(tipoDesastreDto);
    })
    .WithSummary("Retorna um Tipo de Desastre pelo ID")
    .WithDescription("Retorna um Tipo de Desastre pelo ID. Retorna 200 OK se o Tipo de Desastre for encontrado, ou erro se não for achado.")
    .Produces<TipoDesastreReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna todos os endereços cadastrados
enderecos.MapGet("/", async (AppDbContext db) =>
{
    var enderecosObtidos = await db.Enderecos
        .Include(e => e.Bairro)
        .ToListAsync();
    
    var enderecosDto = enderecosObtidos
        .Select(EnderecoReadDto.ToDto)
        .ToList();
    
    return enderecosDto.Count == 0 ? Results.NoContent() : Results.Ok(enderecosDto);
    
})
    .WithSummary("Retorna a lista de todos os endereços")
    .WithDescription("Retorna a lista de todos os endereços cadastrados no sistema.")
    .Produces<List<EnderecoReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna um endereço pelo ID
enderecos.MapGet(idParam, async ([Description("Identificador único do Endereço")] int id, AppDbContext db) =>
{
    var enderecoObtido = await db.Enderecos
        .Include(e => e.Bairro)
        .FirstOrDefaultAsync(t => t.EnderecoId == id);
    if (enderecoObtido == null)
    {
        return Results.NotFound("Nenhum endereço encontrado.");
    }
    
    var enderecoDto = EnderecoReadDto.ToDto(enderecoObtido);

    return Results.Ok(enderecoDto);
    
})
    .WithSummary("Retorna um Endereço pelo ID")
    .WithDescription("Retorna um Endereço pelo ID. Retorna 200 OK se o Endereço for encontrado, ou erro se não for achado.")
    .Produces<EnderecoReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna todos os sensores cadastrados
sensores.MapGet("/", async (AppDbContext db) =>
{
    var sensoresObtidos = await db.Sensores
        .Include(s => s.Bairro)
        .ToListAsync();
    
    var sensoresDto = sensoresObtidos
        .Select(SensorReadDto.ToDto)
        .ToList();
    
    return Results.Ok(sensoresDto);
})
    .WithSummary("Retorna a lista de todos os sensores")
    .WithDescription("Retorna a lista de todos os sensores cadastrados no sistema.")
    .Produces<List<SensorReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna um sensor pelo ID
sensores.MapGet(idParam, async ([Description("Identificador único de Sensor")] int id, AppDbContext db) =>
{
    var sensorObtido = await db.Sensores
            .Include(s => s.Bairro)
            .FirstOrDefaultAsync(t => t.SensorId == id);

    if (sensorObtido == null)
    {
        return Results.NotFound("Nenhum sensor encontrado.");
    }
    
    var sensorDto = SensorReadDto.ToDto(sensorObtido);

    return Results.Ok(sensorDto);
})
    .WithSummary("Retorna um Sensor pelo ID")
    .WithDescription("Retorna um Sensor pelo ID. Retorna 200 OK se o Sensor for encontrado, ou erro se não for achado.")
    .Produces<SensorReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna todos os alertas
alertas.MapGet("/", async (AppDbContext db) =>
{
    var alertasObtidos = await db.Alertas
        .Include(a => a.Sensor)
        .ThenInclude(s => s.Bairro)
        .Include(a => a.TipoDesastre)
        .ToListAsync();
    
    var alertasDto = alertasObtidos
        .Select(AlertaReadDto.ToDto)
        .ToList();
    
    return alertasDto.Count == 0 ? Results.NoContent() : Results.Ok(alertasDto);
})
.WithSummary("Retorna a lista de todos os alertas")
.WithDescription("Retorna a lista de todos os alertas cadastrados no sistema.")
.Produces<List<AlertaReadDto>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status500InternalServerError);

// retorna Alerta por ID
alertas.MapGet(idParam, async ([Description("Identificador único de Alerta")] int id, AppDbContext db) =>
{
    var alertaObtido = await db.Alertas
        .Include(a => a.Sensor)
        .ThenInclude(s => s.Bairro)
        .Include(a => a.TipoDesastre)
        .FirstOrDefaultAsync(a => a.AlertaId == id);
    if (alertaObtido == null)
    {
        return Results.NotFound("Nenhum alerta cadastrado.");
    }
    
    var alertaDto = AlertaReadDto.ToDto(alertaObtido);
    
    return Results.Ok(alertaDto);
})
    .WithSummary("Retorna um Alerta pelo ID")
    .WithDescription("Retorna um Alerta pelo ID. Retorna 200 OK se o Alerta for encontrado, ou erro se não for achado.")
    .Produces<AlertaReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna todas as postagens
postagens.MapGet("/", async (AppDbContext db) =>
{
    var postagensObtidas = await db.Postagens
        .Include(p => p.Comentarios)
        .ThenInclude(c => c.Usuario)
        .Include(p => p.Usuario)
        .Include(p => p.Endereco)
        .ThenInclude(e => e.Bairro)
        .Include(p => p.TipoDesastre)
        .ToListAsync();
    
    var postagensDto = postagensObtidas
        .Select(PostagemReadDto.ToDto)
        .ToList();
    
    return postagensDto.Count == 0 ? Results.NoContent() : Results.Ok(postagensDto);
})
    .WithSummary("Retorna a lista de todas as Postagens")
    .WithDescription("Retorna a lista de todas as Postagens cadastradas no sistema.")
    .Produces<List<PostagemReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna Postagem por ID
postagens.MapGet(idParam, async ([Description("Identificador único de Postagem")] int id, AppDbContext db) =>
{
    var postagemObtida = await db.Postagens
        .Include(p => p.Comentarios)
        .ThenInclude(c => c.Usuario)
        .Include(p => p.Usuario)
        .Include(p => p.Endereco)
        .ThenInclude(e => e.Bairro)
        .Include(p => p.TipoDesastre) 
        .FirstOrDefaultAsync(a => a.PostagemId == id);

    if (postagemObtida == null)
    {
        return Results.NotFound("Nenhuma postagem cadastrada com o ID fornecido.");
    }
    
    var postagemDto = PostagemReadDto.ToDto(postagemObtida);
    
    return Results.Ok(postagemDto);
})
    .WithSummary("Retorna uma Postagem pelo ID")
    .WithDescription("Retorna uma Postagem pelo ID. Retorna 200 OK se a Postagem for encontrada, ou erro se não for achada.")
    .Produces<PostagemReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// buscar postagens por Tipo (governo ou usuario)
postagens.MapGet("/tipo-usuario/{tipo}", async ([Description("Tipo do usuário (Usuário ou Governo)")] string tipo, AppDbContext db) =>
{
    var postagensObtidas = await db.Postagens
        .Include(p => p.Usuario)
        .Include(p => p.Endereco)
        .ThenInclude(e => e.Bairro)
        .Include(p => p.TipoDesastre)
        .Include(p => p.Comentarios)
        .ThenInclude(c => c.Usuario)
        .Where(p => p.TipoPostagem.ToLower() == tipo.ToLower())
        .ToListAsync();
    
    var postagensDto = postagensObtidas
        .Select(PostagemReadDto.ToDto)
        .ToList();
    
    return postagensDto.Count == 0 ? Results.NoContent() : Results.Ok(postagensDto);
    
})
    .WithSummary("Retorna a lista de todas as postagens de um tipo de usuário")
    .WithDescription("Retorna a lista de todas as postagens de um tipo de usuário ('Usuário' ou 'Governo') cadastradas no sistema.")
    .Produces<List<PostagemReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// criar postagem
postagens.MapPost("/", async (PostagemPostDto dto, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    // verifica se o bairro existe
    var bairro = await db.Bairros.FindAsync(dto.Endereco.BairroId);
    if (bairro == null)
    {
        return Results.BadRequest("Bairro informado não existe.");
    }

    // verifica se o usuário existe
    var usuario = await db.Usuarios.FindAsync(dto.UsuarioId);
    if (usuario == null)
    {
        return Results.BadRequest("Usuário informado não existe.");
    }

    // verifica se o tipo de desastre existe
    var tipoDesastre = await db.TipoDesastres.FindAsync(dto.TipoDesastreId);
    if (tipoDesastre == null)
    {
        return Results.BadRequest("Tipo de Desastre não encontrado");
    }

    // Procura endereço com os dados fornecidos (mesmo se incompletos)
    var enderecoExistente = await db.Enderecos
        .FirstOrDefaultAsync(e => e.BairroId == dto.Endereco.BairroId &&
                                  (e.LogradouroEndereco ?? "").ToLower() ==
                                  (dto.Endereco.LogradouroEndereco ?? "").ToLower() &&
                                  e.NumeroEndereco == dto.Endereco.NumeroEndereco &&
                                  (e.ComplementoEndereco ?? "") == (dto.Endereco.ComplementoEndereco ?? "") &&
                                  (e.CepEndereco ?? "") == (dto.Endereco.CepEndereco ?? ""));

    if (enderecoExistente == null)
    {
        enderecoExistente = new Endereco
        {
            LogradouroEndereco = dto.Endereco.LogradouroEndereco,
            NumeroEndereco = dto.Endereco.NumeroEndereco,
            ComplementoEndereco = dto.Endereco.ComplementoEndereco,
            CepEndereco = dto.Endereco.CepEndereco,
            BairroId = dto.Endereco.BairroId
        };

        db.Enderecos.Add(enderecoExistente);
        await db.SaveChangesAsync();
    }

    var postagem = new Postagem
    {
        TituloPostagem = dto.TituloPostagem,
        DescricaoPostagem = dto.DescricaoPostagem,
        DataPostagem = DateTime.Now,
        TipoPostagem = usuario.TipoUsuario == "Usuário" ? "Usuário" : "Governo",
        StatusPostagem = "Ativo",
        UsuarioId = usuario.UsuarioId,
        EnderecoId = enderecoExistente.EnderecoId,
        TipoDesastreId = dto.TipoDesastreId
    };

    db.Postagens.Add(postagem);
    await db.SaveChangesAsync();

    var postagemDto = PostagemReadDto.ToDto(postagem);

    await hubContext.Clients.Group("postagens").SendAsync("PostagemCriadaOuAtualizada", postagemDto);
    
    return Results.Created($"/postagens/{postagem.PostagemId}", postagemDto);
})
    .Accepts<PostagemPostDto>(applicationString)
    .WithSummary("Cria uma Postagem")
    .WithDescription("Cria uma Postagem no sistema.")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status500InternalServerError);

// atualizar postagem
postagens.MapPut(idParam, async ([Description("Identificador único de Postagem")] int id, PostagemPutDto dto, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    // Verifica se a postagem existe
    var postagem = await db.Postagens.FindAsync(id);
    if (postagem == null)
        return Results.NotFound("Postagem não encontrada.");

    // Verifica se o bairro existe
    var bairro = await db.Bairros.FindAsync(dto.Endereco.BairroId);
    if (bairro == null)
        return Results.BadRequest("Bairro informado não existe.");

    // Verifica se o tipo de desastre existe
    var tipoDesastre = await db.TipoDesastres.FindAsync(dto.TipoDesastreId);
    if (tipoDesastre == null)
        return Results.BadRequest("Tipo de Desastre não encontrado");

    // Verifica se o endereço já existe 
    var enderecoExistente = await db.Enderecos
        .FirstOrDefaultAsync(e =>
            e.BairroId == dto.Endereco.BairroId &&
            (e.LogradouroEndereco ?? "").ToLower() == (dto.Endereco.LogradouroEndereco ?? "").ToLower() &&
            e.NumeroEndereco == dto.Endereco.NumeroEndereco &&
            (e.ComplementoEndereco ?? "") == (dto.Endereco.ComplementoEndereco ?? "") &&
            (e.CepEndereco ?? "") == (dto.Endereco.CepEndereco ?? ""));

    if (enderecoExistente == null)
    {
        enderecoExistente = new Endereco
        {
            LogradouroEndereco = dto.Endereco.LogradouroEndereco,
            NumeroEndereco = dto.Endereco.NumeroEndereco,
            ComplementoEndereco = dto.Endereco.ComplementoEndereco,
            CepEndereco = dto.Endereco.CepEndereco,
            BairroId = dto.Endereco.BairroId
        };

        db.Enderecos.Add(enderecoExistente);
        await db.SaveChangesAsync();
    }
    
    postagem.TituloPostagem = dto.TituloPostagem;
    postagem.DescricaoPostagem = dto.DescricaoPostagem;
    postagem.DataPostagem = DateTime.Now;
    postagem.StatusPostagem = dto.StatusPostagem;
    postagem.TipoDesastreId = dto.TipoDesastreId;
    postagem.EnderecoId = enderecoExistente.EnderecoId;

    await db.SaveChangesAsync();
    
    var postagemCompleta = await db.Postagens
        .Include(p => p.Usuario)
        .Include(p => p.Endereco)
        .ThenInclude(e => e.Bairro)
        .Include(p => p.TipoDesastre)
        .Include(p => p.Comentarios)
        .ThenInclude(c => c.Usuario)
        .FirstOrDefaultAsync(p => p.PostagemId == postagem.PostagemId);
    
    var postagemDto = PostagemReadDto.ToDto(postagemCompleta!);
    
    await hubContext.Clients.Group("postagens").SendAsync("PostagemCriadaOuAtualizada", postagemDto);

    return Results.Ok("Postagem atualizada com sucesso.");
})
    .Accepts<PostagemPutDto>(applicationString)
    .WithSummary("Atualiza uma Postagem")
    .WithDescription("Atualiza os dados de uma Postagem existente.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// deletar postagem
postagens.MapDelete(idParam, async ([Description("Identificador único de Postagem")] int id, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    if (await db.Postagens.FindAsync(id) is { } existingPostagem)
    {
        db.Postagens.Remove(existingPostagem);
        await db.SaveChangesAsync();
        await hubContext.Clients.Group("postagens").SendAsync("PostagemRemovida", new { PostagemId = id });
        return Results.NoContent();
    }
    return Results.NotFound("Nenhuma postagem encontrada com ID fornecido.");
})
    .WithSummary("Deleta uma Postagem pelo ID")
    .WithDescription("Deleta uma Postagem pelo ID informado. Retorna 204 No Content caso deletada com sucesso, ou erro se não achado.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError); 

// retorna todos os comentários
comentarios.MapGet("/", async (AppDbContext db) =>
{
    var comentariosObtidos = await db.Comentarios
        .Include(c => c.Postagem)
        .ThenInclude(p => p.Usuario)
        .Include(c => c.Postagem)
        .ThenInclude(p => p.Endereco)
        .ThenInclude(e => e.Bairro)  
        .Include(c => c.Postagem)
        .ThenInclude(p => p.TipoDesastre)
        .Include(c => c.Usuario)  
        .ToListAsync();

    var comentariosDto = comentariosObtidos
        .Select(ComentarioReadDto.ToDto)
        .ToList();
    
    return comentariosDto.Count == 0 ? Results.NoContent() : Results.Ok(comentariosDto);
})
    .WithSummary("Retorna a lista de todos os Comentários")
    .WithDescription("Retorna a lista de todos os Comentários cadastrados no sistema.")
    .Produces<List<ComentarioReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// retorna um comentário pelo ID
comentarios.MapGet(idParam, async ([Description("Identificador único de Comentário")] int id, AppDbContext db) =>
{
    var comentarioObtido = await db.Comentarios
            .Include(c => c.Postagem)
            .ThenInclude(p => p.Usuario)
            .Include(c => c.Postagem)
            .ThenInclude(p => p.Endereco)
            .ThenInclude(e => e.Bairro)  
            .Include(c => c.Postagem)
            .ThenInclude(p => p.TipoDesastre)
            .Include(c => c.Usuario)  
            .FirstOrDefaultAsync(c => c.ComentarioId == id);

    if (comentarioObtido == null)
    {
        return Results.NotFound("Comentário não cadastrado.");
    }
    
    var comentarioDto = ComentarioReadDto.ToDto(comentarioObtido);
    
    return Results.Ok(comentarioDto);
})
    .WithSummary("Retorna um Comentário pelo ID")
    .WithDescription("Retorna um Comentário pelo ID. Retorna 200 OK se o Comentário for encontrado, ou erro se não for achado.")
    .Produces<ComentarioReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// criar um comentario
comentarios.MapPost("/", async (ComentarioPostDto dto, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    var comentario = new Comentario
    {
        TextoComentario = dto.TextoComentario,
        DataComentario = DateTime.Now,
        PostagemId = dto.PostagemId,
        UsuarioId = dto.UsuarioId
    };

    if (string.IsNullOrEmpty(comentario.TextoComentario))
    {
        return Results.BadRequest("O comentário não pode estar vazio.");
    } 
    if (comentario.TextoComentario.Length > 255)
    {
        return Results.BadRequest("O comentário deve ter até 255 caracteres.");
    }
    
    var postagemExiste = await db.Postagens.FindAsync(dto.PostagemId);
    if (postagemExiste == null)
    {
        return Results.NotFound("Postagem não existe.");
    }
    
    var usuarioExiste = await db.Usuarios.FindAsync(dto.UsuarioId);
    if (usuarioExiste == null)
    {
        return Results.NotFound("Usuário não existe.");
    }

    db.Comentarios.Add(comentario);
    await db.SaveChangesAsync();
    
    // recupera o comentário completo para ser enviado para o Front pelo SignalR
    var comentarioCompleto = await db.Comentarios
            .Include(c => c.Postagem)
            .ThenInclude(p => p.Usuario)
            .Include(c => c.Postagem)
            .ThenInclude(p => p.Endereco)
            .ThenInclude(e => e.Bairro)  
            .Include(c => c.Postagem)
            .ThenInclude(p => p.TipoDesastre)
            .Include(c => c.Usuario) 
            .FirstOrDefaultAsync(c => c.ComentarioId == comentario.ComentarioId);
    
    var comentarioDto = ComentarioReadDto.ToDto(comentarioCompleto!);
    
    await hubContext.Clients.Group($"postagem-{comentarioCompleto!.PostagemId}").SendAsync("ComentarioCriado", comentarioDto);

    return Results.Created($"comentarios/{comentario.ComentarioId}", comentarioDto);
})
    .Accepts<ComentarioPostDto>(applicationString)
    .WithSummary("Cria um Comentário")
    .WithDescription("Cria um Comentário no sistema.")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// atualizar um comentario
comentarios.MapPut(idParam, async ([Description("Identificador único de comentário")] int id, ComentarioPutDto dto, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    var comentarioExistente = await db.Comentarios.FindAsync(id);
    if (comentarioExistente == null)
    {
        return Results.NotFound("Comentário não existe.");
    }

    comentarioExistente.TextoComentario = dto.TextoComentario;
    comentarioExistente.DataComentario = DateTime.Now;
    
    if (string.IsNullOrEmpty(comentarioExistente.TextoComentario))
    {
        return Results.BadRequest("O comentário não pode estar vazio.");
    } 
    if (comentarioExistente.TextoComentario.Length > 255)
    {
        return Results.BadRequest("O comentário deve ter até 255 caracteres.");
    }
    
    await db.SaveChangesAsync();
    
    // recupera o comentário completo para ser enviado para o Front pelo SignalR
    var comentarioCompleto = await db.Comentarios
        .Include(c => c.Postagem)
        .ThenInclude(p => p.Usuario)
        .Include(c => c.Postagem)
        .ThenInclude(p => p.Endereco)
        .ThenInclude(e => e.Bairro)  
        .Include(c => c.Postagem)
        .ThenInclude(p => p.TipoDesastre)
        .Include(c => c.Usuario) 
        .FirstOrDefaultAsync(c => c.ComentarioId == comentarioExistente.ComentarioId);
    
    var comentarioDto = ComentarioReadDto.ToDto(comentarioCompleto!);
    
    await hubContext.Clients.Group($"postagem-{comentarioCompleto!.PostagemId}").SendAsync("ComentarioAtualizado", comentarioDto);
    
    return Results.Ok(comentarioDto);
})
    .Accepts<ComentarioPutDto>(applicationString)
    .WithSummary("Atualiza um Comentário")
    .WithDescription("Atualiza os dados de um Comentário existente.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// deletar um comentario
comentarios.MapDelete(idParam, async ([Description("Identificador único de Comentário")] int id, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    if (await db.Comentarios.FindAsync(id) is { } existingComentario)
    {
        db.Comentarios.Remove(existingComentario);
        await db.SaveChangesAsync();
        
        await hubContext.Clients.Group($"postagem-{existingComentario.PostagemId}").SendAsync("ComentarioRemovido", new { ComentarioId = id, PostagemId = existingComentario.PostagemId });
        return Results.NoContent();
    }
    return Results.NotFound("Nenhum comentário encontrado com ID fornecido.");
})
.WithSummary("Deleta um Comentário pelo ID")
.WithDescription("Deleta um Comentário pelo ID informado. Retorna 204 No Content caso deletado com sucesso, ou erro se não achado.")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError);

// trazer todos os ConfirmaPostagem de uma Postagem
confirmaPostagens.MapGet("/postagem/{postagemId}", async ([Description("Identificador único de Postagem")] int postagemId, AppDbContext db) =>
    {
        var confirmaPostagensObtidas = await db.ConfirmaPostagens
            .Include(cp => cp.Postagem)
            .Include(cp => cp.Usuario)
            .Where(cp => cp.PostagemId == postagemId)
            .ToListAsync();

        var confirmaPostagensDto = confirmaPostagensObtidas
            .Select(ConfirmaPostagemReadDto.ToDto)
            .ToList();
        
        return confirmaPostagensDto.Count == 0 ? Results.NoContent() : Results.Ok(confirmaPostagensDto);
    })
    .WithSummary("Retorna a lista de todos as confirmações de uma Postagem")
    .WithDescription("Retorna a lista de todas as confirmações de uma Postagem cadastrados no sistema.")
    .Produces<List<ConfirmaPostagemReadDto>>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);

// Retorna total de confirmações de uma Postagem
confirmaPostagens.MapGet("/count/{postagemId}", async ([Description("Identificador único de Postagem")] int postagemId, AppDbContext db) =>
    {
        var postagemExiste = await db.Postagens.FindAsync(postagemId);
        if (postagemExiste == null)
        {
            return Results.NotFound("Postagem não encontrada.");
        }

        var contagem = await db.ConfirmaPostagens.CountAsync(cp => cp.PostagemId == postagemId);
        return Results.Ok(new { contagem });
    })
    .WithSummary("Retorna a contagem de confirmações para uma postagem")
    .WithDescription("Retorna o número total de confirmações de uma postagem pelo seu ID.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// Retorna se o usuário já confirmou a postagem
confirmaPostagens.MapGet("/usuario/{usuarioId}/postagem/{postagemId}", 
        async (int usuarioId, int postagemId, AppDbContext db) =>
        {
            var confirma = await db.ConfirmaPostagens
                .FirstOrDefaultAsync(cp => cp.UsuarioId == usuarioId && cp.PostagemId == postagemId);

            if (confirma == null)
            {
                return Results.NoContent();
            }
            
            var confirmaDto = ConfirmaPostagemReadDto.ToDto(confirma);
            
            return Results.Ok(confirmaDto); 
        })
    .WithSummary("Verifica se o usuário confirmou uma postagem")
    .WithDescription("Retorna os dados da confirmação caso exista.")
    .Produces<ConfirmaPostagemReadDto>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status500InternalServerError);


// criar (confirmar) um ConfirmaPostagem
confirmaPostagens.MapPost("/", async (ConfirmaPostagemPostDto dto, AppDbContext db, IHubContext<FeedHub> hubContext) =>
{
    var confirmaPostagemExiste = await db.ConfirmaPostagens
        .FirstOrDefaultAsync(cp => cp.PostagemId == dto.PostagemId && cp.UsuarioId == dto.UsuarioId);

    if (confirmaPostagemExiste != null)
    {
        return Results.BadRequest("Usuário já confirmou esta postagem.");
    }
    
    var postagemExiste = await db.Postagens.FindAsync(dto.PostagemId);
    if (postagemExiste == null)
    {
        return Results.NotFound("Postagem não existe.");
    }
    
    var usuarioExiste = await db.Usuarios.FindAsync(dto.UsuarioId);
    if (usuarioExiste == null)
    {
        return Results.NotFound("Usuário não existe.");
    }

    var confirmaPostagem = new ConfirmaPostagem
    {
        UsuarioId = dto.UsuarioId,
        PostagemId = dto.PostagemId,
        DataConfirma = DateTime.Now
    };
    
    db.ConfirmaPostagens.Add(confirmaPostagem);
    await db.SaveChangesAsync();
    
    var novaContagem = await db.ConfirmaPostagens.CountAsync(cp => cp.PostagemId == dto.PostagemId);

    // Enviar atualização em tempo real
    await hubContext.Clients.Group($"postagem-{dto.PostagemId}")
        .SendAsync("AtualizarContagemConfirmacoes", new { PostagemId = dto.PostagemId, Contagem = novaContagem });

    return Results.Created($"/confirmaPostagens/{dto.UsuarioId}/{dto.PostagemId}", "Postagem confirmada com sucesso!");
})
    .Accepts<ConfirmaPostagemPostDto>(applicationString)
    .WithSummary("Confirma uma postagem")
    .WithDescription("Cria (confirma) uma Confirma Postagem no sistema.")
    .Produces(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError);

// deletar um Confirma Postagem
confirmaPostagens.MapDelete("/usuario/{usuarioId}/postagem/{postagemId}",
    async ([Description("Identificador único de Postagem")] int postagemId,
        [Description("Identificador único de Usuário")] int usuarioId, AppDbContext db, IHubContext<FeedHub> hubContext) =>
    {
        var confirmaPostagemExiste = await db.ConfirmaPostagens
            .FirstOrDefaultAsync(cp => cp.PostagemId == postagemId && cp.UsuarioId == usuarioId);

        if (confirmaPostagemExiste == null)
        {
            return Results.NotFound("Confirma Postagem não encontrado.");
        }
        
        db.ConfirmaPostagens.Remove(confirmaPostagemExiste);
        await db.SaveChangesAsync();
        
        // Atualizar contagem depois de deletar
        var novaContagem = await db.ConfirmaPostagens.CountAsync(cp => cp.PostagemId == postagemId);
        
        await hubContext.Clients.Group($"postagem-{postagemId}")
            .SendAsync("AtualizarContagemConfirmacoes", new { PostagemId = postagemId, Contagem = novaContagem });

        return Results.NoContent();
        
    }).WithSummary("Remove a confirmação de uma postagem pelos IDs")
    .WithDescription("Deleta um Confirma Postagem pelos IDs informados. Retorna 204 No Content caso deletado com sucesso, ou erro se não achado.")
    .Produces(StatusCodes.Status204NoContent)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status500InternalServerError); 

app.Run();