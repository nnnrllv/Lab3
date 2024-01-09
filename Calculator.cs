
namespace Lab3
{
    public class Calculator
    {
        public static List<double> results = new List<double>(); // Список для хранения результатов
        public static double result = 0; // Переменная для хранения текущего результата
        public static char operation = ' '; // Переменная для хранения текущей операции
        public static bool exit = false;

        public static void ProcessInputNumber(double inputNumber)
        {
            if (operation == ' ')
            {
                result = inputNumber;
            }
            else
            {
                switch (operation)
                {
                    case '+':
                        result += inputNumber;
                        break;
                    case '-':
                        result -= inputNumber;
                        break;
                    case '*':
                        result *= inputNumber;
                        break;
                    case '/':
                        if (inputNumber == 0)
                        {
                            Console.WriteLine("Ошибка: нельзя делить на ноль");
                            return;
                        }
                        result /= inputNumber;
                        break;
                    default:
                        Console.WriteLine("Ошибка: недопустимая операция");
                        break;
                }
            }
            results.Add(result);
            Console.WriteLine($"{results.Count}: {result}");
        }

        public static void ProcessInputOperation(string input)
        {
            if (input == "выход") // Проверка на выход из калькулятора
            {
                exit = true;
            }
            else if (input == "#")
            {
                Console.WriteLine("Введите номер результата, к которому хотите вернуться:");
                string index = Console.ReadLine();
                if (int.TryParse(index, out int i) && i > 0 && i <= results.Count)
                {
                    result = results[i - 1];
                    Console.WriteLine($"{results.Count + 1}: {result}");
                }
                else
                {
                    Console.WriteLine("Ошибка: недопустимый номер результата");
                }
            }
            else if ("+-*/".Contains(input[0]))
            {
                operation = input[0];
            }
            else
            {
                Console.WriteLine("Ошибка: недопустимая команда");
            }
        }
    }
}

