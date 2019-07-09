/*
1. Написать на C# утилиту, которая мониторит процессы Windows и "убивает" те процессы, которые работают слишком долго.
- На входе три параметра: название процесса, допустимое время жизни (в минутах) и частота проверки (в минутах).
- Утилита запускается из командной строки. При старте она считывает три входных параметра и начинает мониторить 
процессы с указанной частотой. Если процесс живет слишком долго – завершает его и выдает сообщение в лог.
Пример запуска:
monitor.exe notepad 5 1
С такими параметрами утилита раз в минуту проверяет, не живет ли процесс notepad больше пяти минут, и 
"убивает" его, если живет.
*/


using System;
using System.Diagnostics;
using System.Threading;

namespace AppKiller
{
    class Dto
    {
        public static string ProcessName; public static int Lifetime;
        public static int Frequency;
    }
    
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            // Для работы без передачи аргументов в Main
            /*Console.WriteLine("Введите название процесса");
            Dto.ProcessName = Console.ReadLine();
            Console.WriteLine("Введите допустимое время жизни процесса (в минутах)");
            while (!int.TryParse(Console.ReadLine(), out Dto.Lifetime))
                Console.WriteLine("Ошибка! Введите минуты целым числом");
            Console.WriteLine("Введите частоту проверки процесса (в минутах)");
            int process_check;
            while (!int.TryParse(Console.ReadLine(), out process_check))
                Console.WriteLine("Ошибка! Введите минуты целым числом");
            int check = process_check * 60000;
            Console.WriteLine("Приложение запущено. Для завершения работы нажмите Enter");*/
            
            if (args.Length == 3)
            {
                Dto.ProcessName = args[0];
                string life = args[1];
                Dto.Lifetime = Int32.Parse(life);
                while (!int.TryParse(args[1] , out Dto.Lifetime))
                    Console.WriteLine("Ошибка! Введите название процесса, время жизни процесса (в минутах) и " +
                                      "частоту проверки процесса (в минутах)");
                string frequency = args[2];
                Dto.Frequency = ((Int32.Parse(frequency)) * 60000);
                while (!int.TryParse(args[2] , out Dto.Frequency))
                    Console.WriteLine("Ошибка! Введите название процесса, время жизни процесса (в минутах) и " +
                                      "частоту проверки процесса (в минутах)");
                Console.WriteLine("Приложение запущено. Для завершения работы нажмите Enter");
                int num = 0;
                TimerCallback tm = new TimerCallback(Killer);
                using (Timer timer = new Timer(tm, num, 0, Dto.Frequency))
                {
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Ошибка! Введите название процесса, время жизни процесса (в минутах) и " +
                                      "частоту проверки процесса (в минутах)");
            }
        }

        public static void Killer(object obj)
        {
            int x = (int)obj;
            Process[] processes = System.Diagnostics.Process.GetProcessesByName(Dto.ProcessName);
            foreach (Process prc in processes)
            {
                TimeSpan timeLifeFact = DateTime.Now - prc.StartTime; // время жизни процесса
                TimeSpan timeFrequency = ((prc.StartTime.AddMinutes(Dto.Lifetime)) - prc.StartTime); // частота проверки
                                                                                                     // в TimeSpan
                if (timeLifeFact >= timeFrequency)
                {
                    Console.WriteLine("Завершен процесс: {0} , Время жизни: {1} ", 
                        Dto.ProcessName, Convert.ToString(timeLifeFact));
                    try
                    {
                        prc.Kill();
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка доступа");
                    }
                }
                else break;
            }
        }
    }
}
