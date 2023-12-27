using backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;
using System.Text.RegularExpressions;
//using System.Web.Http.ModelBinding;

namespace backend.Infrastructure
{
    public class CollectionUserDtoFileModelBinder : IModelBinder
    {
        public Task BindModelAsync( ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var id = (long)Convert.ToDouble(bindingContext.ValueProvider.GetValue("Id").FirstValue);
            var nickName = bindingContext.ValueProvider.GetValue("NickName").FirstValue;
            string phoneNumber = bindingContext.ValueProvider.GetValue("PhoneNumber").FirstValue;

            var _files = bindingContext.HttpContext.Request.Form.Files;
            var userDto = new UserDto
            {
                Id = id,
                NickName = nickName,
                PhoneNumber = phoneNumber
            };

            if (_files.Count > 0)
            {
                userDto.AvatarRelativePath = GlobalVariables.AvatarRelativePathAndName(id);
            }
            var fields = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
            {
                { "userDto", userDto.ToJson() }
            };
            var formCol = new FormCollection(fields, _files);
            bindingContext.Result = ModelBindingResult.Success(formCol);            
            return Task.CompletedTask;
        }

    }
}
