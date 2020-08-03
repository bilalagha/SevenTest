using SevenTest.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SevenTest.Core
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetPersons();
    }
}
