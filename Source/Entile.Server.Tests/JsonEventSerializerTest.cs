using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Entile.Server.Tests
{
    public class SimpleEvent
    {
        public string MyString { get; private set; }
        public int MyInt { get; private set; }

        public SimpleEvent(string myString, int myInt)
        {
            MyString = myString;
            MyInt = myInt;
        }
    }

    public class JsonEventSerializerTest
    {
        [Fact]
        public void SimpleObjectSerializesToJson()
        {
            var target = new JsonEventSerializer();
            var simpleEvent = new SimpleEvent("Simple string", 1337);

            var result = target.Serialize(simpleEvent);

            var expectedContent = "{\"MyString\":\"Simple string\",\"MyInt\":1337}";
            
            Assert.Equal("{\"SimpleEvent\":" + expectedContent + "}", result);
        }

        [Fact]
        public void SimpleObjectDeserializesFromJson()
        {
            var target = new JsonEventSerializer();
            target.RegisterKnownEventType<SimpleEvent>();

            var eventString = "{\"MyString\":\"Simple string\",\"MyInt\":1337}";

            var result = target.Deserialize("{\"SimpleEvent\":" + eventString + "}");

            Assert.IsType<SimpleEvent>(result);

            var simpleEvent = (SimpleEvent)result;
            Assert.Equal("Simple string", simpleEvent.MyString);
            Assert.Equal(1337, simpleEvent.MyInt);
        }
    }
}