using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain
{
    public abstract class With<T>
        where T : Aggregate<T>, new()
    {
        private Exception _exceptionThrown;
        private IEvent[] _events;

        protected virtual IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected virtual void When(T target)
        {
            
        }
        
        protected virtual T Create()
        {
            return new T();
        }

        protected With()
        {
            var target = Create();

            target.LoadEvents(Given());
            try
            {
                When(target);
            } 
            catch(Exception ex)
            {
                _exceptionThrown = ex;
            }
            _events = target.GetUncommittedEvents().ToArray();
        }

        protected IEvent[] Events
        {
            get 
            {
                return _events;
            }
        }

        protected Exception ExceptionThrown
        {
            get { return _exceptionThrown; }
        }
        
        protected Assertions AssertEvent
        {
            get { return new Assertions(this);}
        }

        public class Assertions
        {
            private readonly With<T> _parent;

            public Assertions(With<T> parent)
            {
                _parent = parent;
            }

            public void IsType<TEvent>(int eventNumber)
            {
                Assert.IsType<TEvent>(_parent.Events[eventNumber]);
            }

            public void Contents<TEvent>(int eventNumber, Action<TEvent> checkEvent)
            {
                checkEvent((TEvent) _parent.Events[eventNumber]);
            }
        }
    }
}