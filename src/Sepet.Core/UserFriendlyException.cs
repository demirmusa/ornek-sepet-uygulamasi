using System;

namespace Sepet.Core
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException()
        {
        }

        public UserFriendlyException(string msj) : base(msj)
        {
        }
    }
}