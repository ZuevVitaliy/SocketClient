using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PubSub
{
    public class Publisher
    {
        private readonly List<Subscriber> _subscribers = new List<Subscriber>();

        public void Subscribe(Subscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void Publish(string textParameter)
        {
            foreach (var subscriber in _subscribers)
            {
                subscriber.TextProperty = textParameter;
            }
        }

        public void Unsubscribe(Subscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }
    }
}
