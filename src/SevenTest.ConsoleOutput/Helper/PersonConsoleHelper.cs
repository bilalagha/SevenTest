using SevenTest.ConsoleOutput.Transport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SevenTest.ConsoleOutput.Helper
{
    public class PersonConsoleHelper
    {
        private  string _baseUrl = "http://localhost:57969/Person";

        public PersonConsoleHelper(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public  void Write7TestOutput()
        {
            WritePersonFullNameWithId42();
            WriteFirstNameForAge23();
            WriteGetGenersPerAge();
        }

        private  void WritePersonFullNameWithId42()
        {
            int id = 42;
            WritePersonFullNameById(id);
        }
        private void WritePersonFullNameById(int id)
        {
            string purpose = $"Person With Id {id}";
            try
            {
                
                using (var client = new HttpClient())
                {
                    var jsonBody = client.GetAsync($"{_baseUrl}/GetFullNameById?id={id}").GetAwaiter().GetResult();
                    if (jsonBody.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        handleStatusCode(jsonBody.StatusCode, purpose);
                    }
                    else
                    {
                        var fullName = Newtonsoft.Json.JsonConvert.DeserializeObject<String>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        Console.WriteLine($"Full Name of the {purpose} :{fullName}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving {purpose}");
            }
        }

       
        private  void WriteFirstNameForAge23()
        {
            int age = 23;
            WriteFirstNameByAge(age);
        }

        private  void WriteFirstNameByAge(int age)
        {
            string purpose = $"First Name of persons age having age {age}";
            try
            {
                using (var client = new HttpClient())
                {
                   

                    var jsonBody = client.GetAsync($"{_baseUrl}/GetFirstNamesByAge?age={age}").GetAwaiter().GetResult();
                    if (jsonBody.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        handleStatusCode(jsonBody.StatusCode, purpose);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{purpose}");
                        var firstNameList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                        foreach (var personFirstName in firstNameList)
                        {
                            Console.WriteLine(personFirstName);
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving {purpose}");
            }
        }

        private  void WriteGetGenersPerAge()
        {
            string purpose = $"Gender Per Age";
            try
            {
                using (var client = new HttpClient())
                {
                   
                    var jsonBody = client.GetAsync($"{_baseUrl}/GetGenersPerAge").GetAwaiter().GetResult();
                    if (jsonBody.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        handleStatusCode(jsonBody.StatusCode, purpose);
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"{purpose}:");
                        var ageWiseGendersList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AgeWiseGender>>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                        foreach (var ageWiseGender in ageWiseGendersList)
                        {
                            Console.WriteLine($"Age:{ageWiseGender.Age}, Females:{ageWiseGender.NumberOfFemales}, Males:{ageWiseGender.NumberOfMales}");
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving {purpose}");
            }
        }


        private void handleStatusCode(HttpStatusCode statusCode, string purpose)
        {
            if (statusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"{purpose} is not available");
            }
            else if (statusCode == HttpStatusCode.GatewayTimeout)
            {
                Console.WriteLine($"{purpose} can not be retrived source Timed out");
            }
            else
            {
                Console.WriteLine($"{purpose} cant be retrived because of unexpected error");
            }

        }

    }

}
