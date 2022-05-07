using Soa.Sample.EntityFrameworkCore;

namespace Soa.Sample.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly SampleDbContext _context;

        public TestDataBuilder(SampleDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            //create test data here...
        }
    }
}