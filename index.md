# Introduction

NRobotRemote is a .Net based Robot Framework remote server. It can be used to host and expose .Net keywords to Robot Framework, thus bringing the full capabilities of Robot Framework keyword based automation to .Net.

# NuGet Package

[Click Here](https://www.nuget.org/packages/NRobotRemote/)

# Release Notes

| **Version** | **Date** | **Issues** |
|:------------|:---------|:-----------|
| 1.0.0       | 11-Jul-2013 | First Release |
| 1.1.0       | 16-Jul-2013 | issue 4 |
| 1.2.0       | 22-Jul-2013 | issue 8, issue 12, issue 13 |
| 1.2.1       | 23-Jul-2013 | issue 14, issue 16, issue 17) |
| 1.2.2       | 24-Jul-2013 | issue 19, issue 20], issue 21 |
| 1.2.3       | 07-Aug-2013 | issue 22, issue 23, issue 25, issue 26 |
| 1.2.4       | 12-Sep-2013 | issue 28 |
| 1.2.5       | 16-Feb-2014 | issue 30 |
| 1.2.6       | 18-Aug-2015 | upgrade to .net 4.5 |

# How To Write a Keyword Library

Writing a keyword library is very simple.Create a new class library project in C#/Vb.net and add a _public_ class to the project. All public _instance_ or _static_ methods of the class with:

  * Return type = void, String, Boolean, Int32, Int64, Double, String[[.md](.md)]
  * No parameters
  * All parameters of type String

Will be considered as keywords.

The return type of String[] corresponds to robot framework list variables, the other return types are robot framework scalar variables.

**Example**

The following class exposes three keyword methods

```
public class MyKeywordClass
{

	public void DoAction()
	{
		...
	}

	public String DoOperation(String arg1, String arg2)
	{
		...
	}

	public void Do_Task(string arg1)
	{
		...
	}

}
```

The following keywords can be used by robot framework

  * DOACTION
  * DOOPERATION
  * DO TASK

**General Notes**

  * It is not possible to overload the same keyword method. For a method to be considered a keyword it must have only one implementation.

**Keyword Names**

When a keyword such as "DO TASK" is used in a robot framework script, NRobotRemote will try to find a corresponding method with name "do\_task". i.e. Spaces are replaced with underscores.

**Documentation**

If you compile your keyword class to produce xml documentation, the xml documentation file can be passed to NRobotRemote. When the keyword is executed the _summary_ xml element of the documentation for the executed method is passed back to robot framework and appears in the results file.

It is also possible to use the _libdoc_ tool from Robot Framework to document all the keywords hosted in NRobotRemote. See Robot Framework documentation on libdoc command line parameters.

**Deprecating a Keyword**

A keyword method can be marked as Deprecated or Obsolete by using the Obsolete attribute on the keyword method, as shown below:

```
public class MyKeywordClass
{

	[Obsolete]
	public void DoAction()
	{
		...
	}
}
```

Obsolete methods are not considered as keywords.

**Continuable and Fatal Errors**

AS of Robot Framework 2.8, a keyword can return continuable and fatal errors. To return one of these error types they keyword must throw one of the following exception types:

  * ContinuableKeywordException
  * FatalKeywordException

These exception types are defined in assembly NRobotRemote.Exceptions. This can be added as a reference in the keyword assembly.

If a keyword throws any other type of exception it is treated as a normal error.

An example of a continuable keyword is shown below:

```
public class MyKeywordClass
{
	public void DoAction()
	{
		try
		{
			...
		}
		catch (Exception e)
		{
			throw new ContinuableKeywordException(e)
		}
	}
}
```

**FAQ**

**Is it possible for the keyword class to maintain state?**<br />
Yes, only one instance of the keyword class is created for the duration of the service.

**How can my keyword give log information back to robot framework to include in the results file?**<br />
All _Trace_ information is collected by NRobotRemote when the keyword method is executed. This is passed back to robot framework to include in the results report. Example:

```
public class MyKeywordClass
{
	public String delete_file(String arg1)
	{
		File.Delete(arg1);
		Trace.WriteLine(String.Format("{0} was deleted",arg1));
	}
}
```

**How is a keyword considered as a FAIL**?
A keyword method is considered as FAIL (and will be shown in the robot framework report as FAIL), if the method raises an _exception_. Otherwise the keyword will be considered as PASS.

**Can a keyword method return null?**
Yes, if a methods return parameter type is _string_ the method can return null. Nullable int, bool, double arent supported

**Should I compile as x86, x64, or AnyCPU?**
Compile to the same as the instance of NRobotRemote that will be used. If your keyword library is x64, you will need x64 NRobotRemote

**Is it possible to load keyword assemblies with conflicting dependencies?**
Yes, each keyword assembly and its dependencies are loaded into a separate application domain.

**What constructor do I need in my keyword class?**
For _instance_ and _static_ methods to be considered as keywords, the keyword class needs a default parameter-less constructor. A constructor is not needed for _static_ methods to be considered as keywords.

**Can I install my keyword library into the GAC?**
Yes, NRobotRemote can load keyword assemblies from the GAC by specifying the full assembly details (Name, Culture, Version, PublicKey). For example using NRobotRemoteConsole to load System.IO.File class as a keyword library:

```
NRobotRemoteConsole.exe -k mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:System.IO.File -p 8271
```

# NRobotRemote Console

**Config File**

