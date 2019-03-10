using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Core22SwaggerWebApp.Models
{
    public sealed class ApiError
    {
        public const string ModelBindingErrorMessage = "Invalid parameters.";

        public ApiError()
        {
        }

        public ApiError(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new <see cref="ApiError"/> from the result of a model binding attempt.
        /// The first model binding error (if any) is placed in the <see cref="Detail"/> property.
        /// </summary>
        /// <param name="modelState"></param>
        public ApiError(ModelStateDictionary modelState)
        {
            Message = ModelBindingErrorMessage;

            Detail = modelState
                .FirstOrDefault(x => x.Value.Errors.Any())
                .Value?.Errors?.FirstOrDefault()?.ErrorMessage;
        }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Detail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }

        //public ModelStateDictionary ModelStateAsCamelCase(ModelStateDictionary modelState)
        //{
        //    var newModelStateDictionary = new ModelStateDictionary();

        //    foreach (var element in modelState)
        //    {
        //        if (!string.IsNullOrWhiteSpace(element.Key))
        //        {
        //            var keys = element.Key.Split('.');
        //            var camelKeys = new List<string>();

        //            foreach (var key in keys)
        //            {
        //                camelKeys.Add(key.First().ToString().ToLowerInvariant() + key.Substring(1));
        //            }

        //            // You can (add a / change this) code if the returned key is not
        //            // composed from the ObjectName.Property, such as when it is 
        //            // composed from the property name

        //            var newKey = camelKeys.Aggregate((i, j) => i + "." + j);

        //            newModelStateDictionary.AddModelError(newKey, element.Value.Errors.FirstOrDefault()?.ErrorMessage);
        //        }
        //        //else
        //        //{
        //        //    newModelStateDictionary.AddModelError(newKey, element.Value.Errors.FirstOrDefault()?.ErrorMessage);
        //        //}
        //    }

        //    return newModelStateDictionary;
        //}
    }
}
