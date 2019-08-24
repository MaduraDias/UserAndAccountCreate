using System;

namespace ZipPay.Users.BusinessService.Exceptions
{
    public class BusinessValidationException:Exception
    {
        public BusinessValidationException(string message):base(message)
        {

        }
    }
}
