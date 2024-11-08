public class GridPosition {
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        GridPosition o = (GridPosition)obj;
        return x == o.x && y == o.y;
    }

    public override int GetHashCode()
    {
        // re-evaluate if x and y are going to be larger than 15
        return (x << 4) | y;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public int x;
    public int y;
    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
