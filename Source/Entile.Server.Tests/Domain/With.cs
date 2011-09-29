using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain
{
    public abstract class With<TAgg, TCmd>
        where TAgg : Aggregate<TAgg>, new()
        where TCmd : ICommand
    {
        private Exception _exceptionThrown;
        
        protected Guid UniqueId = Guid.NewGuid();

        private IEvent[] _events;

        protected virtual IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected abstract IMessageHandler<TCmd> CreateHandler(IRepository<TAgg> repository);
 
        protected abstract TCmd When();

        protected With()
        {
            try
            {
                var handler = CreateHandler(new InternalTestRepository(this));
                handler.Handle(When());
            } 
            catch(Exception ex)
            {
                _exceptionThrown = ex;
            }
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

        private class InternalTestRepository : IRepository<TAgg>
        {
            private readonly With<TAgg, TCmd> _with;

            public InternalTestRepository(With<TAgg, TCmd> with)
            {
                _with = with;
            }

            public TAgg GetById(Guid uniqueId)
            {
                var givenEvents = _with.Given();
                if (givenEvents == null)
                    return null;

                var agg = new TAgg();
                agg.LoadEvents(givenEvents);

                return agg;
            }

            public void SaveChanges(TAgg aggregate)
            {
                _with._events = aggregate.GetUncommittedEvents().ToArray();
            }
        }

        public class Assertions
        {
            private readonly With<TAgg, TCmd> _parent;

            public Assertions(With<TAgg, TCmd> parent)
            {
                _parent = parent;
            }

            public void IsType<TEvent>(int eventNumber)
            {
                Assert.NotNull(_parent.Events[eventNumber]);
                Assert.IsType<TEvent>(_parent.Events[eventNumber]);
            }

            public void Contents<TEvent>(int eventNumber, Action<TEvent> checkEvent)
            {
                checkEvent((TEvent) _parent.Events[eventNumber]);
            }
        }
    }
}