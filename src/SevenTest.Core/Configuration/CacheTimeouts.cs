using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTest.Core.Configuration
{
    public class CacheTimeoutsConfiguration
    {
        public const string ConfigurationName= "CacheTimeouts";
        public long GetFullNameByIdTimeout { get; set; }
        public long GetFirstNameByAgeTimeout { get; set; }
        public long GetGenderPerAgeTimeOut { get; set; }
        public long GetPersonsTimeout { get; set; }
    }
}
