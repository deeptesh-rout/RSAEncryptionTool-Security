using System;
using System.Security.Cryptography;
using System.Text;

namespace RSAEncryptionTool
{
    class Program
    {
        static RSACryptoServiceProvider rsa;

        static void Main(string[] args)
        {
            Console.WriteLine("RSA Encryption/Decryption Tool");
            LoadOrGenerateKeys();

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Generate new keys");
                Console.WriteLine("2. Encrypt a message");
                Console.WriteLine("3. Decrypt a message");
                Console.WriteLine("4. View keys");
                Console.WriteLine("5. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        GenerateKeys();
                        break;
                    case "2":
                        EncryptMessage();
                        break;
                    case "3":
                        DecryptMessage();
                        break;
                    case "4":
                        ViewKeys();
                        break;
                    case "5":
                        SaveKeys();
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void LoadOrGenerateKeys()
        {
            try
            {
                string publicKey = System.IO.File.ReadAllText("publicKey.xml");
                string privateKey = System.IO.File.ReadAllText("privateKey.xml");

                rsa = new RSACryptoServiceProvider(2048);
                rsa.FromXmlString(privateKey);

                Console.WriteLine("Keys loaded successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine("No keys found. Generating new keys...");
                GenerateKeys();
            }
        }

        static void GenerateKeys()
        {
            rsa = new RSACryptoServiceProvider(2048);
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);

            System.IO.File.WriteAllText("publicKey.xml", publicKey);
            System.IO.File.WriteAllText("privateKey.xml", privateKey);

            Console.WriteLine("New RSA keys generated and saved to files.");
        }

        static void SaveKeys()
        {
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);

            System.IO.File.WriteAllText("publicKey.xml", publicKey);
            System.IO.File.WriteAllText("privateKey.xml", privateKey);

            Console.WriteLine("Keys saved to files.");
        }

        static void EncryptMessage()
        {
            Console.Write("Enter the message to encrypt: ");
            string message = Console.ReadLine();
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            try
            {
                byte[] encryptedBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.Pkcs1);
                string encryptedMessage = Convert.ToBase64String(encryptedBytes);
                Console.WriteLine($"Encrypted message: {encryptedMessage}");
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Encryption failed: {ex.Message}");
            }
        }

        static void DecryptMessage()
        {
            Console.Write("Enter the message to decrypt: ");
            string encryptedMessage = Console.ReadLine();

            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);
                Console.WriteLine($"Decrypted message: {decryptedMessage}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid encrypted message format.");
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Decryption failed: {ex.Message}");
            }
        }

        static void ViewKeys()
        {
            string publicKey = rsa.ToXmlString(false);
            string privateKey = rsa.ToXmlString(true);

            Console.WriteLine("\nPublic Key:");
            Console.WriteLine(publicKey);

            Console.WriteLine("\nPrivate Key:");
            Console.WriteLine(privateKey);
        }
    }
}
