using System;

using Dilemma.Data.EntityFramework;

using Disposable.MessagePipe;

namespace Dilemma.Data
{
    internal abstract class DataMessageContext<TMessageTypeEnum> : MessageContext<TMessageTypeEnum> where TMessageTypeEnum : struct, IConvertible
    {
        public readonly DilemmaContext DataContext;

        protected DataMessageContext(TMessageTypeEnum messageType, DilemmaContext dataContext)
            : base(messageType)
        {
            DataContext = dataContext;
        }
    }
}