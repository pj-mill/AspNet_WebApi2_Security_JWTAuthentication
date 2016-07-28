using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApi2_Owin_OAuthAccessTokensAndClaims.Models.Client;

namespace WebApi2_Owin_OAuthAccessTokensAndClaims.Client
{
    /*
     * This exercise tests 3 endpointts of our api server
     * (1) Login
     * (2) Accessing orders
     * (3) Accessing refund functionality
     */

    class Program
    {
        private static string baseUrl = "http://localhost:52330/";
        private static HttpClient client;
        private static User[] users;

        static void Main(string[] args)
        {
            /*------------------------------------------------------------------
                Initialise HttpClient
            ------------------------------------------------------------------*/
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            /*------------------------------------------------------------------
                Create a list of logins already in the db
            ------------------------------------------------------------------*/
            users = new User[] {
                new User { UserName = "millime", Password = "P@ssword!" },
                new User { UserName = "macka", Password = "Pazzword!" },
                new User { UserName = "doedoe", Password = "P@ssw0rd!" }
            };

            /*------------------------------------------------------------------
                Run the excercise async
            ------------------------------------------------------------------*/
            WaitForEnter("Wait for Api to run and then press ENTER to start . . .");

            Task t1 = Task.Run(() => TryLoginAsync());
            Task t2 = t1.ContinueWith((ante) => TryOrdersAsync());
            Task t3 = t2.ContinueWith((ante) => TryRefundAsync());
            Task t4 = t3.ContinueWith((ante) => WaitForEnter("Finito"));

            t4.Wait();
        }

        private static async Task TryLoginAsync()
        {
            PrintTitle("ATTEMPTING LOGIN");
            foreach (var user in users)
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "oauth/token"))
                {
                    request.Content = new StringContent($"userName={user.UserName}&password={user.Password}&grant_type=password", Encoding.UTF8, "application/x-www-form-urlencoded");

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            var error = await response.Content.ReadAsAsync<Error>();
                            Console.WriteLine($"{user.UserName} not Logged in; Reason: {response.ReasonPhrase} ({error.error_description})");
                        }
                        else
                        {
                            user.Token = await response.Content.ReadAsAsync<Token>();
                            Console.WriteLine($"{user.UserName} Logged in");
                        }
                    };
                }
            };
        }


        private static async Task TryOrdersAsync()
        {
            PrintTitle("ATTEMPTING ORDERS ACCESS");
            foreach (var user in users)
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token.access_token);

                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/orders"))
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                Console.WriteLine($"{user.UserName} is unable to access orders; Reason: {response.ReasonPhrase}");
                            }
                            else
                            {
                                Console.WriteLine($"{user.UserName} is able to access orders");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{user.UserName} cannot access orders because they are not logged in");
                }
            };
        }

        private static async Task TryRefundAsync()
        {
            PrintTitle("ATTEMPTING REFUND");
            foreach (var user in users)
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token.access_token ?? "");

                    using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "api/orders/refund/5"))
                    {
                        using (HttpResponseMessage response = await client.SendAsync(request))
                        {
                            if (response.StatusCode != HttpStatusCode.OK)
                            {
                                Console.WriteLine($"{user.UserName} is unable make a refund; Reason: {response.ReasonPhrase}");
                            }
                            else
                            {
                                Console.WriteLine($"{user.UserName} is able to make a refund");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"{user.UserName} unable to make a refund because they are not logged in");
                }
            };
        }

        #region HELPERS
        private static void WaitForEnter(string title)
        {
            Thread.Sleep(500);
            PrintTitle(title);
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter) ;
        }

        private static void PrintTitle(string title)
        {
            Thread.Sleep(500);
            string divider = new String('=', 70);
            Console.WriteLine();
            Console.WriteLine(divider);
            Console.WriteLine(title);
            Console.WriteLine(divider);
        }
        #endregion
    }
}
