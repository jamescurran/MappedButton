using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace NovelTheory.Xamarin.Controls
{
	public class MappedButton : StackLayout 
	{

		#region Bindable Properties (Options, SelectedKey, SelectedColor, ButtonColor 
		public static readonly BindableProperty OptionsProperty
			= BindableProperty.Create(nameof(Options), typeof(string), typeof(MappedButton), default(string), defaultBindingMode: BindingMode.TwoWay);

		public string Options
		{
			get => (string) GetValue(OptionsProperty);
			set => SetValue(OptionsProperty, value);
		}

		public static readonly BindableProperty SelectedKeyProperty
			= BindableProperty.Create(nameof(SelectedKey), typeof(string), typeof(MappedButton), default(string),defaultBindingMode: BindingMode.TwoWay);

		public string SelectedKey
		{
			get => (string)GetValue(SelectedKeyProperty);
			set => SetValue(SelectedKeyProperty, value);
		}

		public static readonly BindableProperty SelectedColorProperty
			= BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(MappedButton), Color.Chartreuse, defaultBindingMode: BindingMode.OneTime);

		public Color SelectedColor 
		{
			get => (Color)GetValue(SelectedColorProperty);
			set => SetValue(SelectedColorProperty, value);
		}

		public static readonly BindableProperty ButtonColorProperty
			= BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(MappedButton), Color.Bisque, defaultBindingMode: BindingMode.OneTime);

		public Color ButtonColor
		{
			get => (Color)GetValue(ButtonColorProperty);
			set => SetValue(ButtonColorProperty, value);
		}
		#endregion

		public MappedButton ()
		{
			this.Orientation = StackOrientation.Horizontal;
			this.HorizontalOptions = LayoutOptions.CenterAndExpand;
			this.WidthRequest = 1000.0;
		}

		public ICommand ShowTappedCommand => new Command<String>(OnButtonTapped);

		private void OnButtonTapped(string key)
		{
			SelectedKey = key;
		}

		private void DeselectAllButtons()
		{
			foreach (var btn in _buttonByCode.Values)
				btn.BackgroundColor = this.ButtonColor;
		}

		private Dictionary<string, string> _labelsByCode;
		private Dictionary<string, Button> _buttonByCode;

		protected override void OnPropertyChanged(string propertyName = null)
		{
			switch (propertyName)
			{
				case "Options":
					_labelsByCode = Options.Split(',').Select(item => item.Split('='))
						.ToDictionary(pair => pair.First().Trim(), pair => pair.Last().Trim());

					var buttons = _labelsByCode
						.Select(kvp => new Button
						{
							Text = kvp.Value,
							CommandParameter = kvp.Key,
							BackgroundColor = Color.Bisque,
							BorderColor = Color.Black,
							BorderWidth = 1,
							Command = ShowTappedCommand
						}).ToList();

					_buttonByCode = buttons.ToDictionary(btn => btn.CommandParameter as string);

					foreach (var button in buttons)
					{
						Children.Add(button);
					}
					break;

				case nameof(SelectedKey):
					DeselectAllButtons();
					SelectButtonByKey(SelectedKey);
					break;
			}

			base.OnPropertyChanged(propertyName);
		}

		private void SelectButtonByKey(string key)
		{
			if (key == null)
				return;

			if (_buttonByCode.TryGetValue(key, out Button btn))
			{
				btn.BackgroundColor = this.SelectedColor;
			}
		}
	}
}