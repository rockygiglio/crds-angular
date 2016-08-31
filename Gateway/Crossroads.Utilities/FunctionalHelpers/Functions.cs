using System;

namespace Crossroads.Utilities.FunctionalHelpers
{
    public static class Functions
    {
        public static int IntegerReturnValue(Func<int> func)
        {
            return func();
        }

    }
}
