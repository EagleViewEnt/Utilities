//-----------------------------------------------------------------------
// <copyright 
//	   Author="Brian Dick"
//     Company="Eagle View Enterprises LLC"
//     Copyright="(c) Eagle View Enterprises LLC. All rights reserved."
//     Email="support@eagleviewent.com"
//     Website="www.eagleviewent.com"
// >
//	   <Disclaimer>
//			THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
// 			TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// 			THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
// 			CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// 			DEALINGS IN THE SOFTWARE.
// 		</Disclaimer>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Text.RegularExpressions;

using EagleViewEnt.Utilities.Core.Extensions.Logger;
using EagleViewEnt.Utilities.Core.Types.FilePath.Enums;

using Serilog;

namespace EagleViewEnt.Utilities.Core.Types.FilePath.Extensions;

/// <summary>
///  Extension methods for <see cref="EveFilePath" /> providing utilities for file and directory operations, path
///  manipulation, and timestamped file naming.
/// </summary>
public static class EveFilePathExtensions
{

    /// <summary>
    ///  Wildcard filter representing all files.
    /// </summary>
    const string STR_ALL_FILES = "*.*";

    /// <summary>
    ///  Regular expression used to find ISO-8601-like date/time stamps placed at the start or end of file names,
    ///  wrapped in brackets, e.g. <c>[2024-10-31T...]_</c> or <c>_[2024-10-31...]</c>.
    /// </summary>
    public static string DtsRegEx
        => @"\[[0-9]{4}-[0-9]{2}-[0-9]{1,2}T[^\]]*\]_|_\[[0-9]{4}-[0-9]{2}-[0-9]{1,2}[^\]]*\]";

    /// <summary>
    ///  Changes the file extension of the file path string to a new extension.
    /// </summary>
    /// <param name="filePath">The original file path.</param>
    /// <param name="newExtension">New extension including the dot.</param>
    /// <returns>The updated file path with the new extension, or the original path if it cannot be changed.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="newExtension" /> is null, empty, or does not start with a dot.</exception>
    public static EveFilePath ChangeFileExtension( this EveFilePath filePath, string newExtension )
    {
        if(string.IsNullOrEmpty(filePath))
            return filePath;

        if(string.IsNullOrEmpty(newExtension) || !newExtension.StartsWith('.'))
            throw new ArgumentException("New extension must start with a dot.", nameof(newExtension));

        string newFilePath = Path.ChangeExtension(filePath, newExtension);

        return newFilePath ?? filePath;
    }

    /// <summary>
    ///  Copy files and/or directories, all files and subdirectories will be backed up and relative paths used. Existing
    ///  files will be overwritten. Copy path will be auto created if not existing No datetime stamp will be added
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Copy( this EveFilePath destDirectory, params EveFilePath[] args )
        => destDirectory.Copy(STR_ALL_FILES, true, true, true, DateTimeStampLocation.None, args);

    /// <summary>
    ///  Copy files and/or directories, all files and subdirectories will be backed up and relative paths used. Existing
    ///  files will be overwritten. Copy path will be auto created if not existing Datetime stamp will be processed per
    ///  enum
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="dts">Prepends, Appends, Removes dts</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Copy
        ( this EveFilePath destDirectory, DateTimeStampLocation dts, params EveFilePath[] args )
        => destDirectory.Copy(STR_ALL_FILES, true, true, true, dts, args);

    /// <summary>
    ///  Copy files and/or directories, all files will be backed up and relative paths used. Existing files will be
    ///  overwritten. Copy path will be auto created if not existing
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="includeSubDirs">When directory passed to backup this determines if subdirectories are processed</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Copy
        ( this EveFilePath destDirectory, bool includeSubDirs = true, params EveFilePath[] args )
        => destDirectory.Copy(STR_ALL_FILES, includeSubDirs, true, true, DateTimeStampLocation.None, args);

    /// <summary>
    ///  Copy files and/or directories
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="fileFilter">Filter for file type, default all files ("*.*")</param>
    /// <param name="includeSubDirs">When directory passed to backup this determines if subdirectories are processed</param>
    /// <param name="buildDestDirs">Builds destination path if true, otherwise throw <see cref="DirectoryNotFoundException" /> if path invalid</param>
    /// <param name="overwrite">If true existing file is overwritten, otherwise file is skipped</param>
    /// <param name="dts">Adds/Removes/or</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Copy
        ( this EveFilePath destDirectory,
          string fileFilter,
          bool includeSubDirs,
          bool buildDestDirs,
          bool overwrite,
          DateTimeStampLocation dts,
          params EveFilePath[] args )
        => destDirectory.ProcessFiles(fileFilter, includeSubDirs, buildDestDirs, overwrite, dts, false, args);

