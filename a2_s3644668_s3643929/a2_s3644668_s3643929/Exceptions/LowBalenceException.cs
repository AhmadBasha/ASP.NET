using System;
using System.Runtime.Serialization;

namespace a2_s3644668_s3643929.Exceptions
{
    [Serializable]
    internal class LowBalenceException : Exception
    {


        public LowBalenceException(double LOWEST_MIN) : base("Balance can't go below $" + LOWEST_MIN)
        { }
    }
}