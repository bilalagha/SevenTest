using SevenTest.Core;
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

        IPersonRepository _personRepository;

        private List<Person> _persons;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<string> GetFullNameById(int id)
        {
            var persons = await GetPerson();
            var person = persons.FirstOrDefault(p => p.Id == id);

            if (person != null)
            {
                return $"{person.First} {person.Last}";
            }
            else
            {
                throw new PersonNotFoundException($"person with specified Id: { id } does not exist");
            }
        }

        public async Task<List<string>> GetFirstNameGreaterThenAge(int age)
        {
            var persons = await GetPerson();
            return persons.FindAll(p => p.Age > 23).Select(p => p.First).ToList();
        }

        public async Task<List<AgeWiseGender>> GetGendersPerAge()
        {
            var persons = await GetPerson();
            var ageGroups = persons.GroupBy(p => p.Age);
            return ageGroups
                .OrderBy(g=>g.Key)
                .Select(g => new AgeWiseGender()
            {
                Age = g.Key,
                NumberOfMales = g.Count(p => p.Gender == MALE),
                NumberOfFemales = g.Count(p => p.Gender == FEMALE)
            })
                            .ToList();
        }

        private async Task<List<Person>> GetPerson()
        {
            if (_persons == null || !_persons.Any())
                _persons = await _personRepository.GetPersons();

            return _persons;
        }
    }
}
