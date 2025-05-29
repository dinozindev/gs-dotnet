using Microsoft.AspNetCore.SignalR;

namespace API_GlobalSolution;

public class FeedHub : Hub
{
    public async Task EntrarNoGrupo(string grupo)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, grupo);
    }

    public async Task SairDoGrupo(string grupo)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
    }
    
    // Grupos específicos para cada postagem (para comentários, confirmações, etc)
    public async Task EntrarNoGrupoPostagem(int postagemId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"postagem-{postagemId}");
    }

    public async Task SairDoGrupoPostagem(int postagemId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"postagem-{postagemId}");
    }
}