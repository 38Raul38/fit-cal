namespace FitCal.Core.Models;

public class UserInformation
{
    public Guid AuthUserId { get; set; }
    public int UserInformationId { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double WeightGoal { get; set; }
    public string ActivityLevel { get; set; }
    public double DailyCalories { get; set; }
    public double Protein { get; set; }
    public double Fats { get; set; }
    public double Carbs { get; set; }
    
    
    // Связь с историей питания
    public virtual ICollection<UserHistory> UserHistories { get; set; } = [];

    // Связь с рецептами, которые создал пользователь
    public virtual ICollection<Recipe> Recipes { get; set; } = [];

    // Связь с отчетами
    public virtual ICollection<Report> Reports { get; set; } = [];
}