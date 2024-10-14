# HTML Dumper

## Description
HTML Dumper is a simple console application that downloads the HTML, CSS, and JavaScript files from a specified URL and saves them in a local directory. It helps users easily extract web page resources for offline viewing or analysis.

## Features
- Downloads the main HTML file.
- Extracts and saves linked CSS files.
- Extracts and saves linked JavaScript files.
- Organizes all downloaded files into a "Dumped" directory.

## Usage
1. Run the application.
2. Enter the desired URL when prompted.
3. The application will fetch and save the resources, displaying progress in the console.
4. Once completed, the files will be available in the "Dumped" directory.

## Requirements
- .NET Core or .NET Framework
- `Leaf.xNet` library for HTTP requests (install via NuGet)
