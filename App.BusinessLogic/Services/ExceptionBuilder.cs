using FluentValidation;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace App.BusinessLogic.Services
{
    internal static class ExceptionBuilder
    {
        internal static ValidationException Create(string message, HttpStatusCode code = HttpStatusCode.NotFound)
        {
            var exception = new ValidationException(message);
            exception.Data.Add("StatusCodeFromValidation", code);
            return exception;
        }
    }
}
