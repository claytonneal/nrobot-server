using System;
using NRobot.Server.Imp.Config;
using NRobot.Server.Imp.Domain;
using NUnit.Framework;

namespace NRobot.Server.Test.DomainTests
{
    
    /// <summary>
    /// Test cases for execution keywords via keyword manager
    /// </summary>
    [TestFixture]
    class RunKeywordFixture
    {

        private KeywordManager _keywordManager;
        private const string RunKeywordType = "NRobot.Server.Test.Keywords.RunKeyword";
        private const string TestKeywordType = "NRobot.Server.Test.Keywords.TestKeywords";

        [TestFixtureSetUp]
        public void Setup()
        {
            var config = new LibraryConfig();
            config.Assembly = "NRobot.Server.Test";
            config.TypeName = RunKeywordType;
            _keywordManager = new KeywordManager();
            _keywordManager.AddLibrary(config);
            config.TypeName = TestKeywordType;
            _keywordManager.AddLibrary(config);
        }

        [Test]
        public void RunKeyword_NoArgs_VoidReturn_NullArgs()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "NoInputNoOutput", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.NoError);
            Assert.IsTrue(result.KeywordReturn == null);
        }

        [Test]
        public void RunKeyword_NoArgs_VoidReturn_EmptyArgs()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "NoInputNoOutput", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.NoError);
            Assert.IsTrue(result.KeywordReturn == null);
        }

        [Test]
        public void RunKeyword_ThrowsException()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "ThrowsException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Normal);
            Assert.IsTrue(result.KeywordError.Equals("A regular exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_ThrowsFatalException()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "ThrowsFatalException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Fatal);
            Assert.IsTrue(result.KeywordError.Equals("A fatal exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_ThrowsContinuableException()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "ThrowsContinuableException", new object[0]);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordErrorType == RunKeywordErrorTypes.Continuable);
            Assert.IsTrue(result.KeywordError.Equals("A continuable exception"));
            Assert.IsFalse(String.IsNullOrEmpty(result.KeywordTraceback));
        }

        [Test]
        public void RunKeyword_TraceOutput()
        {
            var result = _keywordManager.RunKeyword(RunKeywordType, "WritesTraceOutput", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordReturn == null);
            Assert.IsTrue(result.KeywordOutput.Contains("First line"));
            Assert.IsTrue(result.KeywordOutput.Contains("Second line"));
        }

        [Test]
        public void RunKeyword_IntReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "Int ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToInt32(result.KeywordReturn) == 1);
        }

        [Test]
        public void RunKeyword_Int64ReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "Int64 ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToInt32(result.KeywordReturn) == 1);
        }

        [Test]
        public void RunKeyword_StringReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "String ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToString(result.KeywordReturn) == "1");
        }

        [Test]
        public void RunKeyword_DoubleReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "Double ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue((Convert.ToDouble(result.KeywordReturn)).Equals(1));
        }

        [Test]
        public void RunKeyword_BooleanReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "Boolean ReturnType", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(Convert.ToBoolean(result.KeywordReturn));
        }

        [Test]
        public void RunKeyword_StringArrayReturnType()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "StringArray ReturnType", null);
            var returnval = (string[]) result.KeywordReturn;
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(returnval.Length == 3);
        }

        [Test]
        public void RunKeyword_LessThanRequiredArgs()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "String ParameterType", new object[] {"1"});
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
        }

        [Test]
        public void RunKeyword_MoreThanRequiredArgs()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "String ParameterType", new object[] { "1", "2", "3" });
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Fail);
        }

        [Test]
        public void RunKeyword_StaticMethod()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "PublicStatic Method", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
        }

        [Test]
        public void RunKeyword_KeywordDuration()
        {
            var result = _keywordManager.RunKeyword(TestKeywordType, "PublicStatic Method", null);
            Assert.IsTrue(result.KeywordStatus == RunKeywordStatus.Pass);
            Assert.IsTrue(result.KeywordDuration > 0);
        }


    }
}
