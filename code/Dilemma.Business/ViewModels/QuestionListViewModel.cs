﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    public class QuestionListViewModel
    {
        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}