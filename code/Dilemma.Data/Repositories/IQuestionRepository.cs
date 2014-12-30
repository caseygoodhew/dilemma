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
        /// <param name="questionId">The id of the question to get.</param>
        /// <param name="config">Options for what extended data to include.</param>
        /// <returns>The <see cref="Question"/> converted to type T.</returns>
        T GetQuestion<T>(int questionId, GetQuestionAs config) where T : class;

        /// <summary>
        /// Gets the <see cref="Question"/> list in the specified type. There must be a converter registered between <see cref="Question"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <param name="userId">The id of user to get the activity for. If this is null, all questions will be returned.</param>
        /// <returns>A list of <see cref="Question"/>s converted to type T.</returns>
        IEnumerable<T> QuestionList<T>(int? userId) where T : class;

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
        /// Completes an answer that is in an initial 'Answer slot' state.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="userId">The id of the user who is completing the answer.</param>
        /// <param name="questionId">The question id.</param>
        /// <param name="answerType">The convertable instance.</param>
        /// <returns>true if the answer was saved, false if the answer slot was no longer available.</returns>
        bool CompleteAnswer<T>(int userId, int questionId, T answerType) where T : class;
    }
}
