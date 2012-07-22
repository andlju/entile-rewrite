namespace Entile.TestApp.Models
{
    public class LinkModel
    {
        public string Rel;
        public string Uri;
    }

    public class CommandModel
    {
        public string Name;
        public string Uri;
        public string Method;
        public FieldModel[] Fields;
    }

    public class FieldModel
    {
        public string Name;
    }

    public class HyperMediaModel
    {
        public LinkModel[] Links;
        public CommandModel[] Commands;
    }
}