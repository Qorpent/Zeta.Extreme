using Zeta.Extreme.DbfsToMongo.Wrapper;
using Zeta.Extreme.BizProcess.Forms;


namespace Zeta.Extreme.DbfsToMongo.Console
{
    class DbfsToMongoConsole
    {
        static void Main()
        {
            System.Console.WriteLine(@"Сколько файлов перегнать?");


            int value;
            int.TryParse(System.Console.ReadLine(), out value);

            if (value > 0)
            {
                System.Console.WriteLine();
                System.Console.WriteLine(string.Format(@"Будем перегонять {0} файлов", value));

                var wrapper = new DbfsToMongoWrapper();

                for (int i = 0; i < value; i++) {
                    System.Console.Write(@".");
                    var att = wrapper.MigrateNextAttachmentToMongo();
                    if (att == null) break;
                }

                System.Console.WriteLine();
                System.Console.WriteLine(@"Завершено. ");
                System.Console.WriteLine(@"Нажмите <Enter> для выхода.");
            }
            else
            {
                System.Console.WriteLine(@"Окончание выполнения. Нажмите <Enter> для выхода.");
            }

            System.Console.ReadKey();

        }
    }
}