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

        public MainWindow()
        {
            InitializeComponent();
            UseScenario(new NewQuestionScenario());
            //UseScenario(new ExistingQuestionScenario());
        }

        private void UseScenario(IQuestionScenario scenario)
        {
            _scenario = scenario;
            DataContext = _scenario;
        }

        private void BuildModel(object sender, RoutedEventArgs e)
        {
            _scenario.GetAnswerSummary();
        }
    }
}
