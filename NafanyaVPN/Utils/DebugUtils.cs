using yoomoney_api.quickpay;

namespace NafanyaVPN.Utils;

public static class DebugUtils
{
    public static async Task SendPaymentSuccessNotification(Quickpay quickpay)
    {
        using var client = new HttpClient();
        
        var notificationValues = new Dictionary<string, string>
        {
            {"notification_type", "p2p-incoming"},
            {"bill_id", ""},
            {"amount", $"{quickpay.Sum}"},
            {"datetime", $"{DateTimeUtils.GetMoscowNowTime()}"},
            {"codepro", "false"},
            {"sender", "41001000040"},
            {"sha1_hash", "9860448f-7fb7-4d30-949f-241454851273"},
            {"test_notification", "false"},
            {"operation_label", ""},
            {"operation_id", ""},
            {"currency", "643"},
            {"label", $"{quickpay.Label}"}
        };
        var content = new FormUrlEncodedContent(notificationValues);
        var response = await client.PostAsync("http://localhost:8080/api/v1/payment-notification", content);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Ответ на уведомление об успешной оплате:" + responseContent);
        }
        else
        {
            Console.WriteLine("Код ошибки при отправке уведомления об успешной оплате: " + response.StatusCode);
        }
    }
}