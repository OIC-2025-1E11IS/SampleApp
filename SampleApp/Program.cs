using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace SampleApp
{
    internal class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new CalculatorForm());
        }
    }

    public class CalculatorForm : Form
    {
        private readonly TextBox display;
        private double firstNumber;
        private string operation = "";
        private bool newNumber = true;

        public CalculatorForm()
        {
            Text = "Calculator";
            ClientSize = new Size(800, 900);
            MinimumSize = new Size(800, 900);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;

            display = new TextBox
            {
                Text = "0",
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                Font = new Font("Segoe UI", 48, FontStyle.Regular),
                Dock = DockStyle.Top,
                Height = 150,
                Margin = new Padding(15)
            };

            Controls.Add(display);

            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 5,
                Padding = new Padding(18)
            };

            for (int i = 0; i < 4; i++)
            {
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            }

            for (int i = 0; i < 5; i++)
            {
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
            }

            string[] buttons =
            {
                "7", "8", "9", "÷",
                "4", "5", "6", "×",
                "1", "2", "3", "-",
                "0", ".", "=", "+",
                "C", "←", "", ""
            };

            foreach (string text in buttons)
            {
                if (text == "")
                {
                    panel.Controls.Add(new Label());
                    continue;
                }

                var button = new Button
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 30, FontStyle.Regular),
                    Margin = new Padding(8),
                    MinimumSize = new Size(120, 100)
                };

                button.Click += Button_Click;
                panel.Controls.Add(button);
            }

            Controls.Add(panel);
        }

        private void Button_Click(object? sender, EventArgs e)
        {
            if (sender is not Button button)
            {
                return;
            }

            string value = button.Text;

            if (double.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out _)
                || value == ".")
            {
                EnterNumber(value);
                return;
            }

            switch (value)
            {
                case "+":
                case "-":
                case "×":
                case "÷":
                    SetOperation(value);
                    break;
                case "=":
                    Calculate();
                    break;
                case "C":
                    Clear();
                    break;
                case "←":
                    Backspace();
                    break;
            }
        }

        private void EnterNumber(string value)
        {
            if (newNumber)
            {
                display.Text = value == "." ? "0." : value;
                newNumber = false;
                return;
            }

            if (value == "." && display.Text.Contains("."))
            {
                return;
            }

            if (display.Text == "0" && value != ".")
            {
                display.Text = value;
            }
            else
            {
                display.Text += value;
            }
        }

        private void SetOperation(string value)
        {
            if (!double.TryParse(display.Text, out firstNumber))
            {
                return;
            }

            operation = value;
            newNumber = true;
        }

        private void Calculate()
        {
            if (string.IsNullOrEmpty(operation) ||
                !double.TryParse(display.Text, out double secondNumber))
            {
                return;
            }

            double result;

            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber == 0)
                    {
                        MessageBox.Show("0で割ることはできません。", "エラー");
                        Clear();
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
                default:
                    return;
            }

            display.Text = result.ToString(CultureInfo.InvariantCulture);
            operation = "";
            newNumber = true;
        }

        private void Clear()
        {
            display.Text = "0";
            firstNumber = 0;
            operation = "";
            newNumber = true;
        }

        private void Backspace()
        {
            if (newNumber)
            {
                return;
            }

            if (display.Text.Length <= 1)
            {
                display.Text = "0";
            }
            else
            {
                display.Text = display.Text[..^1];
            }
        }
    }
}
