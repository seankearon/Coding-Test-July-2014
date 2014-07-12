using System.Linq;

namespace Tests.Data
{
    public class TestRepositoryBuilder
    {
        public TestRepositoryBuilder()
        {
            Criteria = "raven";
            TotalCount = 801;
        }

        public string Criteria { get; set; }

        public TestRepositoryPage Empty
        {
            get { return new TestRepositoryPage {TotalCount = TotalCount}; }
        }

        public bool Incomplete { get; set; }
        public int TotalCount { get; set; }

        public TestRepository[] Build()
        {
            return Enumerable
                .Range(1, !Incomplete ? TotalCount : TotalCount - 1)
                .Select(i => new TestRepository {Name = Criteria + " " + i, Description = "Description of " + Criteria + i})
                .ToArray();
        }

        public void MatchingOn(string criteria)
        {
            Criteria = criteria;
        }

        public void WithTotalCount(int totalCount)
        {
            TotalCount = totalCount;
        }
    }
}