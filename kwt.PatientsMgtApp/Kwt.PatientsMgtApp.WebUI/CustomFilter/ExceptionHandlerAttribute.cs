using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kwt.PatientsMgtApp.Core;
using Kwt.PatientsMgtApp.DataAccess.SQL;

namespace Kwt.PatientsMgtApp.WebUI.CustomFilter
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        // for any exception that is thrown by the app, use this to log it and direct the user to a friendly web message
        //protected override void OnException(ExceptionContext filterContext)

        readonly IExceptionLoggerRepository _exceptionLogger = new ExceptionLoggerRepository();
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                ExceptionLoggerObject logger = new ExceptionLoggerObject()
                {
                    ExceptionMessage = filterContext.Exception.Message,
                    ExceptionStackTrace = filterContext.Exception.StackTrace,
                    ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                };

                _exceptionLogger.AddExceptionLogger(logger);

                //filterContext.ExceptionHandled = true;
            }
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class GreaterDateAttribute : ValidationAttribute, IClientValidatable
    {
        public string EarlierDateField { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime? date = value != null ? (DateTime?)value : null;
            var earlierDateValue = validationContext.ObjectType.GetProperty(EarlierDateField)
                .GetValue(validationContext.ObjectInstance, null);
            DateTime? earlierDate = earlierDateValue != null ? (DateTime?)earlierDateValue : null;

            if (date.HasValue && earlierDate.HasValue && date <= earlierDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "greaterdate"
            };

            rule.ValidationParameters["earlierdate"] = EarlierDateField;

            yield return rule;
        }
    }
}