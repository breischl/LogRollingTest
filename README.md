# LogRollingTest
A small project to demonstrate a bug in NLog when using date/time in logfile name and the keepFileOpen="true" option.
The problem is that when a new logfile is created, the old one is never closed. This will continue for the life of the process.
For long-running server processes this can interfere with maintenance tasks, such as logfile cleanup. 

# How to Run This Project
1. Open the solution in Visual Studio
2. Run the project
3. Let it run for at least one minute, so it has time to roll over the logfiles
4. Open a Powershell window in the execution directory (eg, [PROJECT_DIR]/bin/Debug)
5. Run the CheckForOpenFiles.ps1 script. It may prompt for permissions, because it's running the SysInternals handle.exe file. Feel free to grab your own copy if you don't want to trust mine. [Download at technet.microsoft.com](https://technet.microsoft.com/en-us/sysinternals/bb896655.aspx)
6. Note that there are open file handles for all the logfiles, even those no longer in use.
