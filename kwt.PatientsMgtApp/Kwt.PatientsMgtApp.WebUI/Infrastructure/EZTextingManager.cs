using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using EzTextingApiClient;
using EzTextingApiClient.Api.Messaging.Model;

namespace Kwt.PatientsMgtApp.WebUI.Infrastructure
{
    public class EZTextingManager
    {
        
        
        public static bool SendMessages(string subject, string message, string phoneNumber)
        {
            bool isSent = true;
            try{
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                EzTextingClient client = new EzTextingClient("KHO", "Kho2018!");
                var sms = new SmsMessage
                {
                    Subject = subject,
                    Message = message,
                    PhoneNumbers = new List<string> { phoneNumber },
                    // Groups = new List<string> { "group1", "group2", "group3" },
                    StampToSend = DateTime.Now,

                };
                SmsMessage response = client.MessagingApi.Send(sms);
            }
            catch (EzTextingClientException ex)
            {
                isSent = false;
            }
            catch (EzTextingApiException ex)
            {
                isSent = false;

            }
            catch (Exception ex)
            {
                isSent = false;

            }

            return isSent;
        }
        

        public static bool SendSms(string phoneNumber, string message)
        {
            bool isSuccesResponse = true;
            if (ValidatePhoneNumberFormat(phoneNumber))
            {

                ServicePointManager.Expect100Continue = true;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                WebRequest req = WebRequest.Create(string.Format("{0}/sending/messages?format=xml", "https://app.eztexting.com"));
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";

                using (Stream writeStream = req.GetRequestStream())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(string.Format("User={0}&Password={1}&PhoneNumbers={2}&Subject={3}&Message={4}",
                        "KHO", "Kho2018!", phoneNumber,"Kuwait Health Office", message));

                    writeStream.Write(bytes, 0, bytes.Length);
                }

                string respBody;
                try
                {
                    using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                        {
                            respBody = reader.ReadToEnd();
                        }

                        if (resp.StatusCode != HttpStatusCode.Created)
                            throw new Exception(string.Format("Response from SMS gateway was not good: {0} - {1}", resp.StatusCode, respBody));
                    }
                }
                catch (Exception e)
                {
                    isSuccesResponse = false;
                    //throw e;
                }
            }else
            {
                isSuccesResponse = false;
            }
            return isSuccesResponse;


        }

        public static bool ValidatePhoneNumberFormat(string phoneNumber)
        {
            return Regex.Match(phoneNumber, @"^([0-9]{10})$").Success;
        }

        public static bool ValidateMessageLength(string messsage)
        {
            return messsage.Length <= 160;
        }
    }
}