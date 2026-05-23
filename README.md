# FileMonitoringService - Windows Service

A Windows Service built with C# that monitors a folder for new files, renames them using GUIDs, moves them to a destination folder, deletes the original files, and logs all operations.

## Features

- Real-time folder monitoring
- Automatic file renaming using GUIDs
- File transfer to destination folder
- Automatic deletion of source files
- Logging system for tracking operations
- Dynamic configuration using App.config

## Technologies Used

- C#
- .NET Framework
- Windows Services
- File System Monitoring
- App.config

## Important Notes

- Source and destination folders must be created manually before running the service.
- The service does not create folders automatically.

## Installation

1. Build the project to generate `FileMonitoringService.exe`
2. Create the source and destination folders
3. Open Command Prompt as Administrator
4. Install the service
