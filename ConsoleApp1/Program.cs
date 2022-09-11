using MQ;
using MQ.Clients;
using MQ.Clients.Interfaces;
using MQ.Messages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Subscription iSub = new Subscription();
            //iSub.SubscribeTo<NotificiationFoo>();

            //var action = new Action<NotificiationFoo>(x => Console.WriteLine("hello"));

            ////Console.WriteLine(action.Method.GetGenericArguments().First().FullName);
            //Console.WriteLine(action.GetType().GenericTypeArguments.First());
            //action.Invoke(null);


            ISubscription iSub = MQPlatform.Subscribe()
                .SubscribeTo<NotificationFoo>(x =>
                {
                    Console.WriteLine("First IN");
                    Thread.Sleep(1000);
                    throw new Exception("PATATA");
                    Console.WriteLine("First OUT");
                })
                .SubscribeTo<NotificationFoo>(x =>
                {
                    Console.WriteLine("SECOND");
                }
                , x => x.Prop1 > 5)
                .SubscribeTo<NotificationFoo>(x => NofiticationFooAction(x));

            //iSub.UnsubscribeFrom<NotificationFoo>();
            MQPlatform.PublishAsync(new NotificationFoo()).Wait();
            iSub.Publish(new NotificationFoo());

            Console.ReadLine();
        }

        public static void NofiticationFooAction(NotificationFoo aNotif) { }

        public class NotificationFoo : INotification
        {
            public string Type => throw new NotImplementedException();

            public Guid SourceId => throw new NotImplementedException();

            public string SourceName => throw new NotImplementedException();

            public int Prop1 { get; set; }
        }
    }
}
