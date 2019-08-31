using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AppStyle.Controls
{
    public enum MaskType
    {
        Decimal,
        Integer
    }

    public class AppNumericBox : TextBox, INotifyPropertyChanged
    {
        public AppNumericBox()
            : base()
        {
            //this.PreviewTextInput += NumericBox_PreviewTextInput;
            DataObject.AddPastingHandler(this, NumericBox_PastingEventHandler);
            //this.TextChanged += (sender, args) => ValidateNumericBox(this);
        }

        #region DecimalSeparator Property

        public string DecimalSeparator
        {
            get { return (string)this.GetValue(DecimalSeparatorProperty); }
            set
            {
                this.SetValue(DecimalSeparatorProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty DecimalSeparatorProperty = DependencyProperty.Register(
            "DecimalSeparator", typeof(string), typeof(AppNumericBox),
            new FrameworkPropertyMetadata(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator));

        #endregion

        #region MinValue Property

        public double MinValue
        {
            get { return (double)this.GetValue(MinValueProperty); }
            set
            {
                this.SetValue(MinValueProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue", typeof(double), typeof(AppNumericBox),
            new FrameworkPropertyMetadata(double.NaN, MinValueChangedCallback));

        private static void MinValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidateNumericBox(d as AppNumericBox);
        }

        #endregion

        #region MaxValue Property

        public double MaxValue
        {
            get { return (double)this.GetValue(MaxValueProperty); }
            set
            {
                this.SetValue(MaxValueProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue", typeof(double), typeof(AppNumericBox),
            new FrameworkPropertyMetadata(double.NaN, MaxValueChangedCallback));

        private static void MaxValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ValidateNumericBox(d as AppNumericBox);
        }

        #endregion

        #region Mask Property

        public MaskType Mask
        {
            get { return (MaskType)this.GetValue(MaskProperty); }
            set
            {
                this.SetValue(MaskProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty MaskProperty = DependencyProperty.Register(
            "Mask", typeof(MaskType), typeof(AppNumericBox),
            new FrameworkPropertyMetadata(MaskChangedCallback));
        
        private static void MaskChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = (d as AppNumericBox);
            if (numericBox == null)
                return;

            ValidateNumericBox(numericBox);
        }

        #endregion

        #region Private Static Methods

        private static void ValidateNumericBox(AppNumericBox numericBox)
        {
            numericBox.Text = ValidateValue(numericBox.Mask, numericBox.Text, numericBox.MinValue, numericBox.MaxValue);
        }

        private static void NumericBox_PastingEventHandler(object sender, DataObjectPastingEventArgs e)
        {
            var numericBox = (sender as AppNumericBox);
            string clipboard = e.DataObject.GetData(typeof(string)) as string;

            clipboard = ValidateValue(numericBox.Mask, clipboard, numericBox.MinValue, numericBox.MaxValue);

            if (!string.IsNullOrEmpty(clipboard))
            {
                numericBox.Text = clipboard;
            }

            e.CancelCommand();
            e.Handled = true;
        }

        private static void NumericBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            var numericBox = (sender as AppNumericBox);
            bool isValid = IsSymbolValid(numericBox.Mask, numericBox.DecimalSeparator, e.Text);

            e.Handled = !isValid;
            if (isValid)
            {
                int caret = numericBox.CaretIndex;
                string text = numericBox.Text;
                bool textInserted = false;
                int selectionLength = 0;

                if (numericBox.SelectionLength > 0)
                {
                    text = text.Substring(0, numericBox.SelectionStart) +
                            text.Substring(numericBox.SelectionStart + numericBox.SelectionLength);
                    caret = numericBox.SelectionStart;
                }

                if (e.Text == numericBox.DecimalSeparator)
                {
                    while (true)
                    {
                        int ind = text.IndexOf(numericBox.DecimalSeparator);
                        if (ind == -1)
                            break;

                        text = text.Substring(0, ind) + text.Substring(ind + 1);
                        if (caret > ind)
                            caret--;
                    }

                    if (caret == 0)
                    {
                        text = "0" + text;
                        caret++;
                    }
                    else
                    {
                        if (caret == 1 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign)
                        {
                            text = NumberFormatInfo.CurrentInfo.NegativeSign + "0" + text.Substring(1);
                            caret++;
                        }
                    }

                    if (caret == text.Length)
                    {
                        selectionLength = 1;
                        textInserted = true;
                        text = text + numericBox.DecimalSeparator + "0";
                        caret++;
                    }
                }
                else if (e.Text == NumberFormatInfo.CurrentInfo.NegativeSign)
                {
                    textInserted = true;
                    if (numericBox.Text.Contains(NumberFormatInfo.CurrentInfo.NegativeSign))
                    {
                        text = text.Replace(NumberFormatInfo.CurrentInfo.NegativeSign, string.Empty);
                        if (caret != 0)
                            caret--;
                    }
                    else
                    {
                        text = NumberFormatInfo.CurrentInfo.NegativeSign + numericBox.Text;
                        caret++;
                    }
                }

                if (!textInserted)
                {
                    text = text.Substring(0, caret) + e.Text +
                        ((caret < numericBox.Text.Length) ? text.Substring(caret) : string.Empty);

                    caret++;
                }

                try
                {
                    double val = Convert.ToDouble(text);
                    double newVal = ValidateLimits(numericBox.MinValue, numericBox.MaxValue, val);
                    if (val != newVal)
                    {
                        text = newVal.ToString();
                    }
                    else if (val == 0)
                    {
                        if (!text.Contains(numericBox.DecimalSeparator))
                            text = "0";
                    }
                }
                catch
                {
                    text = "0";
                }

                while (text.Length > 1 && text[0] == '0' && string.Empty + text[1] != numericBox.DecimalSeparator)
                {
                    text = text.Substring(1);
                    if (caret > 0)
                        caret--;
                }

                while (text.Length > 2 && string.Empty + text[0] == NumberFormatInfo.CurrentInfo.NegativeSign && text[1] == '0'
                    && string.Empty + text[2] != numericBox.DecimalSeparator)
                {
                    text = NumberFormatInfo.CurrentInfo.NegativeSign + text.Substring(2);
                    if (caret > 1)
                        caret--;
                }

                if (caret > text.Length)
                    caret = text.Length;

                numericBox.Text = text;
                numericBox.CaretIndex = caret;
                numericBox.SelectionStart = caret;
                numericBox.SelectionLength = selectionLength;
                e.Handled = true;
            }
        }

        private static string ValidateValue(MaskType mask, string value, double min, double max)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            value = value.Trim();

            switch (mask)
            {
                case MaskType.Integer:
                    {
                        int val;
                        if (int.TryParse(value, out val))
                        {
                            val = (int)ValidateLimits(min, max, val);
                            return val.ToString();
                        }

                        return string.Empty;
                    }
                case MaskType.Decimal:
                    {
                        double val;
                        if (double.TryParse(value, out val))
                        {
                            val = ValidateLimits(min, max, val);
                            return val.ToString();
                        }

                        return string.Empty;
                    }
            }

            return value;
        }

        private static double ValidateLimits(double min, double max, double value)
        {
            if (!min.Equals(double.NaN))
            {
                if (value < min)
                    return min;
            }

            if (!max.Equals(double.NaN))
            {
                if (value > max)
                    return max;
            }

            return value;
        }

        private static bool IsSymbolValid(MaskType mask, string decimalSeparator, string str)
        {
            switch (mask)
            {
                case MaskType.Integer:
                    if (str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;

                case MaskType.Decimal:
                    if (str == decimalSeparator ||
                        str == NumberFormatInfo.CurrentInfo.NegativeSign)
                        return true;
                    break;
            }

            foreach (char ch in str)
            {
                if (!Char.IsDigit(ch))
                    return false;
            }

            return true;
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
