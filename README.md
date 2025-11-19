# FileMonitoringService - Windows Service

## Overview
A Windows Service that monitors a folder for new files, renames them using a GUID, moves them to a destination folder, deletes the originals, and logs all operations.

## Features
- Folder monitoring for new files.
- File renaming with GUID for uniqueness.
- Move files to destination folder and delete originals.
- Logging of all operations.
- Dynamic configuration via `App.config`.
- **Important:** Source and destination folders must be created manually before running the service, as the service does not create them automatically.

## Installation
1. Build the project to generate `FileMonitoringService.exe`.
2. Ensure that the source and destination folders exist.
3. Open Command Prompt as Administrator.
4. Install the service:

