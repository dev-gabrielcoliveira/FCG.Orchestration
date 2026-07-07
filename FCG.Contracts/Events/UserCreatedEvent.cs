namespace FCG.Contracts.Events
{
    public record UserCreatedEvent
    (
        int IdUsuario,
        string Nome,
        string Email
    );
}
