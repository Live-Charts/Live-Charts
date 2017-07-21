using System;

namespace Wpf.Home
{
    public class SampleVm
    {
        private static int _idCount;

        public SampleVm()
        {
            Id = _idCount++;
            Tags = string.Empty;
        }

        public SampleVm(string title, Type content, string tags = "")
        {
            Id = _idCount++;
            Title = title;
            Content = content;
            Tags = tags;
        }

        public int Id { get; private set; }
        public string Title { get; set; }
        public Type Content { get; set; }
        public string Tags { get; set; }
    }
}