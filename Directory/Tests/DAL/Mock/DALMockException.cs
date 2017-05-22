using System;

namespace Tests.DAL
{
    public class DALMockException : Exception
    {
        public DALMockException(string message) : base(message)
        {
        }
    }
}
