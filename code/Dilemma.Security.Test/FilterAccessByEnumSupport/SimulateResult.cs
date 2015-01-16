namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public class SimulateResult
    {
        public readonly FilterAccessStub Controller;

        public readonly FilterAccessStub Action;

        public SimulateResult(FilterAccessStubAttribute controller, FilterAccessStubAttribute action)
        {
            if (controller != null)
            {
                Controller = ((FilterAccessStub)controller.FilterAccessByEnum);
            }

            if (action != null)
            {
                Action = ((FilterAccessStub)action.FilterAccessByEnum);
            }
        }
    }
}