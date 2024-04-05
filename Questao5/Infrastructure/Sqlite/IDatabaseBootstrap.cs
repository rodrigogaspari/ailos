namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();

        void EnsureDeleted();
    }
}