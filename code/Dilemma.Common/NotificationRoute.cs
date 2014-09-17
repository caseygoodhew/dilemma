using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

using Disposable.Common;

namespace Dilemma.Common
{
    public class NotificationRoute
    {
        public readonly NotificationType NotificationType;
        
        public readonly string Controller;

        public readonly string Action;

        public readonly string RouteDataKey;

        public NotificationRoute(NotificationType notificationType, string controller, string action, string routeDataKey = "")
        {
            NotificationType = notificationType; 
            Controller = controller;
            Action = action;
            RouteDataKey = routeDataKey;
        }

        public static NotificationRoute Create<TController>(
            NotificationType notificationType,
            Expression<Action<TController>> expression) where TController : class
        {
            var member = expression.Body as MethodCallExpression;

            if (member == null)
            {
                throw new ArgumentException("Expression is not a method", "expression");
            }
            
            var methodInfo = member.Method;
            
            var parameters = methodInfo.GetParameters();

            if (parameters.Length > 1)
            {
                throw new InvalidOperationException("NotificationRoute only supports methods with zero or one route parameters.");
            }

            return new NotificationRoute(
                    notificationType,
                    methodInfo.DeclaringType.Name,
                    methodInfo.Name,
                    methodInfo.GetParameters().Any() ? methodInfo.GetParameters().First().Name : string.Empty);
        }
    }
}