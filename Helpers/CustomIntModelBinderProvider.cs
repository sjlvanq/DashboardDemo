using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DashboardDemo.Helpers
{
    public class CustomIntModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(int))
            {
                return new CustomIntModelBinder();
            }

            return null;
        }
    }

}
