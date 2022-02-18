using System;

namespace Hephaestus.Repository.Abstraction.Shared
{
    public static class SystemClock
    {
        private static DateTimeOffset? _customDate;

        public static DateTimeOffset Now
        {
            get
            {
                if (_customDate.HasValue)
                {
                    return _customDate.Value;
                }

                return DateTimeOffset.Now.ToLocalTime();
            }
        }

        public static void Set(DateTimeOffset customDate) => _customDate = customDate;

        public static void Reset() => _customDate = null;
    }
}
