namespace Ada_Battleship
{
    public class Tile
    {
        public Coordinate Coordinate { get; set; }

        public Tile(int x, int y)
        {
            Coordinate = new Coordinate(x, y);
        }
    }
}