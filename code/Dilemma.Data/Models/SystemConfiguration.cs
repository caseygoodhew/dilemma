﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Common;

namespace Dilemma.Data.Models
{
    public class SystemConfiguration
    {
        public int Id 
        { 
            get { return 1; }
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        public int MaxAnswers { get; set; }

        public QuestionLifetime QuestionLifetime { get; set; }

        public SystemEnvironment SystemEnvironment { get; set; }
    }
}