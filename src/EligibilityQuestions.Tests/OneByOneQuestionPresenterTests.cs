using System;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace EligibilityQuestions.Tests
{
    public class TestModel
    {
    }

    public class OneByOneQuestionPresenterTests : InteractionContext<OneByOneQuestionPresenter<TestModel>>
    {
        protected override void beforeEach()
        {
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