using System;
using System.Collections.Generic;
using System.Linq;
using Dilemma.Business.WebPurify;
using Disposable.Common.ServiceLocator;
using FluentValidation.Validators;

namespace Dilemma.Business.Validators
{
    public class WebPurifyValidator : PropertyValidator
    {
        private static readonly string ErrorMessage = "'{PropertyName}' cannot contain the word(s) {Explicits}.";

        private static readonly Lazy<IWebPurifyResponder> WebPurifyResponder = Locator.Lazy<IWebPurifyResponder>();

        public WebPurifyValidator() : base(ErrorMessage) { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (context.PropertyValue == null)
            {
                return SetWebPurifyStatus(context, WebPurifyStatus.Ok, true);
            }

            var text = context.PropertyValue.ToString();

            if (string.IsNullOrEmpty(text))
            {
                return SetWebPurifyStatus(context, WebPurifyStatus.Ok, true);
            }

            IEnumerable<string> explicits;
            var result = true;
            var status = WebPurifyResponder.Value.Return(text, out explicits);

            var tidiedExplicits = explicits.Distinct().OrderBy(x => x).ToList();

            if (tidiedExplicits.Any())
            {
                context.MessageFormatter.AppendArgument("Explicits", string.Join(", ", tidiedExplicits.Select(x => string.Format("'{0}'", x))));
                result = false;
            }

            return SetWebPurifyStatus(context, status, result);
        }

        private static bool SetWebPurifyStatus(PropertyValidatorContext context, WebPurifyStatus status, bool isValid)
        {
            var webPurifiable = (context.Instance as IWebPurifyable);
            if (webPurifiable != null)
            {
                webPurifiable.WebPurifyStatus(status);
            }

            return isValid;
        }
    }
}