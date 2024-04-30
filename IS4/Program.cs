using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace IS4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                Console.WriteLine("Please enter text to sign");
                string text = Console.ReadLine();
                // Convert the message to a byte array
                byte[] data = Encoding.UTF8.GetBytes(text);

                // Sign the data using the RSA object
                byte[] signature = rsa.SignData(data, new SHA256CryptoServiceProvider());

                RSAParameters publicKey = rsa.ExportParameters(false);
                rsa.ImportParameters(publicKey);

                SignatureInfo info = new SignatureInfo();
                info.data = data;
                info.signature = signature;
                info.parameters = RSAParser.ParamsToList(publicKey);

                using (TcpClient client = new TcpClient("localhost", 8088))
                {
                    // Get a network stream for the client socket
                    NetworkStream stream = client.GetStream();

                    // Serialize the custom class to a byte array
                    BinaryFormatter formatter = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        formatter.Serialize(ms, info);
                        byte[] buffer = ms.ToArray();

                        // Send the serialized data over the network
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
                Console.WriteLine("Sent!");
                Console.ReadLine();
            }
        }
    }
}