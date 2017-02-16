namespace WellFired.Shared
{
    using System;
    using System.IO;

    public class IOHelper : IIOHelper
    {
        public bool FileExists(string file) => 
            File.Exists(file);
    }
}

