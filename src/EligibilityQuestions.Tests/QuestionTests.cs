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

            var now = DateTime.Now;

            var third = new DateTimeQuestion<EndResultModel>(x => x.BirthDay)
            {
                Answer = now.AddYears(-120)
            };

            var fourth = new DateTimeQuestion<EndResultModel>(x => x.DeathDay)
            {
                Answer = now
            };

            var questions = new Question<EndResultModel>[]
            {
                first, second, third, fourth
            };

            var result = new EndResultModel();
            questions.Each(x => x.SetAnswer(result));

            result.LikesBlue.ShouldBeTrue();
            result.LikesGreen.Value.ShouldBeTrue();
            result.BirthDay.ShouldEqual(now.AddYears(-120));
            result.DeathDay.Value.ShouldEqual(now);
        }

        public class EndResultModel
        {
            public bool LikesBlue { get; set; }
            public bool? LikesGreen { get; set; }
            public DateTime? DeathDay { get; set; }
            public DateTime BirthDay { get; set; }
        }
    }

}