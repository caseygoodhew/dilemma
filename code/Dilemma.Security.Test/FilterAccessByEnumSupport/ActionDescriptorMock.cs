using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public class ActionDescriptorMock : ActionDescriptor
    {
        private readonly MethodCallExpression customAttributesProvider;

        private readonly ControllerDescriptorMock conrollerDescriptor;

        public ActionDescriptorMock(ControllerDescriptorMock conrollerDescriptor, Expression<Action> actionExpression)
        {
            this.conrollerDescriptor = conrollerDescriptor;
            customAttributesProvider = actionExpression.Body as MethodCallExpression;
        }

        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            throw new NotImplementedException();
        }

        public override string ActionName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get
            {
                return conrollerDescriptor;
            }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return customAttributesProvider.Method.GetCustomAttributes(attributeType, inherit);
        }
    }
}