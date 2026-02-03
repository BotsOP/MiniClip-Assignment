namespace Components.Grid
{
    public interface ISpawnManager
    {
        GridContext GetGridContext();
        void SpawnRandomEnemy(int difficulty);
    }
}
