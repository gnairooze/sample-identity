using IdentitySample.SharedModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleIdentity.Utility
{
    internal class Program
    {
        static async Task Main()
        {
            using (var client = new HttpClient())
            {
                // Replace with your API URL
                client.BaseAddress = new Uri(Properties.Settings.Default.ApiBaseAddress);

                for (int i = 0; i < 1000; i++)
                {
                    var model = new RegisterBindingModel
                    {
                        Email = $"testuser{i}@example.com",
                        Password = "Password123!",
                        ConfirmPassword = "Password123!",
                        FirstName = $"Test",
                        LastName = $"User {i}"
                    };

                    var json = JsonConvert.SerializeObject(model);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("api/Account/Register", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"User {model.Email} created successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error creating user {model.Email}: {response.StatusCode} - {response.ReasonPhrase}");
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(errorContent);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
