using System;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Data.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
