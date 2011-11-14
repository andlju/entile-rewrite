using System;
using System.Collections.Generic;
using System.Linq;
using CommonDomain;
using CommonDomain.Persistence;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Entile.Server.Tests.Domain
{
    public class MockRepository : IRepository
    {
        private IEvent[] _givenEvents;
        private Func<IAggregate> _factory;

        private IEvent[] _savedEvents;

        public MockRepository(IEnumerable<IEvent> givenEvents, Func<IAggregate> factory)
        {
            _givenEvents = givenEvents.ToArray();
            _factory = factory;
        }

        public IEvent[] SavedEvents
        {
            get { return _savedEvents; }
        }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            if (_givenEvents.Length == 0)
                return (TAggregate)_factory();

            var obj = _factory();
            foreach (var e in _givenEvents)
            {
                obj.ApplyEvent(e);
            }

            return (TAggregate)obj;
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            if (_givenEvents.Length == 0)
                return (TAggregate)_factory();

            var obj = _factory();
            foreach (var e in _givenEvents)
            {
                obj.ApplyEvent(e);
            }

            return (TAggregate)obj;
        }

        public void Save(IAggregate aggregate, Guid commitId, Action<IDictionary<string, object>> updateHeaders)
        {
            var events = aggregate.GetUncommittedEvents();
            if (events != null)
                _savedEvents = events.Cast<IEvent>().ToArray();
            else 
                _savedEvents = new IEvent[0];
        }
    }

    [TestClass]
    public abstract class With<TAgg, TCmd>
        where TAgg : class, IAggregate, new()
        where TCmd : ICommand
    {
        private Exception _exceptionThrown;
        
        protected Guid UniqueId = Guid.NewGuid();

        private IEvent[] _events;

        protected MockRepository MockRepository;

        protected virtual IEnumerable<IEvent> Given()
        {
            yield break;
        }

        protected abstract IMessageHandler<TCmd> CreateHandler(IRepository repository);
 
        protected abstract TCmd When();

        [TestInitialize]
        public void Initialize()
        {
            var repository = new MockRepository(Given(), () => new TAgg());
            try
            {
                var handler = CreateHandler(repository);
                handler.Handle(When());

                _events = repository.SavedEvents;
            }
            catch (Exception ex)
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


        public class Assertions
        {
            private readonly With<TAgg, TCmd> _parent;

            public Assertions(With<TAgg, TCmd> parent)
            {
                _parent = parent;
            }

            public void IsType<TEvent>(int eventNumber)
            {
                Assert.IsNotNull(_parent.Events[eventNumber]);
                Assert.IsInstanceOfType(_parent.Events[eventNumber], typeof(TEvent));
            }

            public void Contents<TEvent>(int eventNumber, Action<TEvent> checkEvent)
            {
                checkEvent((TEvent) _parent.Events[eventNumber]);
            }
        }
    }
}