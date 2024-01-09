using System.IO;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using System.Data.SQLite;

namespace Lab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Instruction.PrintInstructions();
            bool exit = false; // Указывает, следует ли завершать работу программы

            while (!exit)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1. Сохранить данные в JSON");
                Console.WriteLine("2. Загрузить данные из JSON");
                Console.WriteLine("3. Сохранить данные в XML");
                Console.WriteLine("4. Загрузить данные из XML");
                Console.WriteLine("5. Сохранить данные в SQLite");
                Console.WriteLine("6. Загрузить данные из SQLite");
                Console.WriteLine("7. Обратиться к калькулятору");

                string input = Console.ReadLine();
                if (input == "q") // Завершение программы
                {
                    exit = true;
                    continue;
                }

                if (input == "1")
                {
                    Console.WriteLine("Введите название файла, чтобы сохранить данные: ");
                    string fileName = Console.ReadLine();
                    SaveToJson(fileName);
                }
                else if (input == "2")
                {
                    Console.WriteLine("Введите название файла, чтобы загрузить данные: ");
                    string fileName = Console.ReadLine();
                    LoadFromJson(fileName);
                }
                else if (input == "3")
                {
                    Console.WriteLine("Введите название файла, чтобы сохранить данные: ");
                    string fileName = Console.ReadLine();
                    SaveToXml(fileName);
                }
                else if (input == "4")
                {
                    Console.WriteLine("Введите название файла, чтобы загрузить данные: ");
                    string fileName = Console.ReadLine();
                    LoadFromXml(fileName);
                }
                else if (input == "5")
                {
                    Console.WriteLine("Введите название базы данных, чтобы сохранить данные: ");
                    string dbFileName = Console.ReadLine();
                    SaveToSQLite(dbFileName);
                }
                else if (input == "6")
                {
                    Console.WriteLine("Введите название базы данных, чтобы загрузить данные: ");
                    string dbFileName = Console.ReadLine();
                    LoadFromSQLite(dbFileName);
                }
                else if (input == "7")
                {
                    while (true)
                    {
                        input = Console.ReadLine();
                        double inputNumber;
                        bool isDouble = double.TryParse(input, out inputNumber);
                        if (isDouble)
                        {
                            Calculator.ProcessInputNumber(inputNumber);
                        }
                        else
                        {
                            Calculator.ProcessInputOperation(input);
                        }
                        if (Calculator.exit) // Выход из калькулятора
                          break;
                    }
                }
                else
                {
                    Console.WriteLine("Неправильный выбор");
                }
            }
        }

        public static void SaveToJson(string fileName)
        {
            string jsonData = JsonConvert.SerializeObject(Calculator.results);
            File.WriteAllText(fileName, jsonData);
            Console.WriteLine("Данные сохранены JSON");
        }

        public static void LoadFromJson(string fileName)
        {
            string jsonData = File.ReadAllText(fileName);
            Calculator.results = JsonConvert.DeserializeObject<List<double>>(jsonData);
            Console.WriteLine("Данные загружены из JSON");
        }

        public static void SaveToXml(string fileName)
        {
            using (XmlWriter writer = XmlWriter.Create(fileName))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("results");
                foreach (double item in Calculator.results)
                {
                    writer.WriteElementString("result", item.ToString());
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Console.WriteLine("Данные сохранены XML");
        }

        public static void LoadFromXml(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            Calculator.results.Clear();
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                Calculator.results.Add(double.Parse(node.InnerText));
            }
            Console.WriteLine("Данные загружены из XML");
        }

        public static void SaveToSQLite(string dbFileName)
        {
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbFileName};Version=3;"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS results (id INTEGER PRIMARY KEY, value REAL)", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                foreach (double item in Calculator.results)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO results (value) VALUES (@value)", conn))
                    {
                        cmd.Parameters.AddWithValue("@value", item);
                        cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Данные сохранены в SQLite");
                conn.Close();
            }
        }

        public static void LoadFromSQLite(string dbFileName)
        {
            Calculator.results.Clear();
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={dbFileName};Version=3;"))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand("SELECT value FROM results", conn))
                {
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Calculator.results.Add(reader.GetDouble(0));
                        }
                    }
                }
                Console.WriteLine("Данные загружены SQLite");
                conn.Close();
            }
        }
    }
}