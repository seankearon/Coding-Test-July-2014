using System.Threading.Tasks;

namespace GithubAPIQuery
{
    public interface IPageSearch
    {
        /// <summary>
        ///     Gets a JSON search result for a given page.
        /// </summary>
        Task<string> GetPage(int page = 1);
    }
}