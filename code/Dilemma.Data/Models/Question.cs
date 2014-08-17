using System;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Data.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime ClosesDateTime { get; set; }

        public Category Category { get; set; }

        public int MaxAnswers { get; set; }
    }
}
