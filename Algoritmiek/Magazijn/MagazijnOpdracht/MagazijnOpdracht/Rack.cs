namespace MagazijnOpdracht;

public class Rack
{
    public List<Closet> Closets { get; set; }
    
    public Rack(int numberOfClosets)
    {
        Closets = new List<Closet>();
        for (int i = 0; i < numberOfClosets; i++)
        {
            Closets.Add(new Closet());
        }
    }
}