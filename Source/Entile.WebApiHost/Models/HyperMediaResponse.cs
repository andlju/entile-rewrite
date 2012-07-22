using System;
using System.Collections.Generic;

namespace Entile.WebApiHost.Models
{
    public class LinkDefinition
    {
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }

    public class QueryDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Uri { get; set; }
        public FieldDefinition[] Fields { get; set; }
    }

    public class CommandDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public Uri Uri { get; set; }
        public FieldDefinition[] Fields { get; set; }
    }

    public class FieldDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Optional { get; set; }
    }

    public class HyperMediaResponse
    {
        public HyperMediaResponse()
        {
        }

        public List<LinkDefinition> Links { get; set; }
        public List<QueryDefinition> Queries { get; set; }
        public List<CommandDefinition> Commands { get; set; }
    }
}