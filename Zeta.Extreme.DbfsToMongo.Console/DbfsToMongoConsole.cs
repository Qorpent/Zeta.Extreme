using DbfsToMongo.Wrapper;
using System;


namespace Zeta.Extreme.DbfsToMongo.Console
{
    class DbfsToMongoConsole
    {
        static void Main()
        {
            System.Console.WriteLine(@"Сколько файлов перегнать, мой повелитель?");


            int value;
            int.TryParse(System.Console.ReadLine(), out value);

            if (value > 0)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(string.Format(@"Превосходно, мой повелитель! Будем перегонять {0} файлов", value));

                var wrapper = new DbfsToMongoWrapper();

                for (int i = 0; i < value; i++)
                {
                    System.Console.Write(@"Ухнем! ");
                    wrapper.MigrateNextAttachmentToMongo();
                }

                System.Console.WriteLine();
                System.Console.WriteLine(@"Осмеюсь доложить, мой повелитель, что Ваш безвольный раб закончил работу! ");
                System.Console.WriteLine(@"Нажмите <Enter>, мой повелитель!");
            }
            else
            {
                System.Console.WriteLine(@"До встречи, мой повелитель!");
            }

            System.Console.ReadKey();

        }
    }
}