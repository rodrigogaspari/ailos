using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.IntegrationTests.Fixtures
{
    [CollectionDefinition("Database")]
    public class DatabaseCollection : ICollectionFixture<DbFixture> 
    {

    }
}
