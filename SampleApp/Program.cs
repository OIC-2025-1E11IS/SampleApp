namespace SampleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Calculator ===");
            Console.WriteLine("計算例: 10 + 5");
            Console.WriteLine("終了する場合は q を入力してください。\n");

            while (true)
            {
                Console.Write("計算式 > ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("終了します。");
                    break;
                }

                string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 3)
                {
                    Console.WriteLine("エラー: 「数字 演算子 数字」の形式で入力してください。例: 10 + 5\n");
                    continue;
                }

                if (!double.TryParse(parts[0], out double number1) ||
                    !double.TryParse(parts[2], out double number2))
                {
                    Console.WriteLine("エラー: 数字を正しく入力してください。\n");
                    continue;
                }

                string operation = parts[1];
                double result;

                switch (operation)
                {
                    case "+":
                        result = number1 + number2;
                        break;
                    case "-":
                        result = number1 - number2;
                        break;
                    case "*":
                    case "×":
                        result = number1 * number2;
                        break;
                    case "/":
                    case "÷":
                        if (number2 == 0)
                        {
                            Console.WriteLine("エラー: 0で割ることはできません。\n");
                            continue;
                        }
                        result = number1 / number2;
                        break;
                    default:
                        Console.WriteLine("エラー: 演算子は +、-、*、/ のいずれかを入力してください。\n");
                        continue;
                }

                Console.WriteLine($"結果: {result}\n");
            }
        }
    }
}
