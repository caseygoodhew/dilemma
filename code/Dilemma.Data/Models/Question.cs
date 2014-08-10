using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Data.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        public string Text { get; set; }
    }
}
