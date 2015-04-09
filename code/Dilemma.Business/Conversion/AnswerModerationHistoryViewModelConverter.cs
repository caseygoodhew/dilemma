using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Proxies;

using Disposable.Common.Conversion;

namespace Dilemma.Business.Conversion
{
    public static class AnswerModerationHistoryViewModelConverter
    {
        public static AnswerModerationHistoryViewModel FromAnswerHistoryProxy(AnswerHistoryProxy model)
        {
            return new AnswerModerationHistoryViewModel
                       {
                           Question = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model.Question),
                           Answer = ConverterFactory.ConvertOne<Answer, AnswerViewModel>(model.Answer),
                           ModerationEntries =
                               ConverterFactory.ConvertMany<ModerationEntry, ModerationEntryViewModel>(
                                   model.ModerationEntries).ToList()
                       };
        }
    }
}