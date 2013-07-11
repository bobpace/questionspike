using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EligibilityQuestions.Wpf.Controls
{
    public class MaskedTextBox : TextBox
	{
		public static readonly DependencyProperty DataTypeIsNullableProperty =
			DependencyProperty.Register("DataTypeIsNullable", typeof(bool), typeof(MaskedTextBox));

		public static readonly DependencyProperty DataTypeProperty =
			DependencyProperty.Register("DataType", typeof(Type), typeof(MaskedTextBox));

		public static readonly DependencyProperty DefaultValueProperty =
			DependencyProperty.Register("DefaultValue", typeof(object), typeof(MaskedTextBox), new UIPropertyMetadata(null));

		public static readonly DependencyProperty HasInvalidValueProperty =
			DependencyProperty.Register("HasInvalidValue", typeof(bool), typeof(MaskedTextBox));

		public static readonly DependencyProperty IncludeLiteralsProperty =
			DependencyProperty.Register("IncludeLiterals", typeof(bool), typeof(MaskedTextBox),
			                            new PropertyMetadata(true));

		/// <summary>
		/// Dependency property to store the mask to apply to the textbox
		/// </summary>
		public static readonly DependencyProperty MaskProperty =
			DependencyProperty.Register("Mask", typeof(string), typeof(MaskedTextBox),
			                            new UIPropertyMetadata(null, MaskChanged));

        public static readonly DependencyProperty ForceMaskLengthToMatchValueLengthProperty =
            DependencyProperty.Register("ForceMaskLength", typeof(bool), typeof(MaskedTextBox));

		public static readonly DependencyProperty PreviousTextBoxProperty =
			DependencyProperty.Register("PreviousTextBox", typeof(string), typeof(MaskedTextBox));

		public static readonly DependencyProperty StringFormatProperty =
			DependencyProperty.Register("StringFormat", typeof(string), typeof(MaskedTextBox),
			                            new PropertyMetadata(StringFormatChanged));

		public static readonly DependencyProperty TextBoxToHighlightWhenCompleteProperty =
			DependencyProperty.Register("TextBoxToHighlightWhenComplete", typeof(string), typeof(MaskedTextBox));

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(object), typeof(MaskedTextBox),
			                            new FrameworkPropertyMetadata(ValueChanged) {BindsTwoWayByDefault = true});


	    /// <summary>
		/// Static Constructor
		/// </summary>
		static MaskedTextBox()
		{
			//override the meta data for the Text Proeprty of the textbox 
			var metaData = new FrameworkPropertyMetadata {CoerceValueCallback = ForceText};
			TextProperty.OverrideMetadata(typeof(MaskedTextBox), metaData);
		}

		///<summary>
		/// Default  constructor
		///</summary>
		public MaskedTextBox()
		{
			//cancel the paste and cut command
            DataObject.AddPastingHandler(this, OnPaste);
			CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, null, CancelCommand));
		}

	    public string PreviousTextBox
		{
			get { return (string)GetValue(PreviousTextBoxProperty); }
			set { SetValue(PreviousTextBoxProperty, value); }
		}

		public string TextBoxToHighlightWhenComplete
		{
			get { return (string)GetValue(TextBoxToHighlightWhenCompleteProperty); }
			set { SetValue(TextBoxToHighlightWhenCompleteProperty, value); }
		}

		public object DefaultValue
		{
			get { return GetValue(DefaultValueProperty); }
			set { SetValue(DefaultValueProperty, value); }
		}

		public bool HasInvalidValue
		{
			get { return (bool)GetValue(HasInvalidValueProperty); }
			set { SetValue(HasInvalidValueProperty, value); }
		}

		public bool IsUpdatingValue { get; private set; }

		public bool DataTypeIsNullable
		{
			get { return (bool)GetValue(DataTypeIsNullableProperty); }
			set { SetValue(DataTypeIsNullableProperty, value); }
		}

		public string StringFormat
		{
			get { return (string)GetValue(StringFormatProperty); }
			set { SetValue(StringFormatProperty, value); }
		}

		public Type DataType
		{
			get { return (Type)GetValue(DataTypeProperty); }
			set { SetValue(DataTypeProperty, value); }
		}

		public object Value
		{
			get { return GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public bool IncludeLiterals
		{
			get { return (bool)GetValue(IncludeLiteralsProperty); }
			set { SetValue(IncludeLiteralsProperty, value); }
		}

        public bool PastedText { get; set; }

		/// <summary>
		/// Gets the MaskTextProvider for the specified Mask
		/// </summary>
		public MaskedTextProvider MaskProvider
		{
			get
			{
				MaskedTextProvider maskProvider = null;
				if (Mask != null)
				{
					maskProvider = new MaskedTextProvider(Mask) {IncludeLiterals = IncludeLiterals};
					maskProvider.Set(Text ?? string.Empty);
				}
				return maskProvider;
			}
		}

		/// <summary>
		/// Gets or sets the mask to apply to the textbox
		/// </summary>
		public string Mask
		{
			get { return (string)GetValue(MaskProperty); }
			set { SetValue(MaskProperty, value); }
		}

	    public bool ForceMaskLengthToMatchValueLength
	    {
            get { return (bool) GetValue(ForceMaskLengthToMatchValueLengthProperty); }
            set { SetValue(ForceMaskLengthToMatchValueLengthProperty, value); }
	    }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (MaskProvider.AssignedEditPositionCount == 0)
            {
                Select(0, 0);
            }
        }

        void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var pastedText = e.DataObject.GetData(e.FormatToApply).ToString();
            pastedText = pastedText.Replace("-", "");
            SetPastedText(pastedText);
            e.Handled = true;
            e.CancelCommand();
            PastedText = true;
        }

        void SetPastedText(string pastedText)
        {
            var provider = MaskProvider;
            if (pastedText.Length > Text.Length)
            {
                var partialText = pastedText.Substring(0, Text.Length);
                provider.Set(partialText);
                Text = provider.ToString();
                var remainingText = pastedText.Substring(Text.Length, pastedText.Length - Text.Length);
                var nextTextBox = FindControl<MaskedTextBox>(TextBoxToHighlightWhenComplete);
                if (nextTextBox != null)
                {
                    nextTextBox.SetPastedText(remainingText);
                }
            }
            else
            {
                provider.Set(pastedText);
                Text = provider.ToString();
            }
        }

		protected override void OnDrop(DragEventArgs e)
		{
			var text = (string)e.Data.GetData(typeof(String));
			var provider = MaskProvider;
			if (provider != null)
			{
				provider.Set(text);
				Text = provider.ToString();
			}
			e.Handled = true;
			base.OnDrop(e);
		}

		private static void StringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (MaskedTextBox)d;
			textBox.RefreshText();
		}

		private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBox = (MaskedTextBox)d;
			if (textBox.IsUpdatingValue) return;
			if (textBox.DataType == null && textBox.Value != null) textBox.DataType = textBox.Value.GetType();
			textBox.RefreshText();
		}

		//callback for when the Mask property is changed
		private static void MaskChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			//make sure to update the text if the mask changes
			var textBox = (MaskedTextBox)sender;
            if (textBox.MaskProvider != null)
            {
                textBox.RefreshText(textBox.MaskProvider, 0);
            }
		}

		//force the text of the control to use the mask
		private static object ForceText(DependencyObject sender, object value)
		{
			var textBox = (MaskedTextBox)sender;
            if (textBox.PastedText)
            {
                textBox.PastedText = false;
                return textBox.Text;
            }
			return textBox.ForceText(value != null ? (string)value : string.Empty);
		}

		private object ForceText(string value)
		{
			var result = value;
			string textWithoutLiterals = null;
			if (Mask != null)
			{
				if (value.Length > Mask.Length)
				{
					value = value.Substring(0, Mask.Length);
				}
				var provider = new MaskedTextProvider(Mask) {IncludeLiterals = false};
				provider.Set(value);
				result = provider.ToDisplayString();
				textWithoutLiterals = provider.ToString();
			}
			RefreshValue(value, textWithoutLiterals);
			return result;
		}

		private void RefreshValue(string text, string textWithoutLiterals)
		{
			IsUpdatingValue = true;
			if (DataType != null)
			{
				try
				{
					object newValue = null;
					if (DataType == typeof(string))
					{
						newValue = text;
					}
                    else if (string.IsNullOrEmpty(textWithoutLiterals))
                    {
                        HasInvalidValue = false;
                        if (DataType.IsValueType && !DataTypeIsNullable && DefaultValue != null)
                        {
                            newValue = Activator.CreateInstance(DataType);
                        }
                    }
                    else
                    {
                        var tryParseMethod = DataType.GetMethod("TryParse",
                                                                new[] {typeof (string), DataType.MakeByRefType()});
                        if (tryParseMethod != null)
                        {
                            var outValue = Activator.CreateInstance(DataType);
                            var parameters = new[] {text, outValue};
                            if ((!ForceMaskLengthToMatchValueLength || string.IsNullOrEmpty(text) || string.IsNullOrEmpty(Mask) ||
                                ForceMaskLengthToMatchValueLength && text.Length == Mask.Length) &&
                                    (bool) tryParseMethod.Invoke(null, parameters))
                            {
                                newValue = parameters[1];
                                HasInvalidValue = false;
                            }
                            else
                            {
                                newValue = DataType.IsValueType && !DataTypeIsNullable
                                    ? Activator.CreateInstance(DataType)
                                    : null;
                                HasInvalidValue = true;
                            }
                        }
                    }
				    if (!Equals(newValue, Value))
					{
						Value = newValue;
						if (!String.IsNullOrEmpty(TextBoxToHighlightWhenComplete) && Value is string &&
						    ((string)Value).Length == Mask.Length)
						{
						    var control = FindControl<FrameworkElement>(TextBoxToHighlightWhenComplete);
							if (control != null)
								control.Focus();
						}
					}
				}
				catch (Exception ex)
				{
				}
			}
			IsUpdatingValue = false;
		}

		//cancel the command
		private static void CancelCommand(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
			e.Handled = true;
		}

		protected override void OnGotFocus(RoutedEventArgs e)
		{
			base.OnGotFocus(e);
			SelectAll();
		}

		/// <summary>
		/// override this method to replace the characters entered with the mask
		/// </summary>
		/// <param name="e">Arguments for event</param>
		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			//if the text is readonly do not add the text
			if (IsReadOnly)
			{
				e.Handled = true;
				return;
			}

			var position = SelectionStart;
			var provider = MaskProvider;
			if (SelectionLength == Text.Length)
			{
				int testPosition;
				MaskedTextResultHint resultHint;
				if (provider.Set(e.Text, out testPosition, out resultHint))
				{
					position++;
				}
			}
			else if (position < Text.Length)
			{
				position = GetNextCharacterPosition(position);

				if (Keyboard.IsKeyToggled(Key.Insert))
				{
					if (provider.Replace(e.Text, position))
						position++;
				}
				else
				{
					if (provider.InsertAt(e.Text, position))
						position++;
				}

				position = GetNextCharacterPosition(position);
			}

			RefreshText(provider, position);
			e.Handled = true;

			base.OnPreviewTextInput(e);
		}

		protected override void OnPreviewKeyUp(KeyEventArgs e)
		{
			base.OnPreviewKeyUp(e);
			var position = SelectionStart;
			if (e.Key != Key.Back || position != 0 || String.IsNullOrEmpty(PreviousTextBox)) 
				return;
		    var control = FindControl<MaskedTextBox>(PreviousTextBox);
			if (control != null)
			{
				control.Focus();
				control.Select(control.Text.Length, 0);
			}
		}

		/// <summary>
		/// override the key down to handle delete of a character
		/// </summary>
		/// <param name="e">Arguments for the event</param>
		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			var provider = MaskProvider;
			var position = SelectionStart;
			var selectionlength = SelectionLength;
			// If no selection use the start position else use end position
			var endposition = (selectionlength == 0) ? position : position + selectionlength - 1;

			if (e.Key == Key.Delete && position < Text.Length) //handle the delete key
			{
				if (provider.RemoveAt(position, endposition))
					RefreshText(provider, position);

				e.Handled = true;
			}

			else
				switch (e.Key)
				{
					case Key.Space:
						if (provider.InsertAt(" ", position))
							RefreshText(provider, position);
						e.Handled = true;
						break;
					case Key.Back:
						if ((position > 0) && (selectionlength == 0))
						{
							position--;
							if (provider.RemoveAt(position))
								RefreshText(provider, position);
						}
						if (selectionlength != 0)
						{
							if (provider.RemoveAt(position, endposition))
							{
								if (position > 0)
									position--;

								RefreshText(provider, position);
							}
						}
						e.Handled = true;
						break;
				}
		}

		//refreshes the text of the textbox
		private void RefreshText(MaskedTextProvider provider, int position)
		{
			Text = provider.ToString();
			SelectionStart = position;
		}

		private void FallbackToDefault()
		{
			Value = DefaultValue;
		}

		private void RefreshText()
		{
			if (StringFormat != null && Value != null && DataType != null)
			{
				try
				{
					var toStringMethod = DataType.GetMethod("ToString", new[] {typeof(string)});
					if (toStringMethod != null) Text = (string)toStringMethod.Invoke(Value, new[] {StringFormat});
					return;
				}
				catch (Exception ex)
				{
				}
			}
			Text = Value != null ? Value.ToString() : string.Empty;
		}

		//gets the next position in the textbox to move
		private int GetNextCharacterPosition(int startPosition)
		{
			var position = MaskProvider.FindEditPositionFrom(startPosition, true);
			return position == -1 ? startPosition : position;
		}

        private T FindControl<T>(string name) where T : class
        {
            if (string.IsNullOrEmpty(name)) return null;
            return ((FrameworkElement)Parent).FindName(name) as T;
        }
	}
}