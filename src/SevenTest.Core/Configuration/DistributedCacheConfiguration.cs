using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTest.Core.Configuration
{
    public class DistributedCacheConfiguration
    {
        
        public const string ConfigurationSectionKey= "DistributedCache";

        public bool DistributedCacheEnabled { get; set; }
        public long GetFullNameByIdExpirationSeconds { get; set; }
        public long GetFirstNameByAgeExpirationSeconds { get; set; }
        public long GetGenderPerAgeExpirationSeconds { get; set; }
        public long GetPersonsExpirationSeconds { get; set; }
    }
}
