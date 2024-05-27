namespace IronApp.Models;

public class ProfileModel
{
    public UserModel UserModel { get; set; }
    public int WorkoutCount { get; set; }
    public decimal TotalVolume { get; set; }
    public double AveragePerWeek { get; set; }
    public List<Dictionary<string, int>> WorkoutsPerWeek { get; set; }
}