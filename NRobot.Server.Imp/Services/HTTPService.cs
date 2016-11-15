using System;
using System.Net;
using log4net;
using System.Threading.Tasks;
using System.Threading;
using System.Net.NetworkInformation;
using System.Text;
using NRobot.Server.Imp.Domain;

namespace NRobot.Server.Imp.Services
{
	/// <summary>
	/// HTTP Listener service
	/// </summary>
	public class HttpService
	{
		
		//log4net
		private static readonly ILog Log = LogManager.GetLogger(typeof(HttpService));
		
		//properties
		private HttpListener _listener;
		private Thread _httpthread;
	    private XmlRpcService _rpcService;
	    private KeywordManager _keywordManager;
	    private int _port;

		
		/// <summary>
		/// Constructor
		/// </summary>
		public HttpService(XmlRpcService rpcService, KeywordManager keywordManager, int port)
		{
		    _rpcService = rpcService;
		    _keywordManager = keywordManager;
		    _port = port;
            //setup http listener
			_listener = new HttpListener();
			﻿_listener.Prefixes.Add(String.Format("http://*:{0}/", _port));
            _httpthread = null;
		}
		
		/// <summary>
		/// Background HTTP Listener thread
		/// </summary>
		private void DoWork_Listener()
		{
            Log.Debug(String.Format("HTTP Listener started on port {0}", _port));
            _listener.Start();
			﻿while (true)
		﻿  ﻿  ﻿{
		﻿  ﻿  ﻿	try
				{
					var reqcontext = _listener.GetContext();
					var method = reqcontext.Request.HttpMethod;
					Log.Debug(String.Format("Received Http request with method {0}",method));
                    if (method == "POST")
					{
                        Task.Factory.StartNew(() => ProcessRequest(reqcontext));
					}
                    else if (method == "DELETE")
                    {
                        Log.Debug("Closing http listener");
                        reqcontext.Response.StatusCode = 200;
                        reqcontext.Response.Close();
                        break;
                    }
					else
					{
					    WriteStatusPage(reqcontext.Response);
					}
				}
				catch(Exception e)
				{
					Log.Error(e.ToString());
				}
		﻿  ﻿  ﻿}
            Log.Debug("HTTP listener thread has exited");
		}
		
		/// <summary>
		/// Processes a HTTP request
		/// </summary>
		private void ProcessRequest(HttpListenerContext context)
		{
			Log.Debug(String.Format("Processing Http request for Url : {0}",context.Request.Url));
            try
            {
               _rpcService.ProcessRequest(context);
            }
            catch (Exception e)
            {
                Log.Error(String.Format("Error processing HTTP request : {0}", e));
                context.Response.StatusCode = 500;
                context.Response.Close();
            }
		}
		
		/// <summary>
		/// Starts the http listener and processor async
		/// </summary>
		public void StartAsync()
		{
			if (_httpthread == null)
			{
                if (IsPortInUse()) throw new Exception("Unable to start service, port already in use");
                _httpthread = new Thread(DoWork_Listener);
            	_httpthread.IsBackground = true;
                _httpthread.Start();
			}
		}
		
		/// <summary>
		/// Stop http listener
		/// </summary>
		public void Stop()
		{
			//stop listener
			if (_httpthread!=null)
			{
				//send DELETE method call
                Log.Debug("Sending HTTP request to stop");
                WebRequest stopreq = WebRequest.Create(String.Format("http://127.0.0.1:{0}/", _port));
                stopreq.Method = "DELETE";
                stopreq.GetResponse();
                _httpthread.Join(Timeout.Infinite);
			}
            _httpthread = null;
            _listener.Close(); //free's the port
		}

        /// <summary>
        /// Checks if port is available
        /// </summary>
        private Boolean IsPortInUse()
        {
            bool inUse = false;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] ipEndPoints = ipProperties.GetActiveTcpListeners();
            foreach (IPEndPoint endPoint in ipEndPoints)
            {
                if (endPoint.Port == _port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }
        
        /// <summary>
        /// Writes status page to a browser
        /// </summary>
        private void WriteStatusPage(HttpListenerResponse response)
        {
        	try
        	{
        		StringBuilder html = new StringBuilder();
        		//setup html doc
        		html.Append("<html><body><h1>NRobotRemote</h1><p><h2>Available Keywords</h2>");
        	    var types = _keywordManager.GetLoadedTypeNames();
        	    foreach (var typename in types)
        	    {
        	        var names = _keywordManager.GetKeywordNamesForType(typename);
                    //per type table
        	        html.Append("<h3>Keywords from type: " + typename + "</h3>");
                    html.Append("<table style=\"text-align: left; width: 90%;\" border=\"1\" cellpadding=\"1\" cellspacing=\"0\">");
                    html.Append("<thead><tr style=\" background-color: rgb(153, 153, 153)\">");
                    html.Append("<th>Keyword</th><th>Arguments</th><th>Description</th></tr>");
                    html.Append("</thead><tbody>");
                    //add keywords
                    foreach (string name in names)
                    {
                        var keyword = _keywordManager.GetKeyword(typename, name);
                        html.Append(String.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", name,
                                                  String.Join(",", keyword.ArgumentNames),
                                                  keyword.KeywordDocumentation));
                    }
        	        html.Append("</tbody></table>");
        	    }
                
	        	//finish html
	        	html.Append("</body></html>");
        		response.StatusCode = 200;
        		byte[] buffer = Encoding.UTF8.GetBytes(html.ToString());
				response.OutputStream.Write(buffer,0,buffer.Length);
        	}
        	catch (Exception e)
        	{
        		Log.Error(e.ToString());
        		response.StatusCode = 500;
        	}
        	response.Close();
        }
		
		
	}
}
