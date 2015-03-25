using NoSqlStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var msg = StorageQueueRepository.Get();
                if(msg != null)
                {
                    var guid = msg.AsString;
                    var email = StorageTableRepository.GetEmail("ProductEmail", guid);
                    Console.WriteLine(string.Format("To: {0}  Subject: {1}  Body: {2}", email.To, email.Subject, email.Body));
                    email.Delivered = true;
                    StorageTableRepository.InsertOrUpdateEmail(email, "ProductEmail", guid);
                    StorageQueueRepository.Delete(msg);
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}
