using System;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    public abstract class NonConstructableException : Exception
    {
        protected NonConstructableException()
        {
            throw new InvalidOperationException("This exception should never be constructed. It should only be used in the ExpectedException attribute.");
        }
    }
}