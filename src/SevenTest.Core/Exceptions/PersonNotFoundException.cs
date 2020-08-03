using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;

namespace SevenTest.Core.Exceptions
{
    public class PersonNotFoundException: Exception
    {
        public int PersonId { get; set; }
        public PersonNotFoundException(string message, int personId):base(message)
        {
            PersonId = personId;
        }
    }
}
