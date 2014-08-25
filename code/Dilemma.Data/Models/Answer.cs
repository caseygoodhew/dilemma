using System;
using System.ComponentModel.DataAnnotations;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public Question Question { get; set; }

        public AnswerType AnswerType { get; set; }
    }
}
