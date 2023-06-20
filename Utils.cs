using System.Text.Json;
using System.Random;

namespace Audio_Bot
{
    class Utils
    {
        static Dictionary<string,string> ConfigCheck()
        {
            Dictionary<string,string> config;
            if(File.Exists("config.json"))
            {
                string data = File.ReadAllText("config.json");
                config = JsonSerializer.Deserialize<Dictionary<string,string>>(data)
                return config;
            }
            Console.WriteLine("No config detected starting config setup");
            config = ConfigSetup();
            string data = JsonSerializer.Serialize(config);
            File.WriteAllText("config.json",data);
            return config
        }
        static Dictionary<string,string> ConfigSetup()
        {
            Dictionary<string,string> config = Dictionary<string,string>();
            Console.Write("Enter the discord bot login Token\n >");
            config["Token"] = Console.ReadLine();
            Console.Write("Choose a password for lavalink (optional)\n >");
            string pass = Console.ReadLine();
            if(pass == string.Empty)
            {
                config["LLPassword"] = RandomPassword();
            }
            else
            {
                config["LLPassword"] = pass;
            }
            return config;
        }
        static string RandomPassword()
        {
            string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            string buff = string.Empty;
            Random rnd = new Random();
            for(int i = 0;i < 20;i++){
                int index = rnd.Next(0,chars.Length);
                buff += chars[index];
            }
            return buff;
        }
    }
}