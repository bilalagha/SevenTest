using System.Collections.Generic;
using System.Net.Http;
using Model = SevenTest.Core.Model;
using Newtonsoft.Json;
using SevenTest.ApiRepository.Transport;
using System.Linq;
using System.Threading.Tasks;
using SevenTest.Core;
using SevenTest.Core.Exceptions;
using System;

namespace SevenTest.ApiRepository
{
    public class PersonApiRepository : IPersonRepository
    {
        private readonly string _url;
        public PersonApiRepository(string url)
        {
            _url = url;
        }

        public async Task<List<Model.Person>> GetPersons()
        {
            using (var client = new HttpClient())
            {
                var rawBody = await client.GetAsync(_url);
                var bodyContent = await rawBody.Content.ReadAsStringAsync();
                try
                {
                    var personList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(bodyContent);
                    return personList.Select(p => PersonTranportMapper(p)).ToList();

                }
                catch (Newtonsoft.Json.JsonReaderException jsonEx)
                {
                    throw new InvalidSourceDataFormat("Error Whie Paring Json Data for Person Api Repository Get Persons Method", "Api", _url, jsonEx);
                }
            }
        }

        public Model.Person PersonTranportMapper(Person transportPerson)
        {
            return new Model.Person()
            {
                Id = transportPerson.Id,
                Age = transportPerson.Age,
                First = transportPerson.First,
                Last = transportPerson.Last,
                Gender = transportPerson.Gender
            };
        }
    }
}