The config file _NRobotRemoteConsole.exe.config_ is used for _log4net_ configuration. By default log information is written to the console and also to a _rolling_ log file called _NRobotRemoteConsole.log_. Additional appenders can be added, see Log4Net documentation.

**Command Line**

The following command line arguments are used:

| -p | the port number |
|:---|:----------------|
| -k | specify the libraries, types, doc files to load. Multiple items in format _assembly:type:docfile_ where docfile is optional |

**NOTE** The _type_ name should include the namespace.

**Hosting Multiple Keyword Types**

NRobotRemoteConsole can host multiple keyword types. These are exposed to robot framework XmlRpc at url:

http://(host):(port)/(fulltypename)

Where _fulltypename_ is the full type name of the keyword class (including namespace) with "." replaced by "/"

**Example:**
The following command line loads two keyword types from two different libraries:

```
NRobotRemoteConsole.exe -p 8271 -k UILibrary.dll:UILibrary.UIKeywords OracleLibrary.dll:OracleLibrary.OracleKeywords
```

The keywords are then exposed to robot framework at url's:

  * http://localhost:8271/UILibrary/UIKeywords
  * http://localhost:8271/OracleLibrary/OracleKeywords

**Starting and Stopping**

To start NRobotRemoteConsole.exe supply the above command line parameters. <br />
To stop NRobotRemoteConsole.exe:

  * Press Ctrl+C in the window
  * Close the window
  * Execute keyword "STOP REMOTE SERVER"
  * Call XmlRpc method stop\_remote\_server

**Monitoring**

A remote _NRobotRemoteConsole_ can be checked to see if up and running by pointing a web browser to http://host:port. This will display information on the NRobotRemote instance, and the available keywords. This is also useful for example if NRobotRemote is started via Ant, the http condition can be used to wait for it to start.

# NRobotRemotTray

NRoboteRemoteTray is a desktop tray application that hosts the NRobotRemote Robot Framework remote server. It can be used as an alternative to NRobotRemoteConsole.

**Configuration**

NRobotRemotTray is configured using the .config file _NRobotRemoteTray.exe.config_, in this file the _port_ number of the service, and the keyword assemblies, types and xml documentation files can be defined. An example is shown below:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>

<configSections>
<section name="NRobotRemoteConfiguration" type="NRobotRemote.Config.NRobotRemoteConfiguration, NRobotRemote.Config" />
</configSections>
<NRobotRemoteConfiguration>
<port number="8271" />
<assemblies>
<add name="NRobotRemote.Test.Keywords" type="NRobotRemote.Test.Keywords.PublicClass" docfile="NRobotRemote.Test.Keywords.xml" />
<add name="NRobotRemote.Test.Keywords" type="NRobotRemote.Test.Keywords.FirstClass" />
</assemblies>
</NRobotRemoteConfiguration>
</configuration>
```

| **Element** | **Description** |
|:------------|:----------------|
| port        | Defines the port number the service will listen on |
| assemblies  | Defines the keyword assembly, keyword type name and optionally the xml documentation file. The assembly name can be a path to the assembly or the full name of the assembly if in the GAC. |

**Starting**

To start NRobotRemoteTray double click on _NRobotRemoteTray.exe_, the application will start and the icon will be visible in the application tray (near the system clock on Windows 7).

**Context Menu**

Right clicking on the tray icon gives the following options:

| **Option** | **Description** |
|:-----------|:----------------|
| Keywords   | Opens the browser to the NRobotRemote service address, which lists all the keywords available. |
| About      | Displays the About form |
| Exit       | Exits the application |

# Using RIDE

RIDE (Robot IDE) is a GUI for creating robot framework test cases. One of its main features is _keyword completion_, allowing you to design tests without referencing lots of documentation. Further information on RIDE is available [here](https://github.com/robotframework/RIDE)

RIDE cannot directly interrogate a .Net keyword library to find what keywords are available in it. However a "Library Spec" file can be given to RIDE.

**Generating a Library Spec File**

A library spec file is created using the robot framework libdoc tool. To use this tool with NRobotRemote:

  * Start NRobotRemote with your keyword library and xml documentation
  * Execute libdoc pointing it to NRobotRemote host and port number, and create an xml file

**Example:**

```
java -jar robotframework-2.8.1.jar libdoc --name TestLib Remote::localhost:8271/full/type/name TestLib.xml
```

**Giving RIDE the Library Spec File**

Once the library spec xml file is created by _libdoc_ it has to be passed to RIDE. Details of this can be found [here](https://github.com/robotframework/RIDE/wiki/Keyword-Completion#wiki-using-library-specs)

# Hosting NRobotRemote

NRobotRemote is designed as an asynchronous component that can be hosted in any application. Infact NRobotRemoteConsole.exe is no more than a console host for the NRobotRemote component.

The following example shows how to start the NRobotRemote service.

```
RemoteService srv = new RemoteService(options.library,options.type,options.port,options.docfile);
srv.StartAsync();
Console.ReadLine();
srv.Stop();
```

_RemoteService_ also has a public event called _StopRequested_ this event is raised when keyword STOP REMOTE SERVER is called. The host application can determine if to close or not by handling this event. The event is called in a background thread.

**NOTES**

  * The docfile parameter is optional
  * Two background threads are started when _StartAsync_ is called, one thread to listen for HTTP requests, and the other to process HTTP requests.


