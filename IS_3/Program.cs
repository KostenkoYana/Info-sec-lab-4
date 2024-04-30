using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace IS_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Start a TcpListener on a specified port
            TcpListener listener = new TcpListener(IPAddress.Any, 8089);
            listener.Start();
            IS4.SignatureInfo data;
            // Accept a new client connection
            using (TcpClient client = listener.AcceptTcpClient())
            {
                // Get a network stream for the client socket
                NetworkStream stream = client.GetStream();

                // Receive the serialized data from the client
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

                // Deserialize the data into an instance of the custom class
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
                {
                    data = (IS4.SignatureInfo)formatter.Deserialize(ms);

                    // Use the deserialized data as needed
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(IS4.RSAParser.ListToParams(data.parameters));
                        bool verified = rsa.VerifyData(data.data, new SHA256CryptoServiceProvider(), data.signature);
                        Console.WriteLine("Verification status: " + verified);
                    }
                }
            }
        }
    }
}