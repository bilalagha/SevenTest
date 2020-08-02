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
    class PersonApiRepository: IPersonRepository
    {
        string _url = "https://f43qgubfhf.execute-api.ap-southeast-2.amazonaws.com/sampletest";

        public async Task<List<Model.Person>> GetPersons()
        {
            var client = new HttpClient();
            var jsonBody = await client.GetAsync(_url);
            var bodyContent = await jsonBody.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(bodyContent).Select(p => PersonTranportMapper(p)).ToList();
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
