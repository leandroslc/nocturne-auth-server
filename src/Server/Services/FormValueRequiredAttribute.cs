using System;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Routing;

namespace Nocturne.Auth.Server.Services
{
    public sealed class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string name;

        public FormValueRequiredAttribute(string name)
        {
            this.name = name;
        }

        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            if (IsMethodNotAllowed(routeContext))
            {
                return false;
            }

            var contentType = routeContext.HttpContext.Request.ContentType;

            if (string.IsNullOrEmpty(contentType))
            {
                return false;
            }

            if (!IsFormUrlEncodedContentType(contentType))
            {
                return false;
            }

            return !string.IsNullOrEmpty(routeContext.HttpContext.Request.Form[name]);
        }

        private static bool IsMethodNotAllowed(RouteContext context)
        {
            return IsRequestMethodEqual(context, "GET")
                || IsRequestMethodEqual(context, "HEAD")
                || IsRequestMethodEqual(context, "DELETE")
                || IsRequestMethodEqual(context, "TRACE");
        }

        private static bool IsFormUrlEncodedContentType(string contentType)
        {
            return contentType.StartsWith(
                "application/x-www-form-urlencoded",
                StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool IsRequestMethodEqual(RouteContext context, string method)
        {
            return string.Equals(
                context.HttpContext.Request.Method,
                method,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
