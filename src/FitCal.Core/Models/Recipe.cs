namespace FitCal.Core.Models;

public class Recipe
{
    public int RecipeId { get; set; }
    public string RecipeName { get; set; }
    public double ServingSize { get; set; }
    public string ServingUnit { get; set; }
    
    public int UserInformationId { get; set; }
    public virtual UserInformation UserInformation { get; set; }
    
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    public virtual ICollection<UserHistory> UserHistories { get; set; }
}