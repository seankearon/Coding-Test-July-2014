using System.Linq;

namespace Tests.Data
{
    public class TestRepositoryBuilder
    {
        public const string DefaultCriteria = "raven";

        public TestRepositoryBuilder()
        {
            Criteria = DefaultCriteria;
            TotalCount = 801;
        }

        public string Criteria { get; set; }

        public TestRepositoryPage Empty
        {
            get { return new TestRepositoryPage {TotalCount = TotalCount}; }
        }

        public bool Incomplete { get; set; }
        public int TotalCount { get; set; }
        public int ApiLimit { get; set; }

        public TestRepository[] Build()
        {
            return Enumerable
                .Range(1, !Incomplete ? TotalCount : TotalCount - 1)
                .Select(i =>
                    ApiLimit <= 0 || i <= ApiLimit
                        ? new TestRepository {Name = Criteria + " " + i, Description = "Description of " + Criteria + i}
                        : new ApiLimitExceeded()
                )
                .ToArray();
        }

        public TestRepositoryBuilder MatchingOn(string criteria)
        {
            Criteria = criteria;
            return this;
        }

        public TestRepositoryBuilder WithApiLimit(int limit)
        {
            ApiLimit = limit;
            return this;
        }

        public TestRepositoryBuilder WithTotalCount(int totalCount)
        {
            TotalCount = totalCount;
            return this;
        }
    }
}