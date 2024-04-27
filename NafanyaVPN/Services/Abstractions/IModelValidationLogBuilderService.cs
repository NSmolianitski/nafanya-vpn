using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NafanyaVPN.Services;

public interface IModelValidationLogBuilderService
{
    string GetModelValidationLog(ModelStateDictionary modelState);
}