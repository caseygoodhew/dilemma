using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Proxies;

using Disposable.Common.Conversion;

namespace Dilemma.Business.Conversion
{
    public static class FollowupModerationHistoryViewModelConverter
    {
        public static FollowupModerationHistoryViewModel FromFollowupHistoryProxy(FollowupHistoryProxy model)
        {
            return new FollowupModerationHistoryViewModel
                       {
                           Question = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model.Question),
                           Followup = ConverterFactory.ConvertOne<Followup, FollowupViewModel>(model.Followup),
                           ModerationEntries =
                               ConverterFactory.ConvertMany<ModerationEntry, ModerationEntryViewModel>(
                                   model.ModerationEntries).ToList()
                       };
        }
    }
}