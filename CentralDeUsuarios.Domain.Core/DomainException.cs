using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralDeUsuarios.Domain.Core
{
    public class DomainException : Exception
    {
        public DomainException(string errorMessage)
            : base(errorMessage)
        {

        }

        public static void When(bool hasError, string errorMessage)
        {
            if (hasError)
                throw new DomainException(errorMessage);
         
        }
    }
}
