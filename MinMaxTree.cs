public class PlayList
{
    public BoardGame Game { get; }
    public int Score { get; private set; }
    public Coords Coords { get; private set; }
    public List<PlayList> Children { get; private set; }

    public PlayList(BoardGame game, int score, Coords coords)
    {
        Game = game.Clone();
        Score = score;
        Coords = coords;
        Children = new List<PlayList>();
    }
    public void UpdateScore(int score)
    {
        Score = score;
    }
    public void UpdateCoords(Coords coords)
    {
        Coords = coords;
    }
    public void AddChild(PlayList child)
    {
        Children.Add(child);
    }

    public (int, Coords) GetPlay()
    {
        return (Score, Coords);
    }
}

public class MinMaxTree
{

}