using System;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;

namespace SDK_Usage_SampleApp.Behaviours
{
	public sealed class AvalonEditBehaviour : Behavior<TextEditor>
	{
		public static readonly DependencyProperty TextContentProperty =
			DependencyProperty.Register("TextContent", typeof(string), typeof(AvalonEditBehaviour),
			new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

		public string TextContent
		{
			get { return (string)GetValue(TextContentProperty); }
			set { SetValue(TextContentProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			if (AssociatedObject != null)
				AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (AssociatedObject != null)
				AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
		}

		private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
		{
			var textEditor = sender as TextEditor;
			if (textEditor != null)
			{
				if (textEditor.Document != null)
					TextContent = textEditor.Document.Text;
			}
		}

		private static void PropertyChangedCallback(
			DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var behavior = dependencyObject as AvalonEditBehaviour;
			if (behavior.AssociatedObject != null)
			{
				var editor = behavior.AssociatedObject as TextEditor;
				if (editor.Document != null)
				{
					editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
				}
			}
		}
	}
}