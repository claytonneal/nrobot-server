using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NUnit.Framework;

namespace NRobot.Server.Test.DomainTests
{
    
    /// <summary>
    /// Tests to check different keyword class access levels
    /// </summary>
    [TestFixture]
    class ClassAccessFixture
    {

        [Test]
        public void ClassAccess_PublicClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = "NRobot.Server.Test.Keywords.TestKeywords";
            kwmanager.AddLibrary(config);
            Assert.True(kwmanager.GetKeywordNamesForType("NRobot.Server.Test.Keywords.TestKeywords").Length > 0);
        }

        [Test]
        public void ClassAccess_InternalClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = "NRobot.Server.Test.Keywords.InternalClass";
            kwmanager.AddLibrary(config);
            Assert.True(kwmanager.GetKeywordNamesForType("NRobot.Server.Test.Keywords.InternalClass").Length == 0);
        }

        [Test]
        [ExpectedException(typeof(KeywordLoadingException))]
        public void ClassAccess_StaticClass()
        {
            var config = new LibraryConfig();
            var kwmanager = new KeywordManager();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = "NRobot.Server.Test.Keywords.StaticClass";
            kwmanager.AddLibrary(config);
        }


    }
}
