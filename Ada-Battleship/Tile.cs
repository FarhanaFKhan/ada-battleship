namespace Ada_Battleship
{
    public class Tile
    {
        public Coordinate Coordinate { get; set; }
        public TileStatus TileStatus;
        public string ColumnLabel;

            

        public Tile(int x, int y)
        {
            Coordinate = new Coordinate(x, y);
            TileStatus = TileStatus.Available;
            //ColumnLabel = columnLabel;
        }
    }
}