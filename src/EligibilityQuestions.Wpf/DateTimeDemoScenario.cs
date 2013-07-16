using System;
using System.Collections.Generic;
using System.Linq;

namespace EligibilityQuestions.Wpf
{
    public class DateTimeDemoScenario : QuestionScenario<DateTimeDemoModel>
    {
        public DateTimeDemoScenario()
        {
            var birthdayQuestion = Question.ForAnswer<DateTimeDemoModel>(x => x.BirthDay);
            birthdayQuestion.QuestionText = "When is your birthday?";

            var desiredImmEffectiveDateQuestion = Question.ForAnswer<DateTimeDemoModel>(x => x.DesiredImmEffectiveDate);
            desiredImmEffectiveDateQuestion.QuestionText = "When is your desired Imm Effective Date?";

            var desiredMedicareEffectiveDateQuestion = Question.ForAnswer<DateTimeDemoModel>(x => x.DesiredMedicareEffectiveDate);
            desiredMedicareEffectiveDateQuestion.QuestionText = "When is your desired Medicare Effective Date?";

            birthdayQuestion.OnNext(x =>
            {
                if (x.Answer == null) return null;

                var date = (DateTime) x.Answer;
                return DateTimeToAge(date) < 65
                    ? desiredImmEffectiveDateQuestion
                    : desiredMedicareEffectiveDateQuestion;
            });

            var questions = new Question[]
            {
                birthdayQuestion
            };

            Questions = questions;
            AllQuestions = questions.Concat(new[]{desiredImmEffectiveDateQuestion, desiredMedicareEffectiveDateQuestion});
        }

        public override IEnumerable<object> ScenarioModels
        {
            get
            {
                yield return new DateTimeDemoModel
                {
                    BirthDay = new DateTime(1940, 1, 1),
                    DesiredMedicareEffectiveDate = new DateTime(2013, 10, 1)
                };

                yield return new DateTimeDemoModel
                {
                    BirthDay = new DateTime(1981, 1, 1),
                    DesiredImmEffectiveDate = new DateTime(2013, 10, 1)
                };
            }
        }

        private int DateTimeToAge(DateTime date)
        {
            var now = DateTime.Now;
            var years = now.Year - date.Year;
            if (now < date.AddYears(years)) years--;
            return years;
        }
    }

    public class DateTimeDemoModel
    {
        public DateTime? BirthDay { get; set; }
        public DateTime? DesiredImmEffectiveDate { get; set; }
        public DateTime? DesiredMedicareEffectiveDate { get; set; }
    }
}