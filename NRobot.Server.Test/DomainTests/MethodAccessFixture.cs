using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NUnit.Framework;

namespace NRobot.Server.Test.DomainTests
{
    /// <summary>
    /// Tests that check method access levels to determine if they are keywords
    /// </summary>
    [TestFixture]
    class MethodAccessFixture
    {

        private KeywordManager _keywordManager;
        private const string Typename = "NRobot.Server.Test.Keywords.TestKeywords";

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            _keywordManager = new KeywordManager();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = Typename;
            _keywordManager.AddLibrary(config);
        }

      
        [Test]
        public void Public_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.Contains("PUBLIC METHOD", result);
        }

        [Test]
        public void PublicStatic_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.Contains("PUBLICSTATIC METHOD", result);
        }

        [Test]
        public void Private_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("PRIVATE METHOD"));
        }

        [Test]
        public void Internal_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("INTERNAL METHOD"));
        }

        [Test]
        public void Protected_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("PROTECTED METHOD"));
        }

        [Test]
        public void Obsolete_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("OBSOLETE METHOD"));
        }

        [Test]
        public void PrivateStatic_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("PRIVATESTATIC METHOD"));
        }

        [Test]
        public void InternalStatic_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("INTERNALSTATIC METHOD"));
        }

        [Test]
        public void ProtectedStatic_Method()
        {
            var result = _keywordManager.GetKeywordNamesForType(Typename);
            Assert.That(result, Has.No.Member("PROTECTEDSTATIC METHOD"));
        }


    }
}
