using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class SetupEnum : ActionExecutingContext
    {
        private readonly ActionDescriptorMock actionDescriptor;
        
        private SetupEnum(AllowDenyXYZ controllerAction, AllowDenyXYZ actionAction)
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

        public static SetupEnum ControllerAction(AllowDenyXYZ controller, AllowDenyXYZ action)
        {
            return new SetupEnum(controller, action);
        }

        public static SetupEnum Controller(AllowDenyXYZ controller)
        {
            return new SetupEnum(controller, AllowDenyXYZ.NotSet);
        }

        public static SetupEnum Action(AllowDenyXYZ action)
        {
            return new SetupEnum(AllowDenyXYZ.NotSet, action);
        }

        public FilterAccessByEnumStubAttribute ControllerAttribute()
        {
            return actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(FilterAccessByEnumStubAttribute), true).Cast<FilterAccessByEnumStubAttribute>().SingleOrDefault();
        }

        public FilterAccessByEnumStubAttribute ActionAttribute()
        {
            return actionDescriptor.GetCustomAttributes(typeof(FilterAccessByEnumStubAttribute), true).Cast<FilterAccessByEnumStubAttribute>().SingleOrDefault();
        }

        public SimulateResult<FilterAccessStub> Simulate()
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

            return new SimulateResult<FilterAccessStub>(this, controllerAttribute, actionAttribute);
        }
    }
}