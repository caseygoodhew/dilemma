using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    /// <summary>
    /// Question service provider interface.
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Initializes or reinitializes a <see cref="CreateQuestionViewModel"/>. Reinitialization allows a view model to return the correct state on POST validation.
        /// </summary>
        /// <param name="questionViewModel">(Optional) The <see cref="CreateQuestionViewModel"/> to reinitialize.</param>
        /// <returns>The <see cref="CreateQuestionViewModel"/>.</returns>
        CreateQuestionViewModel InitNewQuestion(CreateQuestionViewModel questionViewModel = null);
        
        /// <summary>
        /// Saves a new <see cref="CreateQuestionViewModel"/> instance.
        /// </summary>
        /// <param name="questionViewModel">The <see cref="CreateQuestionViewModel"/> to save.</param>
        void SaveNewQuestion(CreateQuestionViewModel questionViewModel);

        /// <summary>
        /// Gets all questions as <see cref="QuestionViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        IEnumerable<QuestionViewModel> GetAllQuestions();

        /// <summary>
        /// Gets a single <see cref="QuestionDetailsViewModel"/> by id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The <see cref="QuestionDetailsViewModel"/>.</returns>
        QuestionDetailsViewModel GetQuestion(int questionId);
        
        /// <summary>
        /// Requests an an slot for the given question id. If no slot is available, null is returned.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <returns>The answer id if a slot is available or null if it is not available.</returns>
        int? RequestAnswerSlot(int questionId);

        /// <summary>
        /// Gets an answer in progress by question id and answer id. The provided answer id must be an answer of the question id.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerId">The answer id.</param>
        /// <returns>The <see cref="AnswerViewModel"/>.</returns>
        AnswerViewModel GetAnswerInProgress(int questionId, int answerId);

        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerViewModel">The <see cref="AnswerViewModel"/> to save.</param>
        void CompleteAnswer(int questionId, AnswerViewModel answerViewModel);
    }
}
