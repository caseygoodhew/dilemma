using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Secondary
{
    [TestClass]
    public class NotificationTest : Support.IntegrationTest
    {
        public NotificationTest() : base(false)
        {
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Administration.SetSystemServerConfiguration(
                new SystemServerConfigurationViewModel
                    {
                        SystemConfigurationViewModel = new SystemConfigurationViewModel
                                                           {
                                                               MaxAnswers = 3,
                                                               QuestionLifetimeDays = QuestionLifetime.FiveMinutes,
                                                               SystemEnvironment = SystemEnvironment.Testing
                                                           },
                        ServerConfigurationViewModel = new ServerConfigurationViewModel
                                                           {
                                                               ServerRole = ServerRole.Public
                                                           }
                    });

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
        }

        [TestMethod]
        [ExpectedException(typeof(KnownIssuesException))]
        public void QuestionerNotificationOnQuestionApproved()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var notifications = Notifications.GetAll();
            Assert.AreEqual(0, notifications.Count());

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Approve(moderation.ModerationId);

            notifications = Notifications.GetAll();
            Assert.AreEqual(1, notifications.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(KnownIssuesException))]
        public void AnswererNotificationOnAnswerApproved()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");
            
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var notifications = Notifications.GetAll();
            Assert.AreEqual(0, notifications.Count());

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Approve(moderation.ModerationId);

            notifications = Notifications.GetAll();
            Assert.AreEqual(1, notifications.Count());
        }

        [TestMethod]
        public void QuestionerNotificationOnAnswerApproval()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var notifications = Notifications.GetAll();
            Assert.AreEqual(0, notifications.Count());

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            
            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Approve(moderation.ModerationId);

            SecurityManager.SetUserId("Questioner");

            notifications = Notifications.GetAll();
            var notification = notifications.Single();
            Assert.AreEqual(1, notification.Occurrences);
            throw new NotImplementedException();
            //Assert.AreEqual(NotificationType.QuestionAnswered, notification.NotificationType);
        }

        [TestMethod]
        public void QuestionerNotificationOnQuestionRejection()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var notifications = Notifications.GetAll();
            Assert.AreEqual(0, notifications.Count());

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            notifications = Notifications.GetAll();
            var notification = notifications.Single();
            Assert.AreEqual(1, notification.Occurrences);
            throw new NotImplementedException();
            //Assert.AreEqual(NotificationType.PostRejected, notification.NotificationType);
        }

        [TestMethod]
        public void AnswererNotificationOnAnswerRejection()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var notifications = Notifications.GetAll();
            Assert.AreEqual(0, notifications.Count());

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            notifications = Notifications.GetAll();
            var notification = notifications.Single();
            Assert.AreEqual(1, notification.Occurrences);
            throw new NotImplementedException();
            //Assert.AreEqual(NotificationType.PostRejected, notification.NotificationType);
        }

        [TestMethod]
        public void QuestionAnsweredNotificationsGroupAndMute()
        {
            Action<string, string> createAndApproveAnswer = (userReference, answerReference) =>
                {
                    var currentUserId = SecurityManager.GetUserId();
                    
                    SecurityManager.LoginNewAnonymous(userReference);
                    Answers.RequestAnswerSlot("Question", answerReference);
                    Answers.CompleteAnswer("Question", Answers.FillDefaults(answerReference));
                    ManualModeration.Approve(ManualModeration.GetNextForUser(userReference).ModerationId);
                    
                    SecurityManager.SetUserId(currentUserId);
                };

            Action<int, int> validateNotificationOccurrences = (expectedNew, expectedMuted) =>
                {
                    throw new NotImplementedException();
                    /*var notifications = Notifications.GetAll().Where(x => x.NotificationType == NotificationType.QuestionAnswered).ToList();

                    var newNotifications = notifications.Where(x => !x.IsActioned).ToList();
                    var mutedNotifications = notifications.Where(x => x.IsActioned).ToList();

                    if (expectedNew == 0)
                    {
                        Assert.AreEqual(0, newNotifications.Count());
                    }
                    else
                    {
                        Assert.AreEqual(expectedNew, newNotifications.Single().Occurrences);
                    }

                    if (expectedMuted == 0)
                    {
                        Assert.AreEqual(0, mutedNotifications.Count());
                    }
                    else
                    {
                        Assert.AreEqual(expectedMuted, mutedNotifications.Single().Occurrences);
                    }*/
                };

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            
            createAndApproveAnswer("First Answerer", "Answer One");
            createAndApproveAnswer("Second Answerer", "Answer Two");
            validateNotificationOccurrences(2, 0);

            Questions.GetQuestion("Question");
            validateNotificationOccurrences(0, 2);

            createAndApproveAnswer("Third Answerer", "Answer Three");
            validateNotificationOccurrences(1, 2);

            Questions.GetQuestion("Question");
            validateNotificationOccurrences(0, 3);
        }
    }
}