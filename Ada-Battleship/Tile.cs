namespace Ada_Battleship
{
    public class Tile
    {
        public Coordinate Coordinate { get; set; }
        public TileStatus TileStatus;
        public char TilePlaceholder { get; set; }

            

        public Tile(int x, int y)
        {
            Coordinate = new Coordinate(x, y);
            TileStatus = TileStatus.Available;
            TilePlaceholder = '.';
        }
    }
}