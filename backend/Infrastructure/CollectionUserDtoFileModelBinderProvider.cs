using backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace backend.Infrastructure
{
    public class CollectionUserDtoFileModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Metadata.ModelType == typeof(IFormCollection))
            {
                return new BinderTypeModelBinder(typeof(CollectionUserDtoFileModelBinder));
            }
            return null;
        }
    }
}
