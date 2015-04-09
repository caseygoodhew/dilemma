using System;
using System.Collections.Generic;

using Dilemma.Common;

namespace Dilemma.Business.Utils
{
    internal class NotificationMessageFactory : INotificationMessageFactory
    {
        private readonly Dictionary<Tuple<NotificationType, NotificationTarget>, Func<int, string>> messageDictionary =
            new Dictionary<Tuple<NotificationType, NotificationTarget>, Func<int, string>>();

        public NotificationMessageFactory()
        {
            AddEntry(NotificationType.QuestionApproved, NotificationTarget.Questioner, x => "Your dilemma has been approved");
            AddEntry(NotificationType.QuestionRejected, NotificationTarget.Questioner, x => "Your dilemma has been rejected - Find out why");

            AddEntry(NotificationType.AnswerApproved, NotificationTarget.Questioner, x => x == 1 ? "An answer has been added to your dilemma" : string.Format("{0} answers have been added to your dilemma", x));
            AddEntry(NotificationType.AnswerApproved, NotificationTarget.Bookmarker, x => x == 1 ? "An answer has been added to a bookmarked dilemma" : string.Format("{0} answers have been added to a bookmarked dilemma", x));
            AddEntry(NotificationType.AnswerApproved, NotificationTarget.Answerer, x => "Your answer has been approved");
            AddEntry(NotificationType.AnswerRejected, NotificationTarget.Answerer, x => "Your answer has been rejected - Find out why");

            AddEntry(NotificationType.FollowupApproved, NotificationTarget.Questioner, x => "Your followup has been approved");
            AddEntry(NotificationType.FollowupApproved, NotificationTarget.Bookmarker, x => "A followup has been added to a bookmarked dilemma");
            AddEntry(NotificationType.FollowupApproved, NotificationTarget.Answerer, x => "A followup has been added to a dilemma you've answered");
            AddEntry(NotificationType.FollowupRejected, NotificationTarget.Questioner, x => "Your followup has been rejected - Find out why");

            //AddEntry(NotificationType.FlaggedQuestionApproved,
            //AddEntry(NotificationType.FlaggedAnswerApproved,
            //AddEntry(NotificationType.FlaggedFollowupApproved,
            //AddEntry(NotificationType.FlaggedQuestionRejected,
            //AddEntry(NotificationType.FlaggedAnswerRejected,
            //AddEntry(NotificationType.FlaggedFollowupRejected,

            AddEntry(NotificationType.OpenForVoting, NotificationTarget.Questioner, x => "Your dilemma is open for voting");
            AddEntry(NotificationType.OpenForVoting, NotificationTarget.Answerer, x => "A dilemma you've answered is open for voting");
            AddEntry(NotificationType.OpenForVoting, NotificationTarget.Bookmarker, x => "A bookmarked dilemma is open for voting");

            AddEntry(NotificationType.VoteOnAnswer, NotificationTarget.Answerer, x => x == 1 ? "Your answer got a vote!" : string.Format("Your answer has {0} votes!", x));

            AddEntry(NotificationType.BestAnswerAwarded, NotificationTarget.Questioner, x => "It's time to add your followup!");
            AddEntry(NotificationType.BestAnswerAwarded, NotificationTarget.Answerer, x => "Your answer is the best!");
            AddEntry(NotificationType.BestAnswerAwarded, NotificationTarget.Bookmarker, x => "The best answer has been aswarded to a bookmarked dilemma");
        }

        private void AddEntry(
            NotificationType notificationType,
            NotificationTarget notificationTarget,
            Func<int, string> messageFunc)
        {
            messageDictionary.Add(Tuple.Create(notificationType, notificationTarget), messageFunc);
        }

        public string GetMessage(NotificationType notificationType, NotificationTarget notificationTarget, int occurrences)
        {
            var func = messageDictionary[Tuple.Create(notificationType, notificationTarget)];
            return func(occurrences);
        }
    }
}
