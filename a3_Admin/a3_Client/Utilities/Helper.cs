using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace a3_Client.Utilities
{
    public class Helper
    {
        private const string ApiBaseUri = "https://localhost:5001";

        private static string finalStr = null;

        // when u try to login
        public static void SetUidPass(string uid, string pass)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");

            string auth = uid + ":" + pass;

            // conver to string
            string auth64 = Convert.ToBase64String(encoding.GetBytes(auth));

            finalStr = "Basic " + auth64;

            //System.Diagnostics.Debug.WriteLine("fstr: " + finalStr);
        }


        public static HttpClient InitializeClient()
        {

            //System.Diagnostics.Debug.WriteLine("fstr: "+ finalStr);

            var client = new HttpClient { BaseAddress = new Uri(ApiBaseUri) };
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));


            // Hard Coding Basic authentication for encoding username = admin, password = admin with each request.
            //request.AddHeader("Authorization", "Basic YWRtaW46YWRtaW4=");
            // Ahmed
            client.DefaultRequestHeaders.Add("Authorization", finalStr);

            return client;
        }

        // check if authentication is set.
        public static bool HasAuth()
        {
            return finalStr != null;
        }

        internal static void ClearAuth()
        {
            finalStr = null;
        }
    }
}