    /// <summary>
    ///  Creates the directory represented by the given path if it does not already exist.
    /// </summary>
    /// <param name="EveFilePath">The directory path to create.</param>
    public static void CreateDirectories( this EveFilePath EveFilePath )
    {
        EveFilePath.ThrowIfNullOrEmpty();
        if(!Directory.Exists(EveFilePath.FilePath()))
            Directory.CreateDirectory(EveFilePath.FilePath());
    }

    /// <summary>
    ///  Checks whether the directory portion of the specified path exists.
    /// </summary>
    /// <param name="EveFilePath">The path whose directory will be checked.</param>
    /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
    public static bool DirectoryExists( this EveFilePath EveFilePath )
    {
        EveFilePath.ThrowIfNullOrEmpty();
        return Directory.Exists(EveFilePath.FilePath());
    }

    /// <summary>
    ///  Copies source file to destination file.
    /// </summary>
    /// <param name="sourceFile">Full path to source file</param>
    /// <param name="destFile">Full path for destination file</param>
    /// <param name="overwrite">When true and file exists, file is overwritten, otherwise no action is taken</param>
    /// <param name="buildDestDirs">
    ///  When true automatically creates necessary directories. If false and directory does not exists
    ///  DirectoryNotFoundException thrown
    /// </param>
    static void DoCopy
        ( EveFilePath sourceFile, EveFilePath destFile, bool overwrite, bool buildDestDirs = true )
    {
        sourceFile.ThrowIfEmpty();
        destFile.ThrowIfEmpty();

        if(buildDestDirs && !Directory.Exists(destFile.FilePath()))
            Directory.CreateDirectory(destFile.FilePath());
        if(overwrite || !File.Exists(destFile))
            File.Copy(sourceFile, destFile, true);
    }

    /// <summary>
    ///  Datetime stamps file name
    /// </summary>
    /// <param name="sourceFile">File name to be datetime stamped</param>
    /// <param name="dts">
    ///  Append adds dts to end of file name, Prepend adds to front of file name, None removes dts if present otherwise
    ///  return file name untouched
    /// </param>
    /// <returns>string</returns>
    public static string DtsFileName( this EveFilePath sourceFile, DateTimeStampLocation dts = DateTimeStampLocation.None )
    {
        sourceFile.ThrowIfNullOrEmpty();

        // bool isUri = sourceFile.ToString().Contains('/');
        string now = DateTime.Now.ToString("o").Replace(":", string.Empty);
        EveFilePath path = sourceFile.FilePath();

        path.ThrowIfNullOrEmpty();

        EveFilePath fileName = string.Empty;

        switch(dts) {
            case DateTimeStampLocation.Prepend:
                fileName = $@"[{now}]_{sourceFile.FileName()}";
                break;
            case DateTimeStampLocation.Append:
                fileName = $@"{sourceFile.FileName()}_[{now}]";
                break;
            case DateTimeStampLocation.None:
                fileName = Regex.Replace(sourceFile.FileName(), DtsRegEx, string.Empty);
                break;
        }
        return Path.Combine(path, fileName, sourceFile.FileExt());
    }

    /// <summary>
    ///  Determines whether a file exists at the specified path.
    /// </summary>
    /// <param name="EveFilePath">The file path to check.</param>
    /// <returns><c>true</c> if the file exists; otherwise, <c>false</c>.</returns>
    public static bool FileExists( this EveFilePath EveFilePath ) => File.Exists(EveFilePath);

    /// <summary>
    ///  Gets the file extension (including the leading dot) from the given path.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The file extension, or an empty string if none.</returns>
    public static string FileExt( this EveFilePath EveFilePath ) => Path.GetExtension(EveFilePath) ?? string.Empty;

    /// <summary>
    ///  Gets the file name (including the extension) from the given path.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The file name, or an empty string if none.</returns>
    public static string FileName( this EveFilePath EveFilePath ) => Path.GetFileName(EveFilePath) ?? string.Empty;

    /// <summary>
    ///  Gets the file name without its extension from the given path.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The file name without extension, or an empty string if none.</returns>
    public static string FileNameWithoutExt( this EveFilePath EveFilePath )
        => Path.GetFileNameWithoutExtension(EveFilePath) ?? string.Empty;

