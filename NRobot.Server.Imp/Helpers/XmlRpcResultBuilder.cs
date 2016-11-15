using System;
using CookComputing.XmlRpc;
using NRobot.Server.Imp.Domain;

namespace NRobot.Server.Imp.Helpers
{
	/// <summary>
	/// Builds an XmlRpc Result from a command result
	/// </summary>
	public class XmlRpcResultBuilder
	{
		
		/// <summary>
		/// Converts keyword result to RF XmlRpc Structure
		/// </summary>
		public static XmlRpcStruct ToXmlRpcResult(RunKeywordResult kwresult)
		{
			var result = new XmlRpcStruct();
			//add status
		    if (kwresult.KeywordStatus == RunKeywordStatus.Pass)
		    {
		        result.Add("status", "PASS");
		    }
		    else
		    {
                result.Add("status", "FAIL");
		    }
			//add error
		    if (String.IsNullOrEmpty(kwresult.KeywordError))
		    {
                result.Add("error", "");
		    }
		    else
		    {
                result.Add("error", kwresult.KeywordError);
		    }
            //add traceback
		    if (String.IsNullOrEmpty(kwresult.KeywordTraceback))
		    {
		        result.Add("traceback", "");
		    }
		    else
		    {
                result.Add("traceback", kwresult.KeywordTraceback); 
		    }
            //add output
		    if (String.IsNullOrEmpty(kwresult.KeywordOutput))
		    {
		        result.Add("output", "");
		    }
		    else
		    {
                result.Add("output", kwresult.KeywordOutput);
		    }
			//add return
			if (kwresult.KeywordReturn != null)
			{
                if (kwresult.KeywordReturn is long)
                {
                    //64bit int has to be returned as string
                    result.Add("return", kwresult.KeywordReturn.ToString());
                }
                else
                {
                    result.Add("return", kwresult.KeywordReturn);
                }
			}
			else
			{
                result.Add("return", "");
			}
			//check error type
			if (kwresult.KeywordStatus==RunKeywordStatus.Fail)
			{
				if (kwresult.KeywordErrorType==RunKeywordErrorTypes.Continuable)
				{
					//continuable error
					result.Add("continuable",true);
				}
				if (kwresult.KeywordErrorType==RunKeywordErrorTypes.Fatal)
				{
					//fatal error
					result.Add("fatal",true);
				}
			}
			return result;
		}
		
	}
}
