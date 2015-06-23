using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Dilemma.Common
{
    public enum PointType
    {
        [Display(Name = "Question Asked", Description = "Points are awarded for asking a question and contributing to the community. Points are awarded once the question passes moderation.")]
        [DefaultValue(100)]
        QuestionAsked = 1,

        [Display(Name = "Answer Provided", Description = "Points are awarded for answering a question and contributing to the community. Points are awarded once the answer passes moderation.")]
        [DefaultValue(100)]
        QuestionAnswered = 2,

        [Display(Name = "Star Vote Received", Description = "Points are awarded for receiving the star vote on a question and contributing to the community. Points are awarded immediately.")]
        [DefaultValue(100)]
        StarVoteReceived = 3,

        [Display(Name = "Popular Vote Received", Description = "Points are awarded for receiving the star vote on a question and contributing to the community. Points are awarded immediately.")]
        [DefaultValue(100)]
        PopularVoteReceived = 4,

        [Display(Name = "Vote Cast", Description = "Points are awarded for casting a vote for someone else's answer. Points are awarded immediately.")]
        [DefaultValue(100)]
        VoteCast = 5,

        [Display(Name = "Star Vote Awarded", Description = "Points are awarded for awarding the star vote on your dilemma. Points are awarded immediately.")]
        [DefaultValue(100)]
        StarVoteAwarded = 6,

        [Display(Name = "What Happened Next", Description = "Points are awarded for telling the people that answered, voted, and bookmarked what course of action you took. Points are awarded immediately.")]
        [DefaultValue(100)]
        WhatHappenedNext = 7,
    }
}
