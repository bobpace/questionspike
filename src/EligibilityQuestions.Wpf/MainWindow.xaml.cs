using System.Windows;

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var firstQuestion = Question.ForAnswer<TestModel>(x => x.LikesBlue);
            firstQuestion.QuestionText = "Do you like blue?";

            var secondQuestion = Question.ForAnswer<TestModel>(x => x.LikesGreen);
            secondQuestion.QuestionText = "Do you like green?";

            var thirdQuestion = Question.ForAnswer<TestModel>(x => x.LikesRed);
            thirdQuestion.QuestionText = "Do you like red?";

            firstQuestion.OnNo(x => secondQuestion);

            secondQuestion.OnYes(x => thirdQuestion);

            DataContext = firstQuestion;
        }
    }
}
