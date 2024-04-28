using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NafanyaVPN.Utils;

public static class ModelValidationLogBuilderBuilderUtils
{
    public static string GetModelValidationLog(ModelStateDictionary modelState)
    {
        var allValues = modelState
            .Select(x => new
            {
                Field = x.Key, Value = (x.Value.RawValue ?? string.Empty).ToString(),
                Error = x.Value.Errors.FirstOrDefault()?.ErrorMessage
            });

        var logBuilder = new StringBuilder();
        foreach (var value in allValues)
        {
            var error = string.IsNullOrEmpty(value.Error)
                ? string.Empty
                : $", Error: {value.Error}";
            
            logBuilder.AppendLine($"Field: {value.Field}, Value: {value.Value}{error}");
        }
        
        return logBuilder.ToString();
    }
}