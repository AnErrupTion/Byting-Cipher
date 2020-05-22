using System;
using System.IO;
using System.Text;

namespace Byting_Encrypt_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));

            Console.Write("What do you want to do? (e encrypt/d decrypt) : ");
            string choice = Console.ReadLine();
            if (choice == "e" || choice == "encrypt")
            {
                Console.Write("Input string to encrypt : ");
                string text = Console.ReadLine();
                Console.Write("Encryption key (leave empty for random) : ");
                string read = Console.ReadLine();

                int keyy;
                if (string.IsNullOrEmpty(read) || string.IsNullOrWhiteSpace(read))
                {
                    read = SecureRandoms.String(SecureRandoms.Number(35, 20));
                    Console.WriteLine($"[RANDOM KEY] {read}");
                }
                keyy = read.GetHashCode();
                keyy = int.Parse(keyy.ToString().Replace("-", string.Empty));
                int key = 0;
                foreach (char c in keyy.ToString().ToCharArray()) key += c;
                key *= key;

                Console.WriteLine($"[UNIQUE ID] {key}");
                string encrypted = Encrypt(text, key);
                Console.WriteLine($"[ENCRYPTED] {encrypted}");
                Console.WriteLine($"[DECRYPTED] {Decrypt(encrypted, key)}");

                Console.WriteLine("Key / encrypted format saved to file.");
                File.WriteAllText("formatted.txt", $"{read} / {encrypted}");
            }
            else
            {
            tryagain: Console.Write("Drag & drop the formatted file : ");
                string read = Console.ReadLine().Replace("\"", string.Empty);
                if (File.Exists(read))
                {
                    string readtext = File.ReadAllText(read);
                    string[] array = readtext.Replace(" ", string.Empty).Split('/');

                    int keyy = array[0].GetHashCode();
                    keyy = int.Parse(keyy.ToString().Replace("-", string.Empty));
                    int key = 0;
                    foreach (char c in keyy.ToString().ToCharArray()) key += c;
                    key *= key;

                    string text = array[1];
                    try
                    {
                        Console.WriteLine($"[DECRYPTED] {Decrypt(text, key)}");
                    }
                    catch (Exception ex) { Console.WriteLine($"[DECRYPTED] Error : {ex.Message}{ex.StackTrace}"); goto tryagain; }
                }
                else goto tryagain;
            }

            Console.WriteLine("\nPress any key to exit the program...");
            Console.ReadKey();
        }

        private static string Decrypt(string text, int key)
        {
            string final = string.Empty;
            char[] separators = { '&', '!', '#', '?', '%' };
            string[] array = { };

            foreach (char sep in separators)
                if (text.Contains(sep.ToString()))
                    array = text.Reverse().Split(sep);

            foreach (string str in array)
            {
                string tt = str
                    .Replace("^", "0")
                    .Replace(":", "1")
                    .Replace("$", "2")
                    .Replace("*", "3")
                    .Replace("'", "4")
                    .Replace("(", "5")
                    .Replace("-", "6")
                    .Replace("_", "7")
                    .Replace(")", "8")
                    .Replace("=", "9");
                string s = tt.FromHexString();
                bool yes = int.TryParse(s, out int result);
                //Console.WriteLine($"String parsed into int : {yes} (string : {s})"); // Debugging
                char c = (char)(result / key);
                final += c;
            }
            return final;
        }

        private static string Encrypt(string text, int key)
        {
            string final = string.Empty;
            char[] separators = { '&', '!', '#', '?', '%' };
            char sep = separators[SecureRandoms.Number(separators.Length)];

            foreach (char c in text.ToCharArray())
            {
                int asciiCode = c;
                string keyy = (key * asciiCode).ToString().ToHexString() + sep;
                final += keyy
                    .Replace("0", "^")
                    .Replace("1", ":")
                    .Replace("2", "$")
                    .Replace("3", "*")
                    .Replace("4", "'")
                    .Replace("5", "(")
                    .Replace("6", "-")
                    .Replace("7", "_")
                    .Replace("8", ")")
                    .Replace("9", "=");
            }

            return final.Remove(final.Length - 1).Reverse();
        }
    }
}
