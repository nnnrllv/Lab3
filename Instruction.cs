
namespace Lab3
{
    internal class Instruction
    {
        public static void PrintInstructions()
        {
            Console.WriteLine("Справка по использованию калькулятора:");
            Console.WriteLine("Введите команду в первой строке.");
            Console.WriteLine("Команда может быть либо числом, либо операцией.");
            Console.WriteLine("Если команда представляет собой число, то оно вводится как операнд.");
            Console.WriteLine("Если команда является операцией, она вводится в виде одного из следующих символов:");
            Console.WriteLine("+, -, /, *");
            Console.WriteLine("# - чтобы вернуться к определённому результату");
            Console.WriteLine("q - чтобы выйти из программы.");
        }
    }
}
