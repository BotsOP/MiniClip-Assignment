namespace Components.Grid
{
    public struct GridContext
    {
        public float cellSize;
        public float gridWidth;
        public float gridHeight;
        public GridContext(float cellSize, float gridWidth, float gridHeight)
        {
            this.cellSize = cellSize;
            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;
        }
    }
}
