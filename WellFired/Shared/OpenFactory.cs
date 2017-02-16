namespace WellFired.Shared
{
    using System;

    public static class OpenFactory
    {
        public static IOpen CreateOpen()
        {
            throw new Exception("Platform doesn't support open commands, try to call OpenFactory.PlatformCanOpen");
        }

        public static bool PlatformCanOpen() => 
            false;
    }
}

