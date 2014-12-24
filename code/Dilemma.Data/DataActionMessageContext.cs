using Dilemma.Data.EntityFramework;

using Disposable.MessagePipe;

namespace Dilemma.Data
{
    internal abstract class DataActionMessageContext<TMessageTypeEnum> : MessageContext<TMessageTypeEnum>
    {
        public readonly DilemmaContext DilemmaContext;
        
        protected DataActionMessageContext(DilemmaContext dilemmaContext, TMessageTypeEnum messageType)
            : base(messageType)
        {
            DilemmaContext = dilemmaContext;
        }
    }
}