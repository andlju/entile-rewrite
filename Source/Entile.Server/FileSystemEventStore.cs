﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Entile.Server.Events;

namespace Entile.Server
{
    public class FileSystemEventStore : IEventStore
    {
        private readonly string _rootPath;

        public FileSystemEventStore(string rootPath)
        {
            _rootPath = rootPath;
        }

        public IEnumerable<IEvent> GetAllEvents(Guid uniqueId)
        {
            var path = BuildPath(uniqueId);
            if (!File.Exists(path))
                return null;

            using (var stream = File.OpenRead(path))
            {
                var formatter = new BinaryFormatter();
                return (IEvent[])formatter.Deserialize(stream);
            }
        }

        public void SaveEvents(Guid uniqueId, IEnumerable<IEvent> events)
        {
            // TODO This is completely not thread-safe...
            var oldEvents = GetAllEvents(uniqueId) ?? new IEvent[0];
            var path = BuildPath(uniqueId);
            
            using(var stream = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, oldEvents.Union(events).ToArray());
            }
        }

        public IEnumerable<IEvent> GetAllEventsSince(long timestamp)
        {
            throw new NotImplementedException();
        }

        private string BuildPath(Guid uniqueId)
        {
            return Path.Combine(_rootPath, uniqueId + ".bin");
        }
    }
}