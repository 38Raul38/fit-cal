namespace FitCal.Core.Models;

public class RecipeIngredient
{
    public int RecipeIngredientId { get; set; }
    
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int FoodId { get; set; }
    public virtual Food Food { get; set; }

    public double Quantity { get; set; } //сколько грамм/единиц продукта
}