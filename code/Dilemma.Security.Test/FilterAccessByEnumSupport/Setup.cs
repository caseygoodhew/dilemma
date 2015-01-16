using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public class Setup : ActionExecutingContext
    {
        private readonly ActionDescriptorMock actionDescriptor;
        
        private Setup(AllowDenyXYZ controllerAction, AllowDenyXYZ actionAction)
        {
            var controllerDescriptor = new ControllerDescriptorMock(GetMethodExpression(controllerAction));
            actionDescriptor = new ActionDescriptorMock(controllerDescriptor, GetMethodExpression(actionAction));
        }

        private static Expression<Action> GetMethodExpression(AllowDenyXYZ action)
        {
            switch (action)
            {
                case AllowDenyXYZ.AllowXY:
                    return () => ControllerActionStub.AllowXY();
                case AllowDenyXYZ.AllowX:
                    return () => ControllerActionStub.AllowX();
                case AllowDenyXYZ.DenyXY:
                    return () => ControllerActionStub.DenyXY();
                case AllowDenyXYZ.DenyX:
                    return () => ControllerActionStub.DenyX();
                case AllowDenyXYZ.AllowZ:
                    return () => ControllerActionStub.AllowZ();
                case AllowDenyXYZ.DenyZ:
                    return () => ControllerActionStub.DenyZ();
                case AllowDenyXYZ.NotSet:
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

        public static Setup ControllerAction(AllowDenyXYZ controller, AllowDenyXYZ action)
        {
            return new Setup(controller, action);
        }

        public static Setup Controller(AllowDenyXYZ controller)
        {
            return new Setup(controller, AllowDenyXYZ.NotSet);
        }

        public static Setup Action(AllowDenyXYZ action)
        {
            return new Setup(AllowDenyXYZ.NotSet, action);
        }

        public FilterAccessStubAttribute ControllerAttribute()
        {
            return actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(FilterAccessStubAttribute), true).Cast<FilterAccessStubAttribute>().SingleOrDefault();
        }

        public FilterAccessStubAttribute ActionAttribute()
        {
            return actionDescriptor.GetCustomAttributes(typeof(FilterAccessStubAttribute), true).Cast<FilterAccessStubAttribute>().SingleOrDefault();
        }

        public SimulateResult Simulate()
        {
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

            return new SimulateResult(controllerAttribute, actionAttribute);
        }
    }
}