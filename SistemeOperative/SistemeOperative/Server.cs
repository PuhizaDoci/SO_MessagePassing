using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;

namespace SistemeOperative
{
    class Server
    {
        //_______________________Funksioni Main, punuar nga Medina Krelani_______________
        static void Main(string[] args)
        {
            //Krijohet një instancë e klasës IPEndPoint e cila merr një IP adresë dhe një port për koneksion
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
            //Serveri fillon të dëgjojë
            TcpListener listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine(@"  
            ===================================================  
                   Serveri po dëgjon në: {0} Porti :{1}  
            ===================================================",
            ep.Address, ep.Port);

            // Pjesa e serverit. Unaza përsëritet pa u ndërprerë, dmth serveri rrin duke dëgjuar në portin 1234
            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                //Pranohet kërkesa e Klientit për lidhje me server
                var sender = listener.AcceptTcpClient();
                //Lexon të dhënat e ardhura nga Klienti dhe i ruan ato në një Byte Array
                sender.GetStream().Read(buffer, 0, bytesize);

                message = cleanMessage(buffer);

                // Ruan të dhënat e ardhura nga Klienti  
                Person person = JsonConvert.DeserializeObject<Person>(message); // Deserializohet mesazhi 
                Console.WriteLine("Mesazh nga: " + person.Name);

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes("Faleminderit që dërguat mesazh, " + person.Name);
                sender.GetStream().Write(bytes, 0, bytes.Length); // Dërgon pergjigjen 

                //thirret funksioni sendEmail i inicializuar më poshtë
                sendEmail(person);
            }
        }
        /* Funksioni sendEmail merr të dhënat e ardhura nga Klienti dhe i përdor ato për dërgimin e një email*/
        //__________________SendEmail dhe CleanMessage, punuar nga Puhizë Doçi______________________
        private static void sendEmail(Person p)
        {
            try
            {

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 465))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("rrezartasallauka@gmail.com", "049552503rr.");
                    // Email dergohet
                    client.Send(
                        new MailMessage("rrezartasallauka@gmail.com", p.Email,
                            "Programi jonë me WebService!",
                            string.Format(@"Faleminderit që përdorët programin tonë, {0}.  
                            Mesazhi është marrë me sukses, '{1}'.", p.Name, p.Message
                            )
                        )
                    );
                }

                Console.WriteLine("Email u dërgua në: " + p.Email);
            }
            catch (Exception e)
            {
                Console.WriteLine("Email nuk ka mundur të dërgohet!");
            }
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        // Sends the message string using the bytes provided.  
        //___________________Punuar Puhizë Doçi__________________
        private static void sendMessage(byte[] bytes, TcpClient client)
        {
            client.GetStream()
                .Write(bytes, 0,
                bytes.Length); // Send the stream  
        }

    }
    //________Klasa e punuar nga Rrezarta Sallauka______________________
    public class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }



}