﻿using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SevenTest.Core;
using SevenTest.Core.Configuration;
using SevenTest.Core.Exceptions;
using SevenTest.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenTest.Business
{
    public class PersonService : IPersonService
    {

        public const string MALE = "M";
        public const string FEMALE = "F";

        private IPersonRepository _personRepository;
        private IDistributedCache _cacheClient;
        private List<Person> _persons;
        private ILogger<PersonService> _logger;
        private DistributedCacheConfiguration _distributedCacheConfig;

        public PersonService(ILogger<PersonService> logger, 
            IPersonRepository personRepository, 
            IDistributedCache cacheClient,
            DistributedCacheConfiguration distributedCacheConfig

            )
        {
            _personRepository = personRepository;
            _logger = logger;
            _cacheClient = cacheClient;
            _distributedCacheConfig = distributedCacheConfig;
        }

        public async Task<string> GetFullNameById(int id)
        {
            _logger.LogInformation("Called PersonService.GetFullNameById for Id {id}");
            var cacheKey = $"GetFullNameById-Id:{id}";
            string cachedData = null;
            // if(_distributedCacheConfig.DistributedCacheEnabled)

            if (_distributedCacheConfig.DistributedCacheEnabled)
            {
                cachedData = await AsyncJsonCacheHelper.GetCache<string>(_cacheClient, cacheKey);
            }
            if (string.IsNullOrEmpty(cachedData))
            {

                var persons = await GetPersons();
                var person = persons.FirstOrDefault(p => p.Id == id);

                if (person != null)
                {
                    var personFullName = $"{person.First} {person.Last}";
                    if (_distributedCacheConfig.DistributedCacheEnabled)
                    {
                        await AsyncJsonCacheHelper.SaveCache<string>(_cacheClient, cacheKey, _distributedCacheConfig.GetFullNameByIdExpirationSeconds, personFullName);
                    }
                    return personFullName;
                }
                else
                {
                    throw new PersonNotFoundException($"person with specified Id: { id } does not exist",id);
                }
            }
            else
            {
                return cachedData;
            }
        }

        public async Task<List<string>> GetFirstNamesByAge(int age)
        {
            var cacheKey = $"GetFirstNamesByAge-age:{age}";
            List<string> cachedData=null;
            if (_distributedCacheConfig.DistributedCacheEnabled)
            {
                cachedData = await AsyncJsonCacheHelper.GetCache<List<string>>(_cacheClient, cacheKey);
            }
            if (cachedData == null)
            {
                _logger.LogInformation("Called PersonService.GetFirstNamesByAge for age {age}");
                var persons = await GetPersons();                
                var firstNames= persons.FindAll(p => p.Age == age).Select(p => p.First).ToList();
                if (_distributedCacheConfig.DistributedCacheEnabled)
                {
                    await AsyncJsonCacheHelper.SaveCache<List<string>>(_cacheClient, cacheKey, _distributedCacheConfig.GetFirstNameByAgeExpirationSeconds, firstNames);
                }
                return firstNames;

            }
            else
            {
                return cachedData;
            }
        }

        public async Task<List<AgeWiseGender>> GetGendersPerAge()
        {
            _logger.LogInformation("Called PersonService.GetGendersPerAge");
            var cacheKey = $"GetGendersPerAge";
            List<AgeWiseGender> cachedData=null;
            if (_distributedCacheConfig.DistributedCacheEnabled)
            {
                cachedData = await AsyncJsonCacheHelper.GetCache<List<AgeWiseGender>>(_cacheClient, cacheKey);
            }
            if (cachedData == null)
            {
                var persons = await GetPersons();
                var ageGroups = persons.GroupBy(p => p.Age);
                var ageWiseGender = ageGroups
                    .OrderBy(g => g.Key)
                    .Select(g => new AgeWiseGender()
                    {
                        Age = g.Key,
                        NumberOfMales = g.Count(p => p.Gender == MALE),
                        NumberOfFemales = g.Count(p => p.Gender == FEMALE)
                    })
                     .ToList();
                if (_distributedCacheConfig.DistributedCacheEnabled)
                {
                    await AsyncJsonCacheHelper.SaveCache<List<AgeWiseGender>>(_cacheClient, cacheKey, _distributedCacheConfig.GetGenderPerAgeExpirationSeconds, ageWiseGender);
                }
                return ageWiseGender;
            }
            else
            {
                return cachedData;
            }
        }

        private async Task<List<Person>> GetPersons()
        {
            _logger.LogInformation("Called PersonService.GetPerson");
            var cacheKey = $"GetPersons";
            var cachedData = await AsyncJsonCacheHelper.GetCache<List<Person>>(_cacheClient, cacheKey);
            if (cachedData == null)
            {

                if (_persons == null || !_persons.Any())
                _persons = await _personRepository.GetPersons();
                await AsyncJsonCacheHelper.SaveCache<List<Person>>(_cacheClient, cacheKey, _distributedCacheConfig.GetPersonsExpirationSeconds, _persons);
                return _persons;
            }
            else
            {
                return cachedData;
            }
        }
    }
}
