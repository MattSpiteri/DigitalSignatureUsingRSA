using DigitalSignatureTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignatureTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name:");
            string name = Console.ReadLine();
            Console.WriteLine("\r\n");

            Console.WriteLine("Enter your surname:");
            string Surname = Console.ReadLine();
            Console.WriteLine("\r\n");

            string privateKeyPath = @"..\..\Keys\private.pem";

            string publickeyPath = @"..\..\Keys\\public.pem";

            //The signature formed after signing the data using the private key
            string signature = Signature.Sign(name, Surname, privateKeyPath);

            Console.WriteLine("The data entered has been signed using the private key\r\n");
            Console.WriteLine("\r\n");

            Console.WriteLine("-----------------------------------------------------------\r\n");

            Console.WriteLine("If you enter the same exact name and surname, signature should be valid\r\n" +
                              "otherwise, the signature would be invalidated since data has changed\r\n");

            Console.WriteLine("Enter your name:");
            string name2 = Console.ReadLine();
            Console.WriteLine("\r\n");

            Console.WriteLine("Enter your surname:");
            string Surname2 = Console.ReadLine();
            Console.WriteLine("\r\n");

            //This will validate the digital signature by using the public key
            bool isValid = Signature.Verify(name2, Surname2, publickeyPath, signature);

            Console.WriteLine(@"Is the signature valid ? : {0}", isValid);

            Console.ReadKey();
        }
    }
}