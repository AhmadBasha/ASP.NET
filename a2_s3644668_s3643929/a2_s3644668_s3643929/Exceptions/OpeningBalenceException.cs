using System;
using System.Runtime.Serialization;

namespace a2_s3644668_s3643929.Models
{
    [Serializable]
    internal class OpeningBalenceException : Exception
    {
        public OpeningBalenceException(double amt) : base("Opening balance can't be lower than A$" + amt)
        { }
    }
}