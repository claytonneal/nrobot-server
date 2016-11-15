# Introduction

NRobot Server is a .Net based Robot Framework remote server. It can be used to host and expose .Net keywords to Robot Framework, thus bringing the full capabilities of Robot Framework keyword based automation to .Net testers.

(Previously NRobot Server was known as NRobotRemote)

# Installation

NRobot Server can be downloaded from its NuGet package:

[Click Here](https://www.nuget.org/packages/NRobotServer)

*Pre-requisites:*  
- .Net framework 4.6.1

# Concepts   
   
NRobot Server receives HTTP requests from robot framework using an xml-rpc protocol. To execute keywords, NRobot Service reflects on its loaded assemblies (set via configuration) to find a matching class method. It then executes that method with the parameters from the robot framework xml-rpc call, and then passes the result (HTTP response) of the method back to robot framework.

NRobot Server is multi-threaded, there is one thread dedicated to to listening for HTTP xml-rpc requests from robot framework, and a new thread is created to execute keywords/methods. Because of this NRobot Server can be used with Pabot (see: https://github.com/mkorpela/pabot) when executing robot framework tests in parallel.

# Writing a Keyword Library

Writing a keyword library is very simple. Create a new class library project in C#/Vb.net and add a _public_ class to the project. All public _instance_ or _static_ methods of the class with:

  * Return type = void, String, Boolean, Int32, Int64, Double, String
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

When a keyword such as "DO TASK" is used in a robot framework script, NRobot Server will try to find a corresponding method with name "do\_task". i.e. Spaces are replaced with underscores.

**Documentation**

If you compile your keyword class to produce xml documentation, the xml documentation file can be passed to NRobot Server. When the keyword is executed the _summary_ xml element of the documentation for the executed method is passed back to robot framework and appears in the results file.

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

These exception types are defined in assembly NRobot.Server.Exceptions. This can be added as a reference in the keyword assembly.

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
All _Trace_ information is collected by NRobot Server when the keyword method is executed. This is passed back to robot framework to include in the results report. Example:

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
Compile to the same as the instance of NRobot Server that will be used. If your keyword library is x64, you will need x64 NRobotRemote

**Is it possible to load keyword assemblies with conflicting dependencies?**
Yes, each keyword assembly and its dependencies are loaded into a separate application domain.

**What constructor do I need in my keyword class?**
For _instance_ and _static_ methods to be considered as keywords, the keyword class needs a default parameter-less constructor. A constructor is not needed for _static_ methods to be considered as keywords.

**Can I install my keyword library into the GAC?**
Yes, NRobot Server can load keyword assemblies from the GAC by specifying the full assembly details (Name, Culture, Version, PublicKey). For example using NRobotRemoteConsole to load System.IO.File class as a keyword library:

```
NRobotRemoteConsole.exe -k mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:System.IO.File -p 8271
```


# NRobot Server

NRobote Server exe is a desktop tray application that hosts the NRobot Server Robot Framework remote server.

**Configuration**

NRobot Server is configured using the .config file _NRobot.Server.exe.config_, in this file the _port_ number of the service, and the keyword assemblies, types and xml documentation files can be defined. An example is shown below:

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>

<configSections>
<section name="NRobotServerConfiguration" type="NRobot.Server.Config.NRobotServerConfiguration, NRobot.Server.Config" />
</configSections>
<NRobotServerConfiguration>
<port number="8271" />
<assemblies>
<add name="NRobot.Server.Test.Keywords" type="NRobot.Server.Test.Keywords.PublicClass" docfile="NRobot.Server.Test.Keywords.xml" />
<add name="NRobot.Server.Test.Keywords" type="NRobot.Server.Test.Keywords.FirstClass" />
</assemblies>
</NRobotServerConfiguration>
</configuration>
```

| **Element** | **Description** |
|:------------|:----------------|
| port        | Defines the port number the service will listen on |
| assemblies  | Defines the keyword assembly, keyword type name and optionally the xml documentation file. The assembly name can be a path to the assembly or the full name of the assembly if in the GAC. |

**Starting**

To start NRobot Server double click on _NRobot.Server.exe_, the application will start and the icon will be visible in the application tray (near the system clock on Windows 7).

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

  * Start NRobot Server with your keyword library and xml documentation
  * Execute libdoc pointing it to NRobot Server host and port number, and create an xml file

**Example:**

```
java -jar robotframework-2.8.1.jar libdoc --name TestLib Remote::localhost:8271/full/type/name TestLib.xml
```

**Giving RIDE the Library Spec File**

Once the library spec xml file is created by _libdoc_ it has to be passed to RIDE. Details of this can be found [here](https://github.com/robotframework/RIDE/wiki/Keyword-Completion#wiki-using-library-specs)

# Hosting NRobot Server 

NRobot Server is designed as an asynchronous component that can be hosted in any application. Infact NRobot.Server.exe is no more than a console host for the NRobot Server component.

The following example shows how to start the NRobot service.

```
var serviceConfig = NRobotServerConfig.LoadXmlConfiguration();
var service = new NRobotService(_serviceConfig);
service.StartAsync();
Console.ReadLine();
service.Stop();
```

_NRobotService_ also has a public event called _StopRequested_ this event is raised when keyword STOP REMOTE SERVER is called. The host application can determine if to close or not by handling this event. The event is called in a background thread.



