using System.Collections.Generic;

namespace Dilemma.Business.WebPurify
{
    public interface IWebPurifyResponder
    {
        WebPurifyStatus Return(string text, out IEnumerable<string> result);
    }

    public interface IWebPurifyable
    {
        void SetWebPurifyStatus(WebPurifyStatus status);
    }
}