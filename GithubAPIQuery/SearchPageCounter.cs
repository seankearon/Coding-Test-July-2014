namespace GithubAPIQuery
{
    /// <summary>
    /// Holder for the number of the next page to be retreived.
    /// </summary>
    public class SearchPageCounter
    {
        private readonly object nextSearchPageLock = new object();
        private int nextSearchPage = 1;

        /// <summary>
        /// Gets the number of the next page to be searched.
        /// </summary>
        public int NextSearchPageNumber()
        {
            lock (nextSearchPageLock)
            {
                return nextSearchPage++;
            }
        }
    }
}