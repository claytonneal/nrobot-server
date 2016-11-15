using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using log4net;
using NRobot.Server.Imp.Config;

namespace NRobot.Server.Imp.Domain
{
    /// <summary>
    /// Manages all loaded keywords
    /// </summary>
    public class KeywordManager
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(KeywordManager));

        //loaded keywords by type
        private Dictionary<string, List<Keyword>> _loadedKeywords;

        public KeywordManager()
        {
            _loadedKeywords = new Dictionary<string, List<Keyword>>();
        }

        /// <summary>
        /// Add keywords from specified assembly and type
        /// </summary>
        public void AddLibrary(LibraryConfig config)
        {
            try
            {
                //check and record inputs
                if (String.IsNullOrEmpty(config.Assembly)) throw new Exception("No keyword library specified");
                if (String.IsNullOrEmpty(config.TypeName)) throw new Exception("No keyword class type specified");
                //check if already loaded type
                if (_loadedKeywords.ContainsKey(config.TypeName))
                {
                    Log.Debug(String.Format("Type {0} is already loaded", config.TypeName));
                    return;
                }
                Log.Debug(String.Format("Loading keywords from type : {0}", config.TypeName));

                //get instance
                var kwinstance = Activator.CreateInstance(config.Assembly, config.TypeName).Unwrap();
                var kwtype = kwinstance.GetType();

                //load xml documentation
                XDocument kwdocumentation = null;
                if (!String.IsNullOrEmpty(config.Documentation))
                {
                    if (File.Exists(config.Documentation))
                    {
                        kwdocumentation = XDocument.Load(config.Documentation);
                    }
                    else
                    {
                        throw new Exception(String.Format("Xml documentation file not found : {0}", config.Documentation));
                    }
                }

                //build the list of available keywords
                var methods = kwtype.GetMethods().Where(mi => mi.DeclaringType != typeof(object));
                var keywords = new List<Keyword>();
                foreach (var method in methods)
                {
                    if (HasValidSignature(method))
                    {
                        var keyword = new Keyword(kwinstance, method, kwdocumentation);
                        if (_loadedKeywords.ContainsKey(keyword.FriendlyName)) throw new Exception(String.Format("{0} keyword is duplicated", keyword.FriendlyName));
                        keywords.Add(keyword);
                    }
                }
                _loadedKeywords.Add(config.TypeName, keywords);
                Log.Debug(String.Format("Loaded keywords : {0}", String.Join(",", keywords.Select(k => k.FriendlyName).ToArray())));
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Unable to load keyword library, {0}", e));
                throw new KeywordLoadingException("Unable to load keyword library",e);
            }
        }

        
        /// <summary>
        /// Checks method signature for keyword suitability
        /// </summary>
        private Boolean HasValidSignature(MethodInfo mi)
        {
            Boolean result = false;

            //check return types (void, string, boolean, int32, int64, double, string[] )
            if (mi.ReturnParameter != null)
            {
                Type returntype = mi.ReturnParameter.ParameterType;
                if (returntype == typeof(void))
                {
                    result = true;
                }
                if (returntype == typeof(String))
                {
                    result = true;
                }
                if (returntype == typeof(Boolean))
                {
                    result = true;
                }
                if (returntype == typeof(Int32))
                {
                    result = true;
                }
                if (returntype == typeof(Int64))
                {
                    result = true;
                }
                if (returntype == typeof(Double))
                {
                    result = true;
                }
                if (returntype.IsArray && returntype.GetElementType() == typeof(String))
                {
                    result = true;
                }
            }
            //finish here if false
            if (!result) return false;

            //check method access
            if (!mi.IsPublic) result = false;
            //finish here if false
            if (!result) return false;

            //check if obsolete
            object[] methodattr = mi.GetCustomAttributes(false);
            if (methodattr.Length > 0)
            {
                for (int j = 0; j < methodattr.Length; j++)
                {
                    if (methodattr[j] is ObsoleteAttribute)
                    {
                        result = false;
                        break;
                    }
                }
            }
            //finish here if false
            if (!result) return false;

            //check argument types
            ParameterInfo[] parameters = mi.GetParameters();
            foreach (ParameterInfo par in parameters)
            {
                if (par.ParameterType != typeof(String))
                {
                    result = false;
                    break;
                }
            }
            //finish
            return result;
        }


        /// <summary>
        /// Gets a keyword based on its name, exception if not found in map
        /// </summary>
        public Keyword GetKeyword(string typename, string friendlyname)
        {
            if (!_loadedKeywords.ContainsKey(typename)) throw new Exception(String.Format("Keyword {0} not found in type {1}", friendlyname,typename));
            var keywords = _loadedKeywords[typename];
            foreach (var keyword in keywords)
            {
                if (String.Equals(keyword.FriendlyName,friendlyname,StringComparison.CurrentCultureIgnoreCase))
                {
                    return keyword;
                }
            }
            throw new Exception(String.Format(String.Format("Keyword {0} not found in type {1}", friendlyname,typename)));
        }

        /// <summary>
        /// Gets all keyword friendly names for specific type
        /// </summary>
        public string[] GetKeywordNamesForType(string typename)
        {
            if (String.IsNullOrEmpty(typename)) throw new Exception("No type name specified");
            if (!_loadedKeywords.ContainsKey(typename)) throw new Exception(String.Format("Type {0} is not loaded", typename));
            return _loadedKeywords[typename].Select(k => k.FriendlyName).ToArray();
        }

        /// <summary>
        /// Gets all loaded typenames
        /// </summary>
        public string[] GetLoadedTypeNames()
        {
            return _loadedKeywords.Keys.ToArray();
        }


        /// <summary>
        /// Executes a keyword
        /// </summary>
        public RunKeywordResult RunKeyword(string typename, string friendlyname, object[] arguments)
        {
            
            //setup
            var result = new RunKeywordResult();
            var timer = new Stopwatch();
            var tracecontent = new MemoryStream();
            var tracelistener = new TextWriterTraceListener(tracecontent);
            Trace.Listeners.Add(tracelistener);

            try
            {
                //setup
                var keyword = GetKeyword(typename, friendlyname);
                var method = keyword.KeywordMethod;
                var numargs = keyword.ArgumentCount;

                //check number of arguments
                if (arguments == null)
                {
                    if (numargs != 0) throw new Exception("Incorrect number of keyword arguments supplied");
                }
                else
                {
                    if (arguments.Length != numargs)
                    {
                        throw new Exception("Incorrect number of keyword arguments supplied");
                    }
                }

                //call method
                timer.Start();
                if (method.ReturnParameter != null && method.ReturnParameter.ParameterType == typeof (void))
                {
                    method.Invoke(keyword.ClassInstance, arguments);
                    result.KeywordReturn = null;
                }
                else
                {
                    result.KeywordReturn = method.Invoke(keyword.ClassInstance, arguments);
                }
                //success
                result.KeywordStatus = RunKeywordStatus.Pass;
                result.KeywordErrorType = RunKeywordErrorTypes.NoError;
            }
            catch (TargetInvocationException te)
            {
                result.CaptureException(te.InnerException);
            }
            catch (Exception e)
            {
                result.CaptureException(e);
            }
            finally
            {
                //record execution time
                timer.Stop();
                result.KeywordDuration = timer.Elapsed.TotalSeconds;

                //get trace output
                tracelistener.Flush();
                Trace.Listeners.Remove(tracelistener);
                result.KeywordOutput = System.Text.Encoding.Default.GetString(tracecontent.ToArray());
                
                //clean up
                tracecontent.SetLength(0);
                tracelistener.Dispose();
                tracecontent.Dispose();
            }
            //finish
            return result;
        }

#region AssemblyResolver


        /// <summary>
        /// Handles AssemblyResolve event
        /// This is needed if keyword assembly has dependencies
        /// We attempt to load assembly from same directory as the keyword assembly
        /// </summary>
        public static Assembly KeywordAssemblyResolveHandler(object source, ResolveEventArgs e)
        {
            try
            {
                Assembly result = null;
                //check if library specified includes a path
                if (e.Name.Contains("\\"))
                {
                    var libpath = Path.GetDirectoryName(e.Name);
                    if (!String.IsNullOrEmpty(libpath))
                    {
                        var asmname = new AssemblyName(e.Name);
                        var asmpath = Path.Combine(libpath, asmname.Name);
                        result = Assembly.LoadFrom(asmpath);
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }

        }

#endregion


    }
}
