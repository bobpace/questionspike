using System;
using System.Collections.Generic;
using System.Windows;

namespace EligibilityQuestions.Wpf
{
    public interface IQuestionScenario
    {
        string GetAnswerSummary();
        IEnumerable<Question> Questions { get; }
    }

    public abstract class QuestionScenario<TModel> : IQuestionScenario where TModel : class, new()
    {
        public IEnumerable<Question> Questions { get; set; }

        public TModel BuildModel()
        {
            return new ModelBuilder<TModel>(Questions).BuildModel();
        }

        public string GetAnswerSummary()
        {
            return BuildModel().ToString();
        }
    }

    public class ExistingQuestionScenario : QuestionScenario<ExistingModel>
    {
    }

    public class NewQuestionScenario : QuestionScenario<NewModel>
    {
        public NewQuestionScenario()
        {
            var annualEnrollmentQuestion = Question.ForAnswer<NewModel>(x => x.WithinAnnualEnrollmentPeriod);
            annualEnrollmentQuestion.QuestionText = "Are you within the annual enrollment period?";

            var drugCoverageQuestion =
                Question.ForAnswer<NewModel, PrescriptionDrugCoverage?>(x => x.CurrentlyEnrolledDrugCoverage);
            drugCoverageQuestion.QuestionText =
                "What type of medical or prescription drug coverage are you currently enrolled in?";

            var enrolledInMedigapQuestion = Question.ForAnswer<NewModel>(x => x.EnrolledInMedigapDate);
            enrolledInMedigapQuestion.QuestionText = "When did you enroll in medigap?";
            var enrolledInPdpQuestion = Question.ForAnswer<NewModel>(x => x.EnrolledInPdpDate);
            enrolledInPdpQuestion.QuestionText = "When did you enroll in pdp?";

            var questionMap = new Dictionary<PrescriptionDrugCoverage, Question>
            {
                {PrescriptionDrugCoverage.Medigap, enrolledInMedigapQuestion},
                {PrescriptionDrugCoverage.PDP, enrolledInPdpQuestion}
            };

            annualEnrollmentQuestion.OnYes(x => drugCoverageQuestion);

            drugCoverageQuestion.SetExtraQuestions(questionMap);

            var birthdayQuestion = Question.ForAnswer<NewModel>(x => x.Birthday);
            birthdayQuestion.QuestionText = "When is your birthday?";

            var greenQuestion = Question.ForAnswer<NewModel>(x => x.LikesGreen);
            greenQuestion.QuestionText = "Do you like green?";

            var yellowQuestion = Question.ForAnswer<NewModel>(x => x.LikesYellow);
            yellowQuestion.QuestionText = "Do you like yellow?";

            var purpleQuestion = Question.ForAnswer<NewModel>(x => x.LikesPurple);
            purpleQuestion.QuestionText = "Do you like purple?";

            //TODO: expand multiple select question to have an IEnumerable<Question> for next question
            drugCoverageQuestion.OnNext(x => birthdayQuestion);

            birthdayQuestion.OnNext(x => greenQuestion);

            greenQuestion.OnYes(x => yellowQuestion);
            greenQuestion.OnNo(x => purpleQuestion);

            var questions = new Question[]
            {
                annualEnrollmentQuestion
            };

            Questions = questions;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IQuestionScenario _scenario;
        private WindowContext _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            var scenario = new NewQuestionScenario();
            UseScenario(scenario);
        }

        private void UseScenario(IQuestionScenario scenario)
        {
            _scenario = scenario;
            _dataContext = new WindowContext
            {
                Questions = scenario.Questions
            };
            DataContext = _dataContext;
        }

        private void BuildModel(object sender, RoutedEventArgs e)
        {
            _dataContext.AnswerSummary = _scenario.GetAnswerSummary();
        }
    }
}
