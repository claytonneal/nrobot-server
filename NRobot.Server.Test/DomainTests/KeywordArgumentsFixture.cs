using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NUnit.Framework;

namespace NRobot.Server.Test.DomainTests
{
    
    /// <summary>
    /// Tests for get keyword arguments
    /// </summary>
    [TestFixture]
    class KeywordArgumentsFixture
    {

        private KeywordManager _keywordManager;
        private const string Typename = "NRobot.Server.Test.Keywords.TestKeywords";

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = Typename;
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void GetKeywordArguments_StringArguments()
        {
            var keyword = _keywordManager.GetKeyword(Typename, "String ParameterType");
            Assert.IsTrue(keyword.ArgumentCount == 2);
            Assert.Contains("arg1", keyword.ArgumentNames);
            Assert.Contains("arg2", keyword.ArgumentNames);
        }

        [Test]
        public void GetKeywordArguments_NoArguments()
        {
            var keyword = _keywordManager.GetKeyword(Typename, "No Parameters");
            Assert.IsTrue(keyword.ArgumentCount == 0);
        }


    }
}
