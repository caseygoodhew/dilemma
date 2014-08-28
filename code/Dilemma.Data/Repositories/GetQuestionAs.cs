namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Flags to generally specify the purpose of a call to <see cref="QuestionRepository.GetQuestion{T}"/> 
    /// to minimize the data needs to be pulled back from the database.
    /// </summary>
    public enum GetQuestionAs
    {
        AnswerCount,

        FullDetails
    }
}
