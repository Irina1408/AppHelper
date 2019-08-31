using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppView.Help
{
    using AppCommon.ValidationRules;
    using AppStyle.Controls;
    using AppUtils.Settings;

    public static class AddToGrid
    {
        #region TextBox

        public static TextBox TextBox(Grid grid, BindPropertyInfo bindPropertyInfo)
        {
            return TextBox(grid, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, bindPropertyInfo.Name);
        }

        public static TextBox TextBox(Grid grid, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules, string propertyPath)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add textbox
            var txt = new TextBox()
            {
                MaxWidth = 250,
                TextWrapping = TextWrapping.WrapWithOverflow,
                IsReadOnly = isReadOnly
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                ValidatesOnExceptions = true
            };
            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }
            // set binding
            BindingOperations.SetBinding(txt, System.Windows.Controls.TextBox.TextProperty, bind);
            //foreach (var validationRule in bind.ValidationRules)
            //{
            //    validationRule.ValidatesOnTargetUpdated = true;
            //}
            // add textbox to the details grid
            Grid.SetColumn(txt, 1);
            Grid.SetRow(txt, row);
            grid.Children.Add(txt);

            return txt;
        }

        public static TextBoxButton TextBoxButton(Grid grid, string label, bool isReadOnly,
            IEnumerable<ValidationRule> validationRules, string propertyPath, RoutedEventHandler buttonOnClick)
        {
            // check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add textbox
            var textBoxButton = new TextBoxButton()
            {
                MaxWidth = 250,
                IsEnabled = !isReadOnly
            };

            if (buttonOnClick != null)
                textBoxButton.ButtonOnClick += buttonOnClick;

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                ValidatesOnExceptions = true
            };
            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }
            // set binding
            BindingOperations.SetBinding(textBoxButton, AppStyle.Controls.TextBoxButton.TextProperty, bind);

            // add textboxButton to the details grid
            Grid.SetColumn(textBoxButton, 1);
            Grid.SetRow(textBoxButton, row);
            grid.Children.Add(textBoxButton);

            return textBoxButton;
        }

        public static TextBoxButton InsertTextBoxButton(Grid grid, int row, string label, bool isReadOnly,
            IEnumerable<ValidationRule> validationRules, string propertyPath, RoutedEventHandler buttonOnClick)
        {
            // check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int newRow = grid.RowDefinitions.IndexOf(rowDef);

            row = UpdateRows(newRow, row, grid);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add textbox
            var textBoxButton = new TextBoxButton()
            {
                MaxWidth = 250,
                IsEnabled = !isReadOnly
            };

            if (buttonOnClick != null)
                textBoxButton.ButtonOnClick += buttonOnClick;

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                ValidatesOnExceptions = true
            };
            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }
            // set binding
            BindingOperations.SetBinding(textBoxButton, AppStyle.Controls.TextBoxButton.TextProperty, bind);

            // add textboxButton to the details grid
            Grid.SetColumn(textBoxButton, 1);
            Grid.SetRow(textBoxButton, row);
            grid.Children.Add(textBoxButton);

            return textBoxButton;
        }

        #endregion

        #region NumericBox

        public static TextBox NumericBox(Grid grid, BindPropertyInfo bindPropertyInfo, MaskType maskType)
        {
            return NumericBox(grid, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, maskType, bindPropertyInfo.Name);
        }

        public static TextBox NumericBox(Grid grid, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules,
            MaskType maskType, string propertyPath)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add numericBox
            var numericBox = new TextBox()
            {
                MaxWidth = 250,
                IsReadOnly = isReadOnly
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                ValidatesOnExceptions = true,
                StringFormat = maskType == MaskType.Decimal ? "##0.00" : "0",
                ConverterCulture = AppSettings.DecimalSeparator == "."
                    ? CultureInfo.CreateSpecificCulture("en-US")
                    : CultureInfo.CreateSpecificCulture("de-DE")
            };

            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }

            // set binding
            BindingOperations.SetBinding(numericBox, System.Windows.Controls.TextBox.TextProperty, bind);

            // add numericBox to the details grid
            Grid.SetColumn(numericBox, 1);
            Grid.SetRow(numericBox, row);
            grid.Children.Add(numericBox);

            return numericBox;
        }

        #endregion

        #region CheckBox

        public static CheckBox CheckBox(Grid grid, BindPropertyInfo bindPropertyInfo)
        {
            return CheckBox(grid, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly, bindPropertyInfo.Name);
        }

        public static CheckBox CheckBox(Grid grid, string label, bool isReadOnly, string propertyPath)
        {
            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create checkbox
            var checkBox = new CheckBox()
            {
                IsThreeState = false,
                Content = label,
                IsEnabled = !isReadOnly
            };

            // set binding
            BindingOperations.SetBinding(checkBox, System.Windows.Controls.CheckBox.IsCheckedProperty, new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                NotifyOnTargetUpdated = true,
                NotifyOnSourceUpdated = true
            });

            // add checkbox to the details grid
            Grid.SetColumn(checkBox, 0);
            Grid.SetColumnSpan(checkBox, 2);
            Grid.SetRow(checkBox, row);
            grid.Children.Add(checkBox);

            return checkBox;
        }

        #endregion

        #region ComboBox

        public static ComboBox ComboBox(Grid grid, BindPropertyInfo bindPropertyInfo)
        {
            return ComboBox(grid, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, bindPropertyInfo.SelectedValuePath, bindPropertyInfo.DisplayMemberPath,
                        bindPropertyInfo.SelectedValue, bindPropertyInfo.Variants);
        }

        public static ComboBox ComboBox(Grid grid, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules, string selectedValuePath,
            string displayMemberPath, string propertyPath, IEnumerable itemsSource)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // add label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // create combobox
            var combo = new ComboBox()
            {
                ItemsSource = itemsSource,
                Margin = new Thickness(10),
                IsEnabled = !isReadOnly,
                SelectedValuePath = selectedValuePath,
                DisplayMemberPath = displayMemberPath,
                Width = 250
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                Mode = BindingMode.TwoWay,
                ValidatesOnExceptions = true
            };

            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }

            // set binding
            BindingOperations.SetBinding(combo, System.Windows.Controls.ComboBox.SelectedValueProperty, bind);

            // add combobox to the details grid
            Grid.SetColumn(combo, 1);
            Grid.SetRow(combo, row);
            grid.Children.Add(combo);

            return combo;
        }
        
        public static ComboBox InsertComboBox(Grid grid, int row, BindPropertyInfo bindPropertyInfo)
        {
            return InsertComboBox(grid, row, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, bindPropertyInfo.SelectedValuePath, bindPropertyInfo.DisplayMemberPath,
                        bindPropertyInfo.SelectedValue, bindPropertyInfo.Variants);
        }

        public static ComboBox InsertComboBox(Grid grid, int row, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules, string selectedValuePath,
            string displayMemberPath, string propertyPath, IEnumerable itemsSource)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int newRow = grid.RowDefinitions.IndexOf(rowDef);

            row = UpdateRows(newRow, row, grid);

            // add label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // create combobox
            var combo = new ComboBox()
            {
                ItemsSource = itemsSource,
                Margin = new Thickness(10),
                IsEnabled = !isReadOnly,
                SelectedValuePath = selectedValuePath,
                DisplayMemberPath = displayMemberPath,
                Width = 250
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                Mode = BindingMode.TwoWay,
                ValidatesOnExceptions = true
            };

            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }

            // set binding
            BindingOperations.SetBinding(combo, System.Windows.Controls.ComboBox.SelectedValueProperty, bind);

            // add combobox to the details grid
            Grid.SetColumn(combo, 1);
            Grid.SetRow(combo, row);
            grid.Children.Add(combo);

            return combo;
        }

        #endregion

        #region DatePicker

        public static DatePicker DatePicker(Grid grid, BindPropertyInfo bindPropertyInfo)
        {
            return DatePicker(grid, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, bindPropertyInfo.Name);
        }

        public static DatePicker DatePicker(Grid grid, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules, string propertyPath)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add datePicker
            var datePicker = new DatePicker()
            {
                Margin = new Thickness(10),
                MinWidth = 250,
                IsEnabled = !isReadOnly
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                Mode = BindingMode.TwoWay,
                ValidatesOnExceptions = true,
                StringFormat = AppSettings.DateFormat
            };
            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }

            // set binding
            BindingOperations.SetBinding(datePicker, System.Windows.Controls.DatePicker.SelectedDateProperty, binding: bind);

            // add datePicker to the details grid
            Grid.SetColumn(datePicker, 1);
            Grid.SetRow(datePicker, row);
            grid.Children.Add(datePicker);

            return datePicker;
        }

        public static DatePicker InsertDatePicker(Grid grid, int row, BindPropertyInfo bindPropertyInfo)
        {
            return InsertDatePicker(grid, row, bindPropertyInfo.Header, bindPropertyInfo.IsReadOnly,
                        bindPropertyInfo.ValidationRules, bindPropertyInfo.Name);
        }

        public static DatePicker InsertDatePicker(Grid grid, int row, string label, bool isReadOnly, IEnumerable<ValidationRule> validationRules, string propertyPath)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int newRow = grid.RowDefinitions.IndexOf(rowDef);

            row = UpdateRows(newRow, row, grid);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add datePicker
            var datePicker = new DatePicker()
            {
                Margin = new Thickness(10),
                MinWidth = 250,
                IsEnabled = !isReadOnly
            };

            var bind = new Binding(propertyPath)
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                ValidatesOnDataErrors = true,
                NotifyOnValidationError = true,
                ValidatesOnNotifyDataErrors = true,
                Mode = BindingMode.TwoWay,
                ValidatesOnExceptions = true,
                StringFormat = AppSettings.DateFormat
            };
            if (validationRules != null)
            {
                // add rules for validation
                foreach (var validationRule in validationRules)
                {
                    validationRule.ValidatesOnTargetUpdated = true;
                    bind.ValidationRules.Add(validationRule);
                }
            }

            // set binding
            BindingOperations.SetBinding(datePicker, System.Windows.Controls.DatePicker.SelectedDateProperty, binding: bind);

            // add datePicker to the details grid
            Grid.SetColumn(datePicker, 1);
            Grid.SetRow(datePicker, row);
            grid.Children.Add(datePicker);

            return datePicker;
        }

        #endregion

        #region PasswordBox

        public static PasswordBox PasswordBox(Grid grid, string label, bool isReadOnly, string propertyPath, object source)
        {
            //check column definitions
            CheckColumnDefinitions(grid);

            // get property info
            PropertyInfo pi = source.GetType().GetProperty(propertyPath);

            // add new definitions to the details grid
            var rowDef = new RowDefinition() { Height = GridLength.Auto };
            grid.RowDefinitions.Add(rowDef);
            int row = grid.RowDefinitions.IndexOf(rowDef);

            // create label 
            var lab = new Label() { Content = label };
            Grid.SetColumn(lab, 0);
            Grid.SetRow(lab, row);
            grid.Children.Add(lab);

            // add PasswordBox
            var psw = new PasswordBox()
            {
                MaxWidth = 250,
                IsEnabled = !isReadOnly,
                Password = (string)pi.GetValue(source)
            };

            psw.PasswordChanged += (sender, args) => pi.SetValue(source, psw.Password);

            // add PasswordBox to the details grid
            Grid.SetColumn(psw, 1);
            Grid.SetRow(psw, row);
            grid.Children.Add(psw);

            return psw;
        }

        #endregion

        #region Private methods

        private static void CheckColumnDefinitions(Grid grid)
        {
            if (grid.ColumnDefinitions.Count < 2)
            {
                grid.ColumnDefinitions.Clear();

                var colDef = new ColumnDefinition() { Width = GridLength.Auto };
                var colDef1 = new ColumnDefinition() { Width = GridLength.Auto };

                grid.ColumnDefinitions.Add(colDef);
                grid.ColumnDefinitions.Add(colDef1);
            }
        }

        private static int UpdateRows(int newRow, int row, Grid grid)
        {
            if (newRow > row)
            {
                foreach (UIElement item in grid.Children)
                {
                    int itemRow = Grid.GetRow(item);

                    // find items after need row
                    if (itemRow >= row)
                    {
                        // move items to the next row
                        Grid.SetRow(item, itemRow + 1);
                    }
                }
            }
            else if (newRow < row)
            {
                row = newRow;
            }

            return row;
        }

        #endregion
    }
}
