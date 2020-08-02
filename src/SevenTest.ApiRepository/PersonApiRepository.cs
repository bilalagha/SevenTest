using System.Collections.Generic;
using System.Net.Http;
using Model = SevenTest.Core.Model;
using Newtonsoft.Json;
using SevenTest.ApiRepository.Transport;
using System.Linq;
using System.Threading.Tasks;
using SevenTest.Core;

namespace SevenTest.ApiRepository
{
    public class PersonApiRepository: IPersonRepository
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
                var jsonBody = await client.GetAsync(_url);
                var bodyContent = await jsonBody.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(bodyContent).Select(p => PersonTranportMapper(p)).ToList();
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
