using System;
using NRobot.Server.Exceptions;

namespace NRobot.Server.Imp.Domain
{
    
    public enum RunKeywordStatus
	{
		Pass,
		Fail
	}
    
    public enum RunKeywordErrorTypes
	{
		NoError,
		Normal,
		Continuable,
		Fatal
	}
    
    public class RunKeywordResult
    {

        //generic properties
        public RunKeywordStatus KeywordStatus { get; set; }
        public RunKeywordErrorTypes KeywordErrorType { get; set; }

        //output from command types "RunKeyword"
        public string KeywordOutput {get; set;}
        public Object KeywordReturn {get; set;}
        public string KeywordError {get; set;}
        public string KeywordTraceback {get; set;}
        public double KeywordDuration {get; set;}

        public override string ToString()
		{
			//build return value string
            String kwReturn;
            if (KeywordReturn == null)
            {
                kwReturn = "Null";
            }
            else
            {
                //check if array return type
                if (KeywordReturn.GetType().IsArray && KeywordReturn.GetType().GetElementType() == typeof(String))
                {
                    var retarr = (String[])KeywordReturn;
                    kwReturn = "[" + String.Join(",", retarr) + "]";
                }
                else
                {
                    kwReturn = KeywordReturn.ToString();
                }
            }
            return string.Format("[KeywordResult Status={0}, Output={1}, Return={2}, Error={3}, Traceback={4}, Duration={5}, ErrorType={6}]", KeywordStatus, KeywordOutput, kwReturn, KeywordError, KeywordTraceback, KeywordDuration, KeywordErrorType);
		}
		
		/// <summary>
		/// Captures result properties from an exception
		/// </summary>
		public void CaptureException(Exception e)
		{
			KeywordTraceback = e.StackTrace;
	﻿  ﻿  ﻿  ﻿  KeywordError = e.Message;
			KeywordStatus = RunKeywordStatus.Fail;
		    KeywordReturn = null;
			
			//check exception type
            KeywordErrorType = RunKeywordErrorTypes.Normal;
			if (e is ContinuableKeywordException)
			{
                KeywordErrorType = RunKeywordErrorTypes.Continuable;
			}
			if (e is FatalKeywordException)
			{
                KeywordErrorType = RunKeywordErrorTypes.Fatal;
			}
		}


    }
}
