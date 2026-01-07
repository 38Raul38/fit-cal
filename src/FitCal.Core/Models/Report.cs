namespace FitCal.Core.Models;

public class Report
{
    public int ReportId { get; set; }
    public string ReportName { get; set; }
    public DateTime ReportDate { get; set; }

    // Ссылка на UserHistory (опционально)
    public int? UserHistoryId { get; set; }
    public UserHistory UserHistory { get; set; }

    // Ссылка на пользователя
    public int UserInformationId { get; set; }
    public UserInformation UserInformation { get; set; }
}