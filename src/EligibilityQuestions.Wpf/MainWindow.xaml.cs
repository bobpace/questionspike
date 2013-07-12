using System;
using System.Windows;

namespace EligibilityQuestions.Wpf
{
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
