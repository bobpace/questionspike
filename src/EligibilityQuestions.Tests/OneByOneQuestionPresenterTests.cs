using System;
using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace EligibilityQuestions.Tests
{
    public class OneByOneQuestionPresenterTests : InteractionContext<OneByOneQuestionPresenter>
    {
        protected override void beforeEach()
        {
        }

        [Test]
        public void questions_can_target_properties_on_result_model()
        {
            var first = new Question<EndResultModel>(x => x.LikesBlue);
            first.Answer = true;
            var second = new Question<EndResultModel>(x => x.LikesGreen);
            second.Answer = true;
            var third = new Question<EndResultModel>(x => x.Birthday);
            var today = DateTime.Today;
            third.Answer = today;

            var questions = new[] {first, second, third};
            var result = new EndResultModel();
            questions.Each(x => x.SetAnswer(result));

            result.LikesBlue.ShouldBeTrue();
            result.LikesGreen.Value.ShouldBeTrue();
            result.Birthday.ShouldEqual(today);
        }

        [Test]
        public void may_have_more_than_one_nested_question()
        {
        }

        [Test]
        public void future_questions_are_hidden_until_after_you_answer_the_current_one()
        {
        }

        [Test]
        public void previously_answered_questions_are_shown()
        {
        }

        [Test]
        public void you_can_answer_the_current_question_and_get_the_next_one()
        {
        }

        [Test]
        public void eventually_you_run_out_of_questions_and_trigger_a_calculated_result()
        {
        }

    }
}