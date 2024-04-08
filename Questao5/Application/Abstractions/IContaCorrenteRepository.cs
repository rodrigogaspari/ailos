namespace Questao5.Application.Abstractions
{
    public interface IContaCorrenteRepository
    {
        bool IsValidAccount(string? idContaCorrente);

        bool IsActiveAccount(string? idContaCorrente);
    }
}