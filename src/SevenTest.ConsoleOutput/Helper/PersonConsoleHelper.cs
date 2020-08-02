using SevenTest.ConsoleOutput.Transport;
using System;
using System.Collections.Generic;
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
            try
            {
                using (var client = new HttpClient())
                {
                    var jsonBody = client.GetAsync($"{_baseUrl}/GetFullNameById?id=42").GetAwaiter().GetResult();
                    if (jsonBody.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine($"Person With Id 42 is not available");
                    }
                    else
                    {
                        var fullName = Newtonsoft.Json.JsonConvert.DeserializeObject<String>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                        Console.WriteLine($"Full Name of the Person with Id 42 :{fullName}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving Write Person FullName WithId 42");
            }
        }

        private  void WriteFirstNameForAge23()
        {
            int age = 23;
            WriteFirstNameByAge(age);
        }

        private  void WriteFirstNameByAge(int age)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Console.WriteLine();
                    Console.WriteLine($"First Name of persons age having age {age}:");

                    var jsonBody = client.GetAsync($"{_baseUrl}/GetFirstNamesByAge?age={age}").GetAwaiter().GetResult();
                    var firstNameList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<String>>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                    foreach (var personFirstName in firstNameList)
                    {
                        Console.WriteLine(personFirstName);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving All First Name with age:{age}");
            }
        }

        private  void WriteGetGenersPerAge()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    Console.WriteLine();
                    Console.WriteLine("Gender Per Age:");
                    var jsonBody = client.GetAsync($"{_baseUrl}/GetGenersPerAge").GetAwaiter().GetResult();
                    var ageWiseGendersList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AgeWiseGender>>(jsonBody.Content.ReadAsStringAsync().GetAwaiter().GetResult());

                    foreach (var ageWiseGender in ageWiseGendersList)
                    {
                        Console.WriteLine($"Age:{ageWiseGender.Age}, Females:{ageWiseGender.NumberOfFemales}, Males:{ageWiseGender.NumberOfMales}");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Unexpected Error occour while retriving GetGenersPerAge");
            }
        }
    }

}
