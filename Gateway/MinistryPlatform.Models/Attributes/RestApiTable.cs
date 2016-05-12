using System;

namespace MinistryPlatform.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RestApiTable : System.Attribute
    {
        public string Name { get; set; }
    }
}
