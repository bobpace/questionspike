using System;
using System.Collections.Generic;
using FubuTestingSupport;
using NUnit.Framework;

namespace EligibilityQuestions.Tests
{
    public class QuestionTests
    {
        [Test]
        public void questions_can_target_properties_on_result_model()
        {
            var first = new YesNoQuestion<EndResultModel>(x => x.LikesBlue)
            {
                Answer = true
            };
            var second = new YesNoQuestion<EndResultModel>(x => x.LikesGreen)
            {
                Answer = true
            };

            var questions = new[] {first, second};

            var result = new EndResultModel();
            questions.Each(x => x.SetAnswer(result));

            result.LikesBlue.ShouldBeTrue();
            result.LikesGreen.Value.ShouldBeTrue();
        }

        [Test]
        public void questions_can_set_null_on_answer()
        {
        }

        public class EndResultModel
        {
            public bool LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
        }
    }

}