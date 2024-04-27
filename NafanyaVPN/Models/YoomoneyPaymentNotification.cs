namespace NafanyaVPN.Models;

public record YoomoneyPaymentNotification(
    string? NotificationType,
    string? BillId,
    decimal? Amount,
    DateTime? DateTime,
    bool? Codepro,
    string? Sender,
    string? Sha1Hash,
    bool? TestNotification,
    string? OperationLabel,
    string? OperationId,
    int? Currency,
    string? Label)
{
    public string? NotificationType { get; set; } = NotificationType;
    public string? BillId { get; set; } = BillId;
    public decimal? Amount { get; set; } = Amount;
    public DateTime? DateTime { get; set; } = DateTime;
    public bool? Codepro { get; set; } = Codepro;
    public string? Sender { get; set; } = Sender;
    public string? Sha1Hash { get; set; } = Sha1Hash;
    public bool? TestNotification { get; set; } = TestNotification;
    public string? OperationLabel { get; set; } = OperationLabel;
    public string? OperationId { get; set; } = OperationId;
    public int? Currency { get; set; } = Currency;
    public string? Label { get; set; } = Label;
}