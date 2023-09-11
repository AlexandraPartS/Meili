using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.DotNet.Scaffolding.Shared;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Web.Http.Controllers;
using System.Xml.Linq;

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
            else if (nickName.Length < 2 || nickName.Length > 25)
            {
                return Task.CompletedTask;
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return Task.CompletedTask;
            }

            var _files = bindingContext.HttpContext.Request.Form.Files;
            var userDto = new UserDto { Id = id, NickName = nickName, PhoneNumber = phoneNumber };
            if (_files.Count > 0)
            {
                userDto.AvatarRelativePath = GlobalVariables.AvatarRelativePath;
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
