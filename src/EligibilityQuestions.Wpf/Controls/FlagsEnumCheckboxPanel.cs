using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using EligibilityQuestions.Wpf.Converters;

namespace EligibilityQuestions.Wpf.Controls
{
    public class FlagsEnumCheckBoxPanel : StackPanel
    {
        public static readonly DependencyProperty FlagsEnumTypeProperty =
            DependencyProperty.Register("FlagsEnumType", typeof(Type), typeof(FlagsEnumCheckBoxPanel));

        public static readonly DependencyProperty RawValueProperty =
            DependencyProperty.Register("RawValue", typeof(object), typeof(FlagsEnumCheckBoxPanel),
                                        new FrameworkPropertyMetadata { BindsTwoWayByDefault = true});

        public static readonly DependencyProperty CheckBoxMarginProperty =
            DependencyProperty.Register("CheckBoxMargin", typeof (Thickness), typeof (FlagsEnumCheckBoxPanel),
                                        new UIPropertyMetadata(new Thickness(0, 10, 10, 0)));

        public FlagsEnumCheckBoxPanel()
        {
            DisplayFormatter = new DefaultDisplayFormatter();
        }

        public Thickness CheckBoxMargin
        {
            get { return (Thickness)GetValue(CheckBoxMarginProperty); }
            set { SetValue(CheckBoxMarginProperty, value); }
        }

        public Type FlagsEnumType
        {
            get { return (Type)GetValue(FlagsEnumTypeProperty); }
            set { SetValue(FlagsEnumTypeProperty, value); }
        }

        public object RawValue
        {
            get { return GetValue(RawValueProperty); }
            set { SetValue(RawValueProperty, value); }
        }

        public IFlagsEnumDisplayFormatter DisplayFormatter { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            if (FlagsEnumType != null)
            {
                FlagsEnumType.ValidateFlagsEnumType();
                var provider = DataContext as IFlagsEnumFormatterProvider;
                if (provider != null)
                {
                    DisplayFormatter = provider.DisplayFormatter;
                }

                MakeCheckBoxes().Each(x => Children.Add(x));
            }
            base.OnInitialized(e);
        }

        private IEnumerable<CheckBox> MakeCheckBoxes()
        {
            var converter = new FlagsEnumValueConverter(FlagsEnumType);
            return Enum.GetValues(FlagsEnumType)
                .Cast<object>()
                .Select(x => MakeCheckBox(x, converter));
        }

        private CheckBox MakeCheckBox(object value, IValueConverter converter)
        {
            var checkBox = new CheckBox
            {
                Content = DisplayFormatter.FormatValue(value),
                Margin = CheckBoxMargin
            };

            var binding = new Binding
            {
                Path = new PropertyPath("RawValue"),
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor)
                {
                    AncestorType = GetType()
                },
                Converter = converter,
                ConverterParameter = value,
            };

            checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding);
            return checkBox;
        }
    }
}