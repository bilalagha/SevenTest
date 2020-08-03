using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTest.Core.Model
{
    public class AgeWiseGender
    {
        public int Age { get; set; }
        public int NumberOfFemales { get; set; }
        public int NumberOfMales { get; set; }

        public override bool Equals(object obj)
        {
            var objToCompare = obj as AgeWiseGender;
            return objToCompare!=null 
                && this.Age== objToCompare.Age
                && this.NumberOfFemales == objToCompare.NumberOfFemales
                && this.NumberOfMales == objToCompare.NumberOfMales;
        }

        public override int GetHashCode()
        {
            return Age ^ NumberOfFemales ^ NumberOfMales;
        }
    }
}
