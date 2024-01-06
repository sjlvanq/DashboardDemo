using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DashboardDemo.Helpers
{
    public class CustomIntModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            var valueAsString = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(valueAsString))
            {
                return Task.CompletedTask;
            }

            if (!int.TryParse(valueAsString, out var intValue))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "El valor no es un número válido.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(intValue);
            return Task.CompletedTask;
        }
    }
}
