using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    public class AnswerViewModel
    {
        public int? AnswerId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
