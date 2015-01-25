using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class ControllerDescriptorMock : ControllerDescriptor
    {
        private readonly MethodCallExpression customAttributesProvider;

        public ControllerDescriptorMock(Expression<Action> actionExpression)
        {
            customAttributesProvider = actionExpression.Body as MethodCallExpression;
        }
        
        public override ActionDescriptor FindAction(ControllerContext controllerContext, string actionName)
        {
            throw new NotImplementedException();
        }

        public override ActionDescriptor[] GetCanonicalActions()
        {
            throw new NotImplementedException();
        }

        public override Type ControllerType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return customAttributesProvider.Method.GetCustomAttributes(attributeType, inherit);
        }
    }
}