using System;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace IS4_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Start a TcpListener on a specified port
            TcpListener listener = new TcpListener(IPAddress.Any, 8088);
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
                    Console.WriteLine("Received data");

                    // Print the received signature
                    Console.WriteLine("Received Signature:");
                    Console.WriteLine(BitConverter.ToString(data.signature));

                    // Ask the user if they want to alter the signature
                    Console.WriteLine("Do you want to change the signature? Press 1 for No, 2 for Yes.");
                    char choice = Console.ReadKey().KeyChar;

                    // Handle user choice
                    switch (choice)
                    {
                        case '1':
                            Console.WriteLine("\nProceeding without changing the signature.");
                            break;
                        case '2':
                            Console.WriteLine("\nCalling AlterSignature function to alter the signature.");
                            AlterSignature(data.signature);
                            break;
                        default:
                            Console.WriteLine("\nInvalid choice. Please press 1 or 2.");
                            return;
                    }

                    // Use the deserialized data as needed
                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                    {
                        rsa.ImportParameters(IS4.RSAParser.ListToParams(data.parameters));
                    }
                }
            }

            Console.WriteLine("Press ENTER to send details to the third application");
            //Console.WriteLine("Enter any key to send details to the third application");
            Console.ReadLine();

            using (TcpClient client = new TcpClient("localhost", 8089))
            {
                // Get a network stream for the client socket
                NetworkStream stream = client.GetStream();

                // Serialize the custom class to a byte array
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, data);
                    byte[] buffer = ms.ToArray();

                    // Send the serialized data over the network
                    stream.Write(buffer, 0, buffer.Length);
                }
            }
        }
        public static void AlterSignature(byte[] signature)
        {
            signature[0] = 50;
            signature[1] = 55;
            Console.WriteLine("Signature has been altered");
        }
    }
}


//using System.Net.Sockets;
//using System.Net;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Security.Cryptography;

//namespace IS4_2
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//            // Start a TcpListener on a specified port
//            TcpListener listener = new TcpListener(IPAddress.Any, 8088);
//            listener.Start();
//            IS4.SignatureInfo data;
//            // Accept a new client connection
//            using (TcpClient client = listener.AcceptTcpClient())
//            {
//                // Get a network stream for the client socket
//                NetworkStream stream = client.GetStream();

//                // Receive the serialized data from the client
//                byte[] buffer = new byte[client.ReceiveBufferSize];
//                int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);

//                // Deserialize the data into an instance of the custom class
//                BinaryFormatter formatter = new BinaryFormatter();
//                using (MemoryStream ms = new MemoryStream(buffer, 0, bytesRead))
//                {
//                    data = (IS4.SignatureInfo)formatter.Deserialize(ms);
//                    Console.WriteLine("Received data");

//                    Console.WriteLine("Received Signature:");
//                    Console.WriteLine(BitConverter.ToString(data.signature));

//                    // Use the deserialized data as needed
//                    using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
//                    {
//                        rsa.ImportParameters(IS4.RSAParser.ListToParams(data.parameters));
//                        //AlterSignature(data.signature);
//                    }
//                }
//            }
//            Console.WriteLine("Enter any key to send details to the third application");
//            Console.ReadLine();

//            using (TcpClient client = new TcpClient("localhost", 8089))
//            {
//                // Get a network stream for the client socket
//                NetworkStream stream = client.GetStream();

//                // Serialize the custom class to a byte array
//                BinaryFormatter formatter = new BinaryFormatter();
//                using (MemoryStream ms = new MemoryStream())
//                {
//                    formatter.Serialize(ms, data);
//                    byte[] buffer = ms.ToArray();

//                    // Send the serialized data over the network
//                    stream.Write(buffer, 0, buffer.Length);
//                }
//            }
//        }
//        public static void AlterSignature(byte[] signature)
//        {
//            signature[0] = 50;
//            signature[1] = 55;
//            Console.WriteLine("Signature has been altered");
//        }
//    }
//}