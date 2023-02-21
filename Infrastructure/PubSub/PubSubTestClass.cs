using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PubSub
{
    public static class PubSubTestClass
    {
        public static void TestMethod(Action<string> showMessagesAction)
        {
            var publisher = new Publisher();

            var subscriber1 = new Subscriber();
            var subscriber2 = new Subscriber();
            var subscriber3 = new Subscriber();
            var subscriber4 = new Subscriber();

            var subs = new[] { subscriber1, subscriber2, subscriber3, subscriber4 };

            publisher.Subscribe(subscriber1);
            publisher.Subscribe(subscriber2);
            publisher.Subscribe(subscriber3);
            publisher.Subscribe(subscriber4);

            publisher.Publish("Текст1");

            foreach (var sub in subs)
            {
                showMessagesAction(sub.TextProperty);
            }

            publisher.Unsubscribe(subscriber2);

            publisher.Publish("Текст2");

            foreach (var sub in subs)
            {
                showMessagesAction(sub.TextProperty);
            }
        }
    }
}
