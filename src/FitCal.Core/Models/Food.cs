namespace FitCal.Core.Models;

public class Food
{
    public int FoodId { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public double ServingSize { get; set; } 
    public string ServingUnit { get; set; }
    
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Carbs { get; set; }
    public double Fats { get; set; }
    
    public Guid OwnerAuthUserId { get; set; }
    
    // Связь с UserHistory
    public virtual ICollection<UserHistory> UserHistories { get; set; } = [];

    // Связь с рецептами через RecipeIngredient
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = [];
}