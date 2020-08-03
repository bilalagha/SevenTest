using Castle.Core.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using NUnit.Framework;
using SevenTest.Business;
using SevenTest.Core;
using SevenTest.Core.Configuration;
using SevenTest.Core.Exceptions;
using SevenTest.Core.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
namespace SevenTest.PersonTest
{
    public class PersonServiceTest
    {
        List<string> _expectedString = new List<string>() { "Saleem", "Carla", "Steven", "Sanjeev" };
        Mock<IPersonRepository> _mockRepository;
        Mock<IDistributedCache> _mockDistributedCache;
        Mock<Microsoft.Extensions.Logging.ILogger<PersonService>> _mockLogger;
        PersonService _personService;
        
        [SetUp]
        public void Setup()
        {
            List<Person> personList = new List<Person>()
            {
                new Person(){ Id = 1, Age = 44,  First = "Saleem", Last = "Shahzad", Gender = "M"},
                new Person(){ Id = 2, Age = 23,  First = "Carla", Last = "Houston", Gender = "F"},
                new Person(){ Id = 3, Age = 22,  First = "Ashwaria", Last = "Roy", Gender = "F"},
                new Person(){ Id = 4, Age = 23,  First = "Steven", Last = "Rider", Gender = "M"},
                new Person(){ Id = 5, Age = 19,  First = "Monali", Last = "Thakur", Gender = "F"},
                new Person(){ Id = 6, Age = 23,  First = "Sanjeev", Last = "Kapoor", Gender = "M"}

            };

            _mockDistributedCache = new Mock<IDistributedCache>();           
            _mockLogger = new Mock<Microsoft.Extensions.Logging.ILogger<PersonService>>();
            _mockRepository = new Mock<IPersonRepository>();
            _mockRepository.Setup(repo => repo.GetPersons())
                .Returns(Task.FromResult(personList));
            _personService = new PersonService(_mockLogger.Object, _mockRepository.Object, _mockDistributedCache.Object, new DistributedCacheConfiguration());
        }

        [Test]
        [TestCase(2, "Carla Houston")]
        [TestCase(5, "Monali Thakur")]
        [TestCase(6, "Sanjeev Kapoor")]        
        public async Task GetFullNameById_Should_Match_Correct_FullName_From_Tupple(int inputAge, string outputFullName)
        {
            var personService = _personService;
            var result = await personService.GetFullNameById(inputAge);
            Assert.AreEqual(outputFullName, result);
        }

        [Test]
        public async Task GetFullNameById_Should_Throw_Exception_On_NonExisting_Id()
        {
            var personService = _personService;
            //Assert.ThrowsAsync<Exception>(async () => await personService.GetFullNameById(42));  // Not done in below fastion as this give no await warning

            try
            {
                await personService.GetFullNameById(42);
                Assert.Fail($"Expected exception of type: {typeof(Exception)}");
            }
            catch (PersonNotFoundException)
            {               
                //Nothing To Do Just Test Passed Expected Exception thrown;
            }            
        }


        [Test]        
        public async Task GetFirstNameGreaterThenAge_Should_Return_Correct_FirstName_List()
        {
            var personService = _personService;
            var result = await personService.GetFirstNamesByAge(23);
            Assert.AreEqual(new List<string>() { "Carla", "Steven", "Sanjeev" }, result);
        }


        [Test]
        public async Task GetGendersPerAge_should_return_correct_data()
        {
            var personService = _personService;
            var result = await personService.GetGendersPerAge();

            var expected = new List<AgeWiseGender>()
                {
                    new AgeWiseGender(){Age=19,NumberOfFemales=1, NumberOfMales=0 },
                    new AgeWiseGender(){Age=22,NumberOfFemales=1, NumberOfMales=0 },
                    new AgeWiseGender(){Age=23,NumberOfFemales=1,NumberOfMales=2 },
                    new AgeWiseGender(){Age=44,NumberOfFemales=0, NumberOfMales=1 }

                };
            CollectionAssert.AreEqual(expected, result);
        }        
    }
}