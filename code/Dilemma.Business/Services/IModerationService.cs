using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    public interface IModerationService
    {
        ModerationViewModel GetNext();

        void Approve(int moderationId);

        void Reject(int moderationId, string message);
    }
}
