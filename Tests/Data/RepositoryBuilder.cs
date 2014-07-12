using System.Linq;

namespace Tests.Data
{
    public class RepositoryBuilder
    {
        public const string DefaultCriteria = "raven";

        public RepositoryBuilder()
        {
            Criteria = DefaultCriteria;
            TotalCount = 801;
        }

        public string Criteria { get; set; }

        public RepositoryPage Empty
        {
            get { return new RepositoryPage {TotalCount = TotalCount}; }
        }

        public bool Incomplete { get; set; }
        public int TotalCount { get; set; }
        public int ApiLimit { get; set; }

        public Repository[] Build()
        {
            return Enumerable
                .Range(1, !Incomplete ? TotalCount : TotalCount - 1)
                .Select(i =>
                    ApiLimit <= 0 || i <= ApiLimit
                        ? new Repository {Name = Criteria + " " + i, Description = "Description of " + Criteria + i}
                        : new ApiLimitExceeded()
                )
                .ToArray();
        }

        public RepositoryBuilder MatchingOn(string criteria)
        {
            Criteria = criteria;
            return this;
        }

        public RepositoryBuilder WithApiLimit(int limit)
        {
            ApiLimit = limit;
            return this;
        }

        public RepositoryBuilder WithTotalCount(int totalCount)
        {
            TotalCount = totalCount;
            return this;
        }
    }
}