using System;
using System.Windows;

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ModelBuilder<TestModel> _modelBuilder;
        private readonly WindowContext _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            var annualEnrollmentQuestion = Question.ForAnswer<TestModel>(x => x.WithinAnnualEnrollmentPeriod);
            annualEnrollmentQuestion.QuestionText = "Are you within the annual enrollment period?";

            var drugCoverageQuestion = Question.ForAnswer<TestModel,PrescriptionDrugCoverage?>(x => x.CurrentlyEnrolledDrugCoverage);
            drugCoverageQuestion.QuestionText = "What type of medical or prescription drug coverage are you currently enrolled in?";

            var birthdayQuestion = Question.ForAnswer<TestModel>(x => x.Birthday);
            birthdayQuestion.QuestionText = "When is your birthday?";

            var greenQuestion = Question.ForAnswer<TestModel>(x => x.LikesGreen);
            greenQuestion.QuestionText = "Do you like green?";

            var yellowQuestion = Question.ForAnswer<TestModel>(x => x.LikesYellow);
            yellowQuestion.QuestionText = "Do you like yellow?";

            var purpleQuestion = Question.ForAnswer<TestModel>(x => x.LikesPurple);
            purpleQuestion.QuestionText = "Do you like purple?";

            //TODO: expand multiple select question to have an IEnumerable<Question> for next question
            drugCoverageQuestion.OnNext(x => birthdayQuestion);

            //birthdayQuestion.OnNext(x => greenQuestion);

            greenQuestion.OnYes(x => yellowQuestion);
            greenQuestion.OnNo(x => purpleQuestion);

            var questions = new Question[]
            {
                annualEnrollmentQuestion,
                drugCoverageQuestion
            };

            _dataContext = new WindowContext
            {
                Questions = questions
            };
            _modelBuilder = new ModelBuilder<TestModel>(questions);

            DataContext = _dataContext;
        }

        private void BuildModel(object sender, RoutedEventArgs e)
        {
            var model = _modelBuilder.BuildModel();
            _dataContext.AnswerSummary = model.ToString();
        }
    }
}
