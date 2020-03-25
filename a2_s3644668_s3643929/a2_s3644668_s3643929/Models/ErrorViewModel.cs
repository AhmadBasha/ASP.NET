using System;

namespace a2_s3644668_s3643929.Models
{
    public class ErrorViewModel
    {

        // Variables for status code errors
        public string ErrorMessage { get; set; }

        //public string ErrorPath { get; set; }

        //public string QuerryString { get; set; }

        public int? StatusCode { get; set; }



        // Variables for Exceptions
        private readonly bool dispDetails = true;

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string ExceptionPath { get; set; }

        public string ExceptionMessage { get; set; }

        public string StackTrace { get; set; }

        public bool getDetails()
        {
            return dispDetails;
        }
    }
}