    /// <summary>
    ///  Gets the directory part of the path.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The directory path, or an empty string if none.</returns>
    public static EveFilePath FilePath( this EveFilePath EveFilePath )
        => Path.GetDirectoryName(EveFilePath) ?? string.Empty;

    /// <summary>
    ///  Gets the file extension from the file path string, including the leading dot.
    /// </summary>
    /// <param name="filePath">The path to inspect.</param>
    /// <param name="withoutDot">If true, the leading dot is removed from the returned extension.</param>
    /// <returns>The extension (with or without dot), or empty string if none.</returns>
    public static string GetFileExtension( this EveFilePath filePath, bool withoutDot = false )
    {
        if(string.IsNullOrEmpty(filePath))
            return string.Empty;
        string extension = Path.GetExtension(filePath);
        return string.IsNullOrEmpty(extension)
            ? string.Empty
            : (withoutDot ? extension.Replace(".", string.Empty) : extension);
    }

    /// <summary>
    ///  Get File Name from EveFilePath.
    /// </summary>
    /// <param name="filePath">The path to inspect.</param>
    /// <returns>The file name, or an empty string if none.</returns>
    public static string GetFileName( this EveFilePath filePath )
    {
        if(string.IsNullOrEmpty(filePath))
            return string.Empty;
        string fileName = Path.GetFileName(filePath);
        return string.IsNullOrEmpty(fileName)
            ? string.Empty
            : (new EagleViewEnt.Utilities.Core.Types.FilePath.EveFilePath(fileName));
    }

    /// <summary>
    ///  Gets the file name without its extension from the file path string.
    /// </summary>
    /// <param name="filePath">The path to inspect.</param>
    /// <returns>The file name without extension, or an empty string if none.</returns>
    public static string GetFileNameWithoutExtension( this EveFilePath filePath )
    {
        if(string.IsNullOrEmpty(filePath))
            return string.Empty;
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        return string.IsNullOrEmpty(fileName) ? string.Empty : fileName;
    }

    /// <summary>
    ///  Get Path from EveFilePath.
    /// </summary>
    /// <param name="filePath">The path to inspect.</param>
    /// <returns>The directory path portion as <see cref="EveFilePath" />, or empty if none.</returns>
    public static EveFilePath GetPath( this EveFilePath filePath )
    {
        if(string.IsNullOrEmpty(filePath))
            return string.Empty;
        string? dir = Path.GetDirectoryName(filePath);
        return string.IsNullOrEmpty(dir)
            ? string.Empty
            : (new EagleViewEnt.Utilities.Core.Types.FilePath.EveFilePath(dir));
    }

