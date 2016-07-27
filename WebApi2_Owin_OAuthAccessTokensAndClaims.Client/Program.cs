using System;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Client
{
    class Program
    {

        const string baseUrl = "http://localhost:52330";


        static void Main(string[] args)
        {
            // Allow time for the web api to run
            WaitForEnter("Wait for Api to run and then press ENTER to start . . .");



        }


        private static void WaitForEnter(string title)
        {
            Console.WriteLine(title);
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
        }

    }
}
