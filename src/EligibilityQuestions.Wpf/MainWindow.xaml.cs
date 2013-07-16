using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EligibilityQuestions.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IScenarioSwitcher _scenarioSwitcher;

        public MainWindow()
        {
            InitializeComponent();
            _scenarioSwitcher = new ScenarioSwitcher(new IQuestionScenario[]
            {
                new YesNoDemoScenario(),
                new DateTimeDemoScenario(),
                new MultipleSelectDemoScenario(),
                new ExistingQuestionScenario(),
                new NewQuestionScenario(),
            });
            DataContext = _scenarioSwitcher;
        }

        private void GetAnswerSummary(object sender, RoutedEventArgs e)
        {
            _scenarioSwitcher.CurrentScenario.GetAnswerSummary();
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            _scenarioSwitcher.WithoutBindings(x => x.Reset());
        }

        private void OnScenarioChanged(object sender, SelectionChangedEventArgs e)
        {
            var scenario = e.AddedItems
                .Cast<object>()
                .FirstOrDefault() as IQuestionScenario;
            if (scenario != null)
            {
                _scenarioSwitcher.CurrentScenario = scenario;
            }
        }

        private void OnScenarioModelsChanged(object sender, SelectionChangedEventArgs e)
        {
            var model = e.AddedItems
                .Cast<object>()
                .FirstOrDefault();
            if (model != null)
            {
                _scenarioSwitcher.WithoutBindings(x => x.SetAnswersFromModel(model));
            }
        }
    }

    public class ScenarioSwitcher : NotifyPropertyChangedBase, IScenarioSwitcher
    {
        private IQuestionScenario _currentScenario;
        private readonly IEnumerable<IQuestionScenario> _scenarios;

        public ScenarioSwitcher(IEnumerable<IQuestionScenario> scenarios)
        {
            _scenarios = scenarios;
            CurrentScenario = _scenarios.FirstOrDefault();
        }

        public IQuestionScenario CurrentScenario
        {
            get { return _currentScenario; }
            set
            {
                _currentScenario = value;
                NotifyOfPropertyChange(() => CurrentScenario);
            }
        }

        public void WithoutBindings(Action<IQuestionScenario> action)
        {
            if (CurrentScenario == null) return;

            var backup = CurrentScenario;
            CurrentScenario = null; //disable wpf bindings momentarily
            action(backup);
            CurrentScenario = backup;
        }

        public IEnumerable<IQuestionScenario> Scenarios
        {
            get { return _scenarios; }
        }
    }

    public interface IScenarioSwitcher
    {
        IQuestionScenario CurrentScenario { get; set; }
        IEnumerable<IQuestionScenario> Scenarios { get; }
        void WithoutBindings(Action<IQuestionScenario> action);
    }
}
