using System.Collections.Generic;

using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Question repository services.
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        /// Creates a <see cref="Question"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="Question"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user creating the question.</param>
        /// <param name="questionType">The convertable instance.</param>
        /// <returns>The new question id.</returns>
        int CreateQuestion<T>(int userId, T questionType) where T : class;

        /// <summary>
        /// Gets the <see cref="Question"/> in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The user id of the user requesting the question.</param>
        /// <param name="questionId">The id of the question to get.</param>
        /// <param name="config">Options for what extended data to include.</param>
        /// <returns>The <see cref="Question"/> converted to type T.</returns>
        T GetQuestion<T>(int userId, int questionId, GetQuestionAs config) where T : class;

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        IEnumerable<T> QuestionList<T>(int maximum) where T : class;

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="userId">The id of user to get the questions for.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        IEnumerable<T> QuestionList<T>(int userId, int maximum) where T : class;

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <typeparam name="TC">The <see cref="category"/> type.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="category">The category of question to get.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        IEnumerable<T> QuestionList<T, TC>(TC category, int maximum) where T : class where TC : class;

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="maximum">The maximum number of questions to return.</param>
        /// <param name="questionIds">The ids of question to get.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        IEnumerable<T> QuestionList<T>(IEnumerable<int> questionIds, int maximum) where T : class;

        /// <summary>
        /// Requests an an slot for the given question id. If no slot is available, null is returned.
        /// </summary>
        /// <param name="userId">The id of the user requesting the answer slot.</param>
        /// <param name="questionId">The question id.</param>
        /// <returns>The answer id if a slot is available or null if it is not available.</returns>
        int? RequestAnswerSlot(int userId, int questionId);

        /// <summary>
        /// Gets an answer in progress by question id and answer id. The provided answer id must be an answer of the question id.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of the user who is requesting the answer.</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerId">The answer id.</param>
        /// <returns>The <see cref="Answer"/> converted to type T.</returns>
        T GetAnswerInProgress<T>(int userId, int questionId, int answerId) where T : class;

        /// <summary>
        /// Touches an answer so that the answer slot does not expire.
        /// </summary>
        /// <param name="userId">The user who is touching the answer.</param>
        /// <param name="answerId">The id of the answer to touch.</param>
        void TouchAnswer(int userId, int answerId);
        
        /// <summary>
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user who is completing the answer.</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerType">The convertable instance.</param>
        /// <returns>true if the answer was saved, false if the answer slot was no longer available.</returns>
        bool CompleteAnswer<T>(int userId, int questionId, T answerType) where T : class;

        /// <summary>
        /// Adds a followup to a question.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user who is completing the followup (this must be the question owner).</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="followupType">The convertable instance.</param>
        /// <returns>true if the followup was saved, false if a followup has already been added.</returns>
        bool AddFollowup<T>(int userId, int questionId, T followupType) where T : class;
        
        /// <summary>
        /// Registers a users vote for an answer.
        /// </summary>
        /// <param name="userId">The user id of the user that is voting.</param>
        /// <param name="answerId">The answer to register the vote against.</param>
        void RegisterVote(int userId, int answerId);

        /// <summary>
        /// Deregisters a users vote for an answer.
        /// </summary>
        /// <param name="userId">The user id of the user that is deregistering their vote.</param>
        /// <param name="answerId">The answer to deregister the vote against.</param>
        void DeregisterVote(int userId, int answerId);

        /// <summary>
        /// Gets the user id of the user who posted the question.
        /// </summary>
        /// <param name="answerId">The answer id of the question to find.</param>
        /// <returns>The user id of the user who posted the questions.</returns>
        int GetQuestionUserIdByAnswerId(int answerId);

        /// <summary>
        /// Bookmarks a question
        /// </summary>
        /// <param name="userId">The user id of the user that is bookmarking.</param>
        /// <param name="questionId">The question to bookmark.</param>
        void AddBookmark(int userId, int questionId);

        /// <summary>
        /// Removes a question bookmark
        /// </summary>
        /// <param name="userId">The user id of the user that is removing the bookmark.</param>
        /// <param name="questionId">The question to remove the bookmark from.</param>
        void RemoveBookmark(int userId, int questionId);

        IEnumerable<int> GetBookmarkedQuestionIds(int userId);

		void CloseQuestions();
    }
}
