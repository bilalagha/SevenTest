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
using System.Threading;

namespace SevenTest.ApiRepository
{
    public class PersonApiRepository : IPersonRepository
    {
        private readonly string _url;
        private readonly long _timeoutInSeconds;
        public PersonApiRepository(string url, long timeoutInSeconds)
        {
            _timeoutInSeconds = timeoutInSeconds;
            _url = url;
        }

        public async Task<List<Model.Person>> GetPersons()
        {
            using (var client = new HttpClient())
            {
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(_timeoutInSeconds));
                var cancellationToken = cancellationTokenSource.Token;
                try
                {
                    var httpResponse = await client.GetAsync(_url, cancellationToken);
                    var bodyContent = await httpResponse.Content.ReadAsStringAsync();
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
                catch (TaskCanceledException ex)
                {
                    var cancllationIsCausedByTimeout = ex.CancellationToken != cancellationToken;
                    if (cancllationIsCausedByTimeout)
                    {
                        throw new TimeoutException("Request Timed Out on Source", ex);
                    }
                    else
                    {
                        throw ex;
                    }
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
