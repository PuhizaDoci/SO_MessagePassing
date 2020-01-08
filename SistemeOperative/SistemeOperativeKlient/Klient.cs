using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using SistemeOperative;

namespace SistemeOperativeKlient
{
    public class Klient
    {
        // _____________Funksioni Main punuar nga Rrezarta Sallauka_____________
        static void Main(string[] args)
        {
            //Krijon një instancë të klasës Person
            Person person = new Person();
            //Kërkon nga useri të shkruajë të dhënat që shkojnë si parametra të klasës Person
            Console.Write("Enter your name: ");
            person.Name = Console.ReadLine();
            Console.Write("Enter your email address: ");
            person.Email = Console.ReadLine();
            Console.Write("Enter your message: ");
            person.Message = Console.ReadLine();

            /*Të dhënat në objektin "person" serializohen, enkodohen në bajta dhe me anë të funksionit sendMessage (i krijuar më poshtë)
             këto bajta dërgohen tek serveri*/
            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(person)));
        }

        //funksioni sendMessage i cili dërgon informatat nga klienti në server,
        //_________________Punuar nga Hajrije Mjeku______________________________
        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;


            try // Provon të kryej lidhjen me server dhe t'i dërgojë bajtat
            {

                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Krijohet një koneksion i ri
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Shkruhen bajtat  
                Console.WriteLine("=============================================");
                Console.WriteLine("=  Lidhja me server është kryer me sukses   =");
                Console.WriteLine("=============================================\n");


                messageBytes = new byte[bytesize]; // fshihen bajtat
                // Lexohen bajtat e ardhur nga server
                
                stream.Read(messageBytes, 0, messageBytes.Length);


                // Mbyllet soketa
                stream.Dispose();
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Shtypni një karakter për të vazhduar!");
            Console.ReadLine();
            return messageBytes; // Kthehet pergjigja     

        }
    }

}