using System;

namespace ZipPay.Users.BusinessService.Exceptions
{
    public class ValidationException:Exception
    {
        public ValidationException(string message):base(message)
        {

        }
    }
}
