using SevenTest.Core.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SevenTest.Core
{
    public interface IPersonService
    {
        Task<List<string>> GetFirstNamesByAge(int age);
        Task<string> GetFullNameById(int id);
        Task<List<AgeWiseGender>> GetGendersPerAge();
    }
}
