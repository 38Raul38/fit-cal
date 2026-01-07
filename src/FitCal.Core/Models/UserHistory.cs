namespace FitCal.Core.Models;

public class UserHistory
{
    public int UserHistoryId { get; set; }
    public DateTime JournalDate { get; set; }
    
    public int FoodId { get; set; }
    public Food Food { get; set; }
    
    public int UserInformationId { get; set; }
    public UserInformation UserInformation { get; set; } = null!;
}