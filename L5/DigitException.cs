using System;

namespace L5Order
{
    public class DigitException : ApplicationException
    {
        public DigitException() : base()
        {

        }

        public DigitException(string zinute) : base(zinute)
        {

        }

        public DigitException(string zinute, ApplicationException vidine) : base(zinute, vidine)
        {

        }
    }
}