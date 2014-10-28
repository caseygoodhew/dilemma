using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dilemma.Common
{
    /// <summary>
    /// Defines a route for notification types based on the controller and action to be called.
    /// </summary>
    public class NotificationRoute
    {
        /// <summary>
        /// The <see cref="NotificationType"/> represented by this route.
        /// </summary>
        public readonly NotificationType NotificationType;
        
        /// <summary>
        /// The MVC controller name.
        /// </summary>
        public readonly string Controller;

        /// <summary>
        /// The MVC controller action name.
        /// </summary>
        public readonly string Action;

        /// <summary>
        /// The MVC controller action route data key to be filled with the route data value for the given <see cref="NotificationType"/>.
        /// </summary>
        public readonly string RouteDataKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRoute"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType"/> represented by this route.</param>
        /// <param name="controller">The MVC controller name.</param>
        /// <param name="action">The MVC controller action name.</param>
        /// <param name="routeDataKey">The MVC controller action route data key to be filled with the route data value for the given <see cref="NotificationType"/>.</param>
        public NotificationRoute(NotificationType notificationType, string controller, string action, string routeDataKey = "")
        {
            NotificationType = notificationType; 
            Controller = controller;
            Action = action;
            RouteDataKey = routeDataKey;
        }

        /// <summary>
        /// Static factory method to create a new <see cref="NotificationRoute"/> from the given expression.
        /// </summary>
        /// <typeparam name="TController">Any class, but intended to be an MVC controller.</typeparam>
        /// <param name="notificationType">The <see cref="NotificationType"/> handled by this controller action.</param>
        /// <param name="expression">An expression pointing to the controller action. This must be a method and must take either 0 or 1 parameters.</param>
        /// <returns>The new <see cref="NotificationRoute"/>.</returns>
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

            var controllerName = methodInfo.DeclaringType.Name;

            if (controllerName.EndsWith("Controller"))
            {
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            }

            return new NotificationRoute(
                    notificationType,
                    controllerName,
                    methodInfo.Name,
                    methodInfo.GetParameters().Any() ? methodInfo.GetParameters().Single().Name : string.Empty);
        }
    }
}