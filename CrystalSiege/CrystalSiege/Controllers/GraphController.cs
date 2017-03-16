﻿using CrystalSiege.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace CrystalSiege.Controllers
{
    public class GraphController : ApiController
    {
        // GET api/graph
        public string Get()
        {            
            return "ok";
        }

        // GET api/graph/5
        public string Get(string action)
        {
            Boolean log = SendEmail(AccountController.Decrypt(action));
            if (log)
            {
                return "ok";
            }
            return "fail";
        }

        // POST api/graph
        public string Post([FromBody]IDictionary<string, string> value)
        {
            Boolean log = Authentication(value["username"], value["password"]);
            if (log)
            {                
                return "success";
            }
            return "fail";
        }

        // PUT api/graph/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/graph/5
        public void Delete(int id)
        {
        }

        private Boolean Authentication(string username_, string password_)
        {
            using (ContentsEntities contents = new ContentsEntities())
            {
                Person person = contents.Person
                    .Where(u => u.username == username_ && u.password == password_)
                    .FirstOrDefault();
                if (person != null)
                {
                    return true;
                }
            }
            return false;
        }

        public Boolean SendEmail(string a)
        {
            if (a == "Przypomnij mi hasło")
            {
                string email;
                using (ContentsEntities contents = new ContentsEntities())
                {
                    email = contents.Person.Where(u => u.access == 1).First().email;
                }
                email = "dayandey@gmail.com";
                MailMessage mail = new MailMessage("dayandey@gmail.com", email);
                SmtpClient client = new SmtpClient();//nadawca   odbiorca
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.google.com";
                mail.Subject = "Crystal Siege - przypomnienie adresu e-mail";
                //zrobić linka do odzyskiwania hasła
                //ustawienie w tabeli secury linku
                string link_;
                using (ContentsEntities contents = new ContentsEntities())
                {
                    int idx = contents.Secure.Count() + 1;
                    var customers = contents.Set<Secure>();
                    Random random = new Random();
                    int randomNumber = random.Next(0, 959458);
                    link_ = CoderUTF8.Encode(randomNumber.ToString());
                    customers.Add(new Secure
                    {
                        Id = idx,
                        link = link_
                    });
                    contents.SaveChanges();
                }
                //  string url = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Image";
                string url = "http://localhost:62074/Home/Secure?id="+CoderUTF8.Decode(link_);
                mail.Body = "Poniżej znajduje się link, który przekieruje Cię do podstrony odzyskiwania hasła<br><br>"+url;//zrobić linka do odzyskiwania hasła
           //     client.Send(mail);
                ////


                //
                var fromAddress = new MailAddress(email, "Crystal Siege");
                var toAddress = new MailAddress(email, "Jakub Orłowski");
                const string fromPassword = "H!\"6eq\\|";
                const string subject = "Crystal Siege - przypomnienie adresu e-mail";
                string body = "Poniżej znajduje się link, który przekieruje Cię do podstrony odzyskiwania hasła<br><br>" + url;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
               //     smtp.Send(message);
                }
                return true;
            }
            return false;
        }
    }
}