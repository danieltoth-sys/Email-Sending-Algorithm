using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

public class EmailSending
{
    /// <summary>
    /// First algorithm: Sending an email to a certain address.
    /// </summary>

    private static string? mailTo;
    private static string? subject;
    private static string? message;

    private static void Main()
    {
        try
        {
            string? inputFromUser;
            Console.WriteLine("Where to send?");
            inputFromUser = InputFromUser();
            
                mailTo = inputFromUser;

            Console.WriteLine("Subject?");
            inputFromUser = InputFromUser();
            if (!string.IsNullOrEmpty(inputFromUser))
                subject = inputFromUser;

            Console.WriteLine("Message?");
            inputFromUser = InputFromUser();
            if (!string.IsNullOrEmpty(inputFromUser))
                message = inputFromUser;

            Email.SendMailTask(mailTo, subject, message);
        }
        catch (Exception)
        {
            Console.WriteLine("An error occured! Try again!");
            Main();
            throw;
        }
    }

    private static string? InputFromUser()
    {
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
            string input = Console.ReadLine().ToString();
        #pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (string.IsNullOrEmpty(input))
        {
            throw new Exception();            
        }        
        return input;
    }
}

class Email
{    
    public static void SendMailTask(string? mailTo, string? subject, string? message, bool isHTML = false, string? attachment = null)
    {
        try
        {            
            using (System.Net.Mail.SmtpClient MailClient = new System.Net.Mail.SmtpClient())
            {
                using (MailMessage Email = new MailMessage())
                {
                    Email.Subject = subject;
                    Email.From = new MailAddress("testemail@emailaddress.com", "Test Name"); //Email address, User's name
                    Email.Body = message;

                    if (attachment != null)
                    {
                        string[] aa = attachment.Split('\n');
                        foreach (var fn in aa)
                        {
                            if (fn.Trim() != "" && File.Exists(fn.Trim()))
                            {
                                var a = new Attachment(fn.Trim());
                                Email.Attachments.Add(a);
                            }
                        }
                    }

                    Email.IsBodyHtml = isHTML;

                    #pragma warning disable CS8602 // Dereference of a possibly null reference.
                        string[] mt = mailTo.Split(',');
                    #pragma warning restore CS8602 // Dereference of a possibly null reference.
                    for (int i = 0; i < mt.Length; i++) if (mt[i][0] != '*') Email.To.Add(mt[i].Trim());
                    if (Email.To.Count > 0)
                    {
                        MailClient.Host = "TestHostName"; //Hostname
                        MailClient.Send(Email);
                    }
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Failed to send the email!");
        }
    }
}