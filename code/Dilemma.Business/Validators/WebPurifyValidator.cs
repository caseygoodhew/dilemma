using System;
using System.Collections.Generic;
using System.Linq;
using Dilemma.Business.Services;
using Dilemma.Business.WebPurify;
using Dilemma.Logging;
using Disposable.Common.ServiceLocator;
using FluentValidation.Validators;

namespace Dilemma.Business.Validators
{
    public class WebPurifyValidator : PropertyValidator
    {
        private static readonly string ErrorMessage = "'{PropertyName}' cannot contain the word(s) {Explicits}.";

		private static readonly Lazy<IAdministrationService> AdministrationService = Locator.Lazy<IAdministrationService>();
		
		private static readonly Lazy<IWebPurifyResponder> WebPurifyResponder = Locator.Lazy<IWebPurifyResponder>();

        private static readonly Lazy<ILogger> Logger = Locator.Lazy<ILogger>();

        public WebPurifyValidator() : base(ErrorMessage) { }

        protected override bool IsValid(PropertyValidatorContext context)
        {
	        var systemConfigurationViewModel = AdministrationService.Value.GetSystemServerConfiguration().SystemConfigurationViewModel;

			if (!systemConfigurationViewModel.EnableWebPurify)
	        {
		        return true;
	        }
			
			if (context.PropertyValue == null)
            {
                return true;
            }

            var text = context.PropertyValue.ToString();

            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            IEnumerable<string> explicits;
            var result = true;
            var status = WebPurifyResponder.Value.Return(text, out explicits);

            var tidiedExplicits = explicits.Distinct().OrderBy(x => x).ToList();
            var explicitsString = string.Join(", ", tidiedExplicits.Select(x => string.Format("'{0}'", x)));

            if (status == WebPurifyStatus.Ok)
            {
                Logger.Value.Info(string.Format("WebPurifyStatus.Ok : {0} explicits : {1}", tidiedExplicits.Count, explicitsString));
            }
            else
            {
                Logger.Value.Warn(string.Format("WebPurifyStatus.{0}", status));
            }
            
            if (tidiedExplicits.Any())
            {
                context.MessageFormatter.AppendArgument("Explicits", explicitsString);
                result = false;
            }

            var webPurifiable = (context.Instance as IWebPurifyable);
            if (webPurifiable != null)
            {
                webPurifiable.SetWebPurifyStatus(status);
            }

            return result;
        }
    }
}