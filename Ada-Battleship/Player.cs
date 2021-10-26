namespace Ada_Battleship
{
    public class Player
    {
        public string Name;
        public int State;
        private readonly Board _gameBoard = new Board();
        private readonly Board _shotBoard = new Board();
    }
}