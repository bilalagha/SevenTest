using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Xsl;

namespace SevenTest.Core
{
    public class PersonNotFoundException: Exception
    {
        public PersonNotFoundException(string message):base(message)
        {

        }
    }
}
