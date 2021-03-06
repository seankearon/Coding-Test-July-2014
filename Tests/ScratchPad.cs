﻿using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GithubAPIQuery;
using Xunit;

namespace Tests
{
    /// <summary>
    ///     Just for working up ideas.
    /// </summary>
    public class ScratchPad
    {
        public static bool Get(string s)
        {
            Thread.Sleep(500);
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + s);
            return true;
        }

        [Fact]
        public void ErrorsAreSurfaced()
        {
            var tasks = new[]
            {
                Task.Run(() => Console.WriteLine("Hello!")),
                Task.Run(() =>
                {
                    Console.WriteLine("I'm bad...");
                    throw new Exception();
                }),
                Task.Run(() => Console.WriteLine("Goodbye!"))
            };

            Assert.Throws<AggregateException>(() => Task.WaitAll(tasks));
        }

        [Fact]
        public void GetFromApi()
        {
            const string url = @"https://api.github.com/search/repositories?q=raven&per_page=2&page=2";

            using (var client = new HttpClient())
            {
                // User agent header required: https://developer.github.com/v3/#user-agent-required
                client.DefaultRequestHeaders.Add("User-Agent", "Seans-Code");
                var task = client.GetAsync(url).Result;
                var body = task.Content.ReadAsStringAsync().Result;
                Console.WriteLine(body);
            }
        }

        [Fact]
        public void RepositoryQueryReturnsValues()
        {
            var query = new GithubPageSearch("raven");
            var json = query.GetPage().Result;
            foreach (var detail in RepositoryDetails.FromJson(json))
            {
                Console.WriteLine(detail.Name + " - " + detail.Description);
            }
        }

        [Fact]
        public void Tasks()
        {
            var shared = new ConcurrentDictionary<string, bool>();

            Action<string> run = s =>
            {
                var hasResults = Get(s);
                shared.AddOrUpdate(s, true, (k, v) => hasResults);
            };

            // Waiting for each one.
            run("A");
            run("B");

            // Concurrently.
            var taskC = new Task(() => run("C"));
            var taskD = new Task(() => run("D"));

            taskC.Start();
            taskD.Start();

            Task.WaitAll(taskC, taskD);

            foreach (var pair in shared)
            {
                Console.WriteLine(pair.Key + " - " + pair.Value);
            }
        }
    }
}