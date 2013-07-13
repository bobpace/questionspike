using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using EligibilityQuestions.Wpf.Converters;

namespace EligibilityQuestions.Wpf.Controls
{
    public class FlagsEnumCheckboxPanel : StackPanel
    {
        public static readonly DependencyProperty FlagsEnumTypeProperty =
            DependencyProperty.Register("FlagsEnumType", typeof(Type), typeof(FlagsEnumCheckboxPanel));

        public static readonly DependencyProperty RawValueProperty =
            DependencyProperty.Register("RawValue", typeof(object), typeof(FlagsEnumCheckboxPanel),
                                        new FrameworkPropertyMetadata { BindsTwoWayByDefault = true});

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

        protected override void OnInitialized(EventArgs e)
        {
            if (FlagsEnumType != null)
            {
                var provider = DataContext as IFlagsEnumFormatterProvider;
                var formatter = provider != null
                    ? provider.DisplayFormatter
                    : new DefaultDisplayFormatter();
                FlagsEnumType.ValidateFlagsEnumType();
                var converter = new FlagsEnumValueConverter(FlagsEnumType);
                foreach (var value in Enum.GetValues(FlagsEnumType))
                {
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

                    var checkBox = new CheckBox
                    {
                        Content = formatter.FormatValue(value),
                        Margin = new Thickness(0, 10, 10, 0)
                    };
                    checkBox.SetBinding(ToggleButton.IsCheckedProperty, binding);
                    Children.Add(checkBox);
                }
            }
            base.OnInitialized(e);
        }
    }
}