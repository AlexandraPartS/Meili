using backend.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NuGet.Protocol;
using System.Text.RegularExpressions;

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
            var phoneNumber = bindingContext.ValueProvider.GetValue("PhoneNumber").FirstValue;

            if (string.IsNullOrEmpty(nickName))
            {
                return Task.CompletedTask;
            }
            else if (nickName.Length < 2 || nickName.Length > 128)
            {
                return Task.CompletedTask;
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return Task.CompletedTask;
            }
            if (!Regex.IsMatch(phoneNumber, GlobalVariables.RegexPhonePattern))
            {
                return Task.CompletedTask;
            }

            var _files = bindingContext.HttpContext.Request.Form.Files;
            var userDto = new UserDto { Id = id, NickName = nickName, PhoneNumber = phoneNumber };
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
