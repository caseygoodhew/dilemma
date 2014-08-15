using System;

namespace Dilemma.Business.ViewModels
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        
        public string Text { get; set; }

        public DateTime? CreatedDateTime { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
