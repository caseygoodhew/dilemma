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
        /// Saves a new <see cref="QuestionViewModel"/> instance.
        /// </summary>
        /// <param name="questionViewModel">The <see cref="QuestionViewModel"/> to save.</param>
        /// <returns>The new question id.</returns>
        int SaveNewQuestion(QuestionViewModel questionViewModel);

        /// <summary>
        /// Gets all questions as <see cref="QuestionViewModel"/>s.
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        IEnumerable<QuestionViewModel> GetAllQuestions();

        /// <summary>
        /// Gets questions as <see cref="QuestionViewModel"/>s.
        /// </summary>
        /// <param name="category">The category of questions to get, or null for 'all'.</param>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        IEnumerable<QuestionViewModel> GetQuestions(CategoryViewModel category);
        
        IEnumerable<QuestionViewModel> GetBookmarkedQuestions();

        /// <summary>
        /// Gets all questions that the current user has been involved in
        /// </summary>
        /// <returns>The <see cref="QuestionViewModel"/>s.</returns>
        IEnumerable<QuestionViewModel> GetMyActivity();

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
        /// Touches an answer so that the answer slot does not expire.
        /// </summary>
        /// <param name="answerId">The id of the answer to touch.</param>
        void TouchAnswer(int answerId);
        
        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerViewModel">The <see cref="AnswerViewModel"/> to save.</param>
        /// <returns>Flag indicating if the answer was completed.</returns>
        bool CompleteAnswer(int questionId, AnswerViewModel answerViewModel);

        /// <summary>
        /// Adds a followup to a question.
        /// </summary>
        /// <param name="questionId">The question id.</param>
        /// <param name="followupViewModel">The <see cref="FollowupViewModel"/> to save.</param>
        /// <returns>true if the followup was saved, false if a followup has already been added.</returns>
        bool AddFollowup(int questionId, FollowupViewModel followupViewModel);

        /// <summary>
        /// Registers a users vote for an answer.
        /// </summary>
        /// <param name="answerId">The answer to register the vote against.</param>
        void RegisterVote(int answerId);

        /// <summary>
        /// Deregisters a users vote for an answer.
        /// </summary>
        /// <param name="answerId">The answer to deregister the vote against.</param>
        void DeregisterVote(int answerId);

        /// <summary>
        /// Bookmarks a question
        /// </summary>
        /// <param name="questionId">The question to bookmark.</param>
        void AddBookmark(int questionId);

        /// <summary>
        /// Removes a question bookmark
        /// </summary>
        /// <param name="questionId">The question to remove the bookmark from.</param>
        void RemoveBookmark(int questionId);

	    void CloseQuestions();
    }
}
