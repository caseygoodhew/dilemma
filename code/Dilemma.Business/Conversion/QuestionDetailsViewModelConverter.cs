﻿using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Security;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="QuestionDetailsViewModel"/>.
    /// </summary>
    public static class QuestionDetailsViewModelConverter
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);

        private static readonly Lazy<ISecurityManager> SecurityManager = new Lazy<ISecurityManager>(Locator.Current.Instance<ISecurityManager>);

        /// <summary>
        /// Converts a <see cref="QuestionDetailsViewModel"/> to a <see cref="Question"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="QuestionDetailsViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Question"/>.</returns>
        public static Question ToQuestion(QuestionDetailsViewModel viewModel)
        {
            var question = ConverterFactory.ConvertOne<QuestionViewModel, Question>(viewModel.QuestionViewModel);

            if (viewModel.Answer != null && viewModel.Answer.AnswerId != null)
            {
                question.Answers.Add(ConverterFactory.ConvertOne<AnswerViewModel, Answer>(viewModel.Answer));
            }

            return question;
        }

        /// <summary>
        /// Converts a <see cref="Question"/> to a <see cref="QuestionDetailsViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Question"/> to convert.</param>
        /// <returns>The resultant <see cref="QuestionDetailsViewModel"/>.</returns>
        public static QuestionDetailsViewModel FromQuestion(Question model)
        {
            return new QuestionDetailsViewModel
                       {
                           QuestionViewModel = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model),
                           CanAnswer = CanAnswer(model)
                       };
        }

        private static bool CanAnswer(Question model)
        {
            if (model.TotalAnswers > model.MaxAnswers)
            {
                return false;
            }

            if (model.ClosesDateTime < TimeSource.Value.Now)
            {
                return false;
            }

            var userId = SecurityManager.Value.GetUserId();
            
            if (model.User.UserId == userId)
            {
                return false;
            }

            if (model.Answers.Any(x => x.User.UserId == userId))
            {
                return false;
            }

            return true;
        }
    }
}