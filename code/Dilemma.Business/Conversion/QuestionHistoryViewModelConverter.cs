using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Proxies;

using Disposable.Common.Conversion;

namespace Dilemma.Business.Conversion
{
    public static class QuestionModerationHistoryViewModelConverter
    {
        public static QuestionModerationHistoryViewModel FromQuestionHistoryProxy(QuestionHistoryProxy model)
        {
            return new QuestionModerationHistoryViewModel
                       {
                           Question = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model.Question),
                           ModerationEntries =
                               ConverterFactory.ConvertMany<ModerationEntry, ModerationEntryViewModel>(
                                   model.ModerationEntries).ToList()
                       };
        }
    }
}