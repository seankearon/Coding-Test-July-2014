using System;

namespace GithubAPIQuery
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Beginning search of Github repository API for 'raven'.");

            var searcher = new RepositorySearch();
            var results = searcher.SearchFor("raven");

            foreach (var repository in results)
            {
                Console.WriteLine("    {0} - {1}", repository.Name, repository.Description);
            }

            Console.WriteLine("Finished - found {0} repositories for the term 'raven'.", results.Length);
            Console.Read();
        }
    }
}