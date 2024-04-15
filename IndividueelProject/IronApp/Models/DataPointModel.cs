using System.Runtime.Serialization;

namespace IronApp.Models;

public class DataPointModel
{
    public DataPointModel(DateTime x, decimal y)
    {
        this.X = x;
        this.Y = y;
    }
    public DateTime X { get; set; }
    public decimal Y { get; set; }
}