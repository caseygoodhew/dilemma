using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using Dilemma.Security.AccessFilters.ByEnum;
using Dilemma.Security.AccessFilters.ByUserRole;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class SetupUserRole : ActionExecutingContext
    {
        private readonly ActionDescriptorMock actionDescriptor;

        private SetupUserRole(AllowDenyRole controllerAction, AllowDenyRole actionAction)
        {
            var controllerDescriptor = new ControllerDescriptorMock(GetMethodExpression(controllerAction));
            actionDescriptor = new ActionDescriptorMock(controllerDescriptor, GetMethodExpression(actionAction));
        }

        private static Expression<Action> GetMethodExpression(AllowDenyRole action)
        {
            switch (action)
            {
                case AllowDenyRole.AllowAdministrator:
                    return () => ControllerActionStub.AllowAdministrator();
                case AllowDenyRole.DenyAdministrator:
                    return () => ControllerActionStub.DenyAdministrator();
                case AllowDenyRole.NotSet:
                    return () => ControllerActionStub.NotSet();
                default:
                    throw new ArgumentOutOfRangeException("action");
            }
        }

        public override ActionDescriptor ActionDescriptor
        {
            get
            {
                return actionDescriptor;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public static SetupUserRole ControllerAction(AllowDenyRole controller, AllowDenyRole action)
        {
            return new SetupUserRole(controller, action);
        }

        public static SetupUserRole Controller(AllowDenyRole controller)
        {
            return new SetupUserRole(controller, AllowDenyRole.NotSet);
        }

        public static SetupUserRole Action(AllowDenyRole action)
        {
            return new SetupUserRole(AllowDenyRole.NotSet, action);
        }

        public FilterAccessByUserRoleAttribute ControllerAttribute()
        {
            return actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(FilterAccessByUserRoleAttribute), true).Cast<FilterAccessByUserRoleAttribute>().SingleOrDefault();
        }

        public FilterAccessByUserRoleAttribute ActionAttribute()
        {
            return actionDescriptor.GetCustomAttributes(typeof(FilterAccessByUserRoleAttribute), true).Cast<FilterAccessByUserRoleAttribute>().SingleOrDefault();
        }

        public SimulateResult<FilterAccessBySystemEnvironment> Simulate()
        {
            Result = null;
            
            var controllerAttribute = ControllerAttribute();
            if (controllerAttribute != null)
            {
                controllerAttribute.OnActionExecuting(this);
            }

            var actionAttribute = ActionAttribute();
            if (actionAttribute != null)
            {
                actionAttribute.OnActionExecuting(this);
            }

            return new SimulateResult<FilterAccessBySystemEnvironment>(this, controllerAttribute, actionAttribute);
        }
    }
}