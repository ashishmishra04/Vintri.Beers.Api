using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Vintri.Beers.Model;

namespace Vintri.Beers.Api
{
    /// <summary>
    /// The UserName Check validation Filter
    /// </summary>
    public class ValidUserFilterAttribute : Attribute, IActionFilter
    {
        public bool AllowMultiple
        {
            get { return true; }
        }

        /// <summary>
        /// Execute Action Filter Async
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            var userRating = actionContext.ActionArguments["userRating"] as UserRating;
            if (userRating == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            if (string.IsNullOrEmpty(userRating.UserName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));

            if (!IsValidEmail(userRating.UserName))
            {
                actionContext.Response = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent($"Invalid username provided: {userRating.UserName}, valid email required")
                };

                return actionContext.Response;
            }

            var result = await continuation();
            return result;
        }

        //Make sure its a valid email address
        private bool IsValidEmail(string email)
        {
            var validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

            // More Accurate way would be to use 
            // MailAddress m = new MailAddress(email); and handle try catch
            return validateEmailRegex.IsMatch(email);
        }
    }
}