    /// <summary>
    ///  Test if file is locked by another process
    /// </summary>
    /// <param name="filePath">full path of file to test</param>
    /// <returns>bool</returns>
    public static bool IsFileLocked( this EveFilePath filePath )
    {
        try {
            using(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                return false;
        } catch(IOException) {
            return true;
        }
    }

    /// <summary>
    ///  Determines whether the specified <see cref="EveFilePath" /> is null or an empty string.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <returns><c>true</c> if null or empty; otherwise, <c>false</c>.</returns>
    public static bool IsNullOrEmpty( this EveFilePath value ) => string.IsNullOrEmpty(value);

    /// <summary>
    ///  Detects if the path resembles a URI by checking for forward slashes.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns><c>true</c> if the value looks like a URI; otherwise, <c>false</c>.</returns>
    public static bool IsUri( this EveFilePath EveFilePath )
    {
        EveFilePath.ThrowIfEmpty();
        return EveFilePath.ToString().Contains('/');
    }

    /// <summary>
    ///  Gets the name of the last directory in the path.
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The last directory name, or empty if not available.</returns>
    public static EveFilePath LastDirectory( this EveFilePath EveFilePath )
    {
        EveFilePath.ThrowIfNullOrEmpty();
        string? dir = Path.GetDirectoryName(EveFilePath);
        if(string.IsNullOrEmpty(dir))
            return string.Empty;
        string trimmed = dir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string last = Path.GetFileName(trimmed);
        return last ?? string.Empty;
    }

    /// <summary>
    ///  Moves files and/or directories, all files and subdirectories will be backed up and relative paths used.
    ///  Existing files will be overwritten. Move path will be auto created if not existing No datetime stamp will be
    ///  added
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Move( this EveFilePath destDirectory, params EveFilePath[] args )
        => destDirectory.Move(STR_ALL_FILES, true, true, true, DateTimeStampLocation.None, args);

    /// <summary>
    ///  Moves files and/or directories, all files and subdirectories will be backed up and relative paths used.
    ///  Existing files will be overwritten. Move path will be auto created if not existing Datetime stamp will be
    ///  processed per enum
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="dts">Prepends, Appends, Removes dts</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Move
        ( this EveFilePath destDirectory, DateTimeStampLocation dts, params EveFilePath[] args )
        => destDirectory.Move(STR_ALL_FILES, true, true, true, dts, args);

    /// <summary>
    ///  Move files and/or directories, all files will be backed up and relative paths used. Existing files will be
    ///  overwritten. Move path will be auto created if not existing. If args contains a directory and directory is
    ///  empty after file move then source directory will be deleted.
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="includeSubDirs">When directory passed to backup this determines if subdirectories are processed</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Move
        ( this EveFilePath destDirectory, bool includeSubDirs = true, params EveFilePath[] args )
        => destDirectory.Move(STR_ALL_FILES, includeSubDirs, true, true, DateTimeStampLocation.None, args);

    /// <summary>
    ///  Move files and/or directories
    /// </summary>
    /// <param name="destDirectory">Root backup destination directory</param>
    /// <param name="fileFilter">Filter for file type, default all files ("*.*")</param>
    /// <param name="includeSubDirs">When directory passed to backup this determines if subdirectories are processed</param>
    /// <param name="buildDestDirs">Builds destination path if true, otherwise throw <see cref="DirectoryNotFoundException" /> if path invalid</param>
    /// <param name="overwrite">If true existing file is overwritten, otherwise file is skipped</param>
    /// <param name="dts">Prepends, Appends, Removes dts</param>
    /// <param name="args">List of files and/or directories to backup</param>
    public static void Move
        ( this EveFilePath destDirectory,
          string fileFilter,
          bool includeSubDirs,
          bool buildDestDirs,
          bool overwrite,
          DateTimeStampLocation dts,
          params EveFilePath[] args )
        => destDirectory.ProcessFiles(fileFilter, includeSubDirs, buildDestDirs, overwrite, dts, true, args);

    /// <summary>
    ///  Checks if the directory of the file path exists.
    /// </summary>
    /// <param name="filePath">The file path whose directory will be checked.</param>
    /// <returns><c>true</c> if the directory exists; otherwise, <c>false</c>.</returns>
    public static bool PathExists( this EveFilePath filePath )
    {
        if(string.IsNullOrEmpty(filePath))
            return false;
        string? dir = Path.GetDirectoryName(filePath);
        return !string.IsNullOrEmpty(dir) && Directory.Exists(dir);
    }

    /// <summary>
    ///  Internal processor that performs copy or move operations on the provided targets.
    /// </summary>
    /// <param name="destDirectory">Destination root directory.</param>
    /// <param name="fileFilter">File search pattern.</param>
    /// <param name="includeSubDirs">Whether to include subdirectories.</param>
    /// <param name="buildDestDirs">Whether to create missing destination directories.</param>
    /// <param name="overwrite">Overwrite existing files when true.</param>
    /// <param name="dts">Date-time stamp behavior for destination names.</param>
    /// <param name="isMove">When true, files are moved (source deleted) after copy.</param>
    /// <param name="args">Files and/or directories to process.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="args" /> is null or empty.</exception>
    static void ProcessFiles
        ( this EveFilePath destDirectory,
          string fileFilter,
          bool includeSubDirs,
          bool buildDestDirs,
          bool overwrite,
          DateTimeStampLocation dts,
          bool isMove,
          params EveFilePath[] args )
    {
        destDirectory.ThrowIfNullOrEmpty();

        if(args?.Length == 0)
            throw new ArgumentNullException(nameof(args));

        SearchOption so = includeSubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        fileFilter = string.IsNullOrEmpty(fileFilter) ? STR_ALL_FILES : fileFilter; // default file filter to all files if not passed

        args!.ToList()
        .ForEach(item
            => {
            string fileName = DtsFileName(item.FileName(), dts);
            bool isDirectory = Directory.Exists(item);
            if(isDirectory) {
                Directory.GetFiles(item, fileFilter, so)
                .ToList()
                .ForEach(file
                    => {
                    fileName = Path.GetFileName(file);
                    int i = Path.GetFullPath(file).IndexOf(item.RootDir()) + item.RootDir().ToString().Length + 1;
                    string relPath = Path.GetFullPath(file).Substring(i).Replace(fileName, string.Empty);
                    relPath = (!relPath.Contains(Path.DirectorySeparatorChar))
                        ? item.RootDir()
                        : Path.Combine(item.RootDir(), relPath);
                    fileName = DtsFileName(fileName, dts);
                    DoCopy(file, Path.Combine(destDirectory, relPath, fileName), overwrite, buildDestDirs);
                    if(isMove)
                                File.Delete(file); });
                if(isMove && ((so == SearchOption.AllDirectories) || (Directory.GetDirectories(item).Length == 0)))
                        Directory.Delete(item, true);
            } else {
                DoCopy(item, Path.Combine(destDirectory, fileName), overwrite, buildDestDirs);
                if(isMove)
                        File.Delete(item);
            } });
    }

    /// <summary>
    ///  Reads all text from the file, or returns an empty string if the file does not exist.
    /// </summary>
    /// <param name="filePath">The file to read.</param>
    /// <returns>File contents as a string, or empty if the file does not exist.</returns>
    public static string ReadAllText( this EveFilePath filePath )
        => (!filePath.FileExists()) ? string.Empty : File.ReadAllText(filePath);

    /// <summary>
    ///  Gets the root directory of the provided path (e.g., "C:\").
    /// </summary>
    /// <param name="EveFilePath">The path to inspect.</param>
    /// <returns>The root directory, or empty if not available.</returns>
    public static EveFilePath RootDir( this EveFilePath EveFilePath )
    {
        EveFilePath.ThrowIfNullOrEmpty();
        return Path.GetPathRoot(EveFilePath) ?? string.Empty;
    }

    /// <summary>
    ///  Saves text to the file, optionally creating missing directories.
    /// </summary>
    /// <param name="filePath">The destination file path.</param>
    /// <param name="text">The text to save.</param>
    /// <param name="createMissingDirs">Whether to create missing directories.</param>
    /// <returns>True if successful, otherwise false.</returns>
    public static bool SaveText
        ( this EveFilePath filePath, string text, bool createMissingDirs = true )
    {
        try {
            string? path = Path.GetDirectoryName(filePath);
            bool pathExists = !string.IsNullOrEmpty(path) && Directory.Exists(path);

            if(string.IsNullOrEmpty(path) || (!pathExists && !createMissingDirs))
                return false;

            if(!pathExists)
                Directory.CreateDirectory(path!);

            File.WriteAllText(filePath, text);

            return true;
        } catch(Exception ex) {
            Log
            .ForContext("Path", filePath)
            .WithCallingContext<EveFilePath>()
            .Error(ex, "Error saving text to file path");
        }
        return false;
    }

    /// <summary>
    ///  Throws an <see cref="ArgumentException" /> if the provided path is null or empty.
    /// </summary>
    /// <param name="filePath">The path to validate.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="filePath" /> is null or empty.</exception>
    public static void ThrowIfEmpty( this EveFilePath filePath )
    {
        if(string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
    }

    /// <summary>
    ///  Throws a <see cref="FileNotFoundException" /> if the provided path is null, empty, or the file does not exist.
    /// </summary>
    /// <param name="EveFilePath">The file path to check.</param>
    /// <exception cref="FileNotFoundException">Thrown when the file cannot be found.</exception>
    public static void ThrowIfFileNotExists( this EveFilePath EveFilePath )
    {
        if(string.IsNullOrEmpty(EveFilePath) || !File.Exists(EveFilePath))
            throw new FileNotFoundException($"File not found: {EveFilePath}");
    }

    /// <summary>
    ///  Throws a <see cref="FileNotFoundException" /> if the file does not exist.
    /// </summary>
    /// <param name="filePath">The file path to check.</param>
    /// <exception cref="FileNotFoundException">Thrown when the file cannot be found.</exception>
    public static void ThrowIfNotFileExists( this EveFilePath filePath )
    {
        if(!filePath.FileExists())
            throw new FileNotFoundException($"File not found: {filePath}");
    }

    /// <summary>
    ///  Throws an <see cref="ArgumentNullException" /> if the provided path is null or empty.
    /// </summary>
    /// <param name="EveFilePath">The path to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="EveFilePath" /> is null or empty.</exception>
    public static void ThrowIfNullOrEmpty( this EveFilePath EveFilePath )
    {
        if(string.IsNullOrEmpty(EveFilePath))
            throw new ArgumentNullException(nameof(EveFilePath));
    }

}