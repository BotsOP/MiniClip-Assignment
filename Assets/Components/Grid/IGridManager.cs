namespace Components.Grid
{
    public interface IGridManager
    {
        GridContext GetGridContext();
        void SpawnRandomEnemy(int difficulty);
    }
}
