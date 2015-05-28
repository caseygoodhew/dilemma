namespace Dilemma.Business.WebPurify
{
    public enum WebPurifyStatus
    {
        // WebPurify statii
        Ok,
        InvalidApiKey,
        InactiveApiKey,
        ApiKeyNotSent,
        ServiceUnavailable,
        RateLimitExceeded,

        // Xml Issues
        ResponseNodeNotFound,
        StatusAttributeNotFound,
        UnknownStatusAttributeValue,
        ErrorNodeNotFound,
        CodeAttributeNodeFound,
        UnknownCodeAttributeValue,

        // Response issues
        BadResponseStatusCode
    }
}