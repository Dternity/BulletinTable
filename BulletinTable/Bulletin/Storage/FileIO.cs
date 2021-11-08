using BulletinTable.Utils;
using System.Reflection;
namespace BulletinTable.Storage
{
    public class FileIO
    {
        public static readonly FileIO _IO = new FileIO();
        public string SavePath = AppContext.BaseDirectory + "Data";
        public const string JSON_EXT = ".json";

#pragma warning disable CS8604 // Possible null reference argument.

        /// <summary>
        /// Loads a specifed string using name to query the file. 
        /// </summary>
        /// <param name="name">Name of the file.</param>
        /// <returns>The file's content as string.</returns>
        /// <exception cref="DriveNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public string? LoadString(string name)
        {
            if (name == null)
            {
                LOG.Inst.Error(@"'name' was null.", MethodBase.GetCurrentMethod());
                return default;
            }

            if (!Directory.Exists(SavePath))
            {
                LOG.Inst.Error(@"Folder did not exist!", MethodBase.GetCurrentMethod());
                return default;
            }

            var fullPath = FileHelper.GetFullPath(name, SavePath, JSON_EXT);

            if (!File.Exists(fullPath))
            {
                LOG.Inst.Error(@"File not existing, please exist again.", MethodBase.GetCurrentMethod());
                return default;
            }

            return Load(fullPath);
        }

        private static string? Load(string fullPath)
        {
            try
            {
                var buffer = File.ReadAllBytes(fullPath);
                var str = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                if (string.IsNullOrEmpty(str))
                {
                    return default;
                }

                LOG.Inst.Info(str);
                return str;
            }
            catch (DriveNotFoundException ex)
            {
                LOG.Inst.Error($@"The specified drive was not found. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (UnauthorizedAccessException ex)
            {
                LOG.Inst.Error($@"No access! Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (FileNotFoundException ex)
            {
                LOG.Inst.Error($"File not found! Exception:{ex} Path:{fullPath}", MethodBase.GetCurrentMethod());
            }
            catch (IOException ex)
            {
                LOG.Inst.Error($@"An I/O error occurred while opening the file. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (Exception ex)
            {
                LOG.Inst.Error($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
                throw new UnauthorizedAccessException($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'");
            }

            return null;
        }

        /// <summary>
        /// Saves a specified string as a .json file. 
        /// </summary>
        /// <param name="str">The text to be written.</param>
        /// <param name="name">Name of the file.</param>
        /// <param name="allowOverwrite">Wether or not to overwrite existing file if available</param>
        /// <returns>Error = <see langword="false"/>. 
        /// Success = <see langword="true"/>.</returns>
        /// <exception cref="DriveNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public bool SaveString(string str, string name, bool allowOverwrite = true)
        {
            var fullPath = @$"{SavePath}/{name}.json";

            if (str == null || name == null)
            {
                LOG.Inst.Error(@"'str' or 'name' is/was null", MethodBase.GetCurrentMethod());
                return false;
            }

            if (!allowOverwrite && File.Exists(fullPath))
            {
                LOG.Inst.Error(@$"A file with the name '{name}' already exist at '{fullPath}'.", MethodBase.GetCurrentMethod());
                return false;
            }

            return Save(str, name, fullPath);
        }

        private bool Save(string str, string name, string fullPath)
        {
            FileInfo fi = new(@$"{SavePath}/{name}.json");
            try
            {
                using FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                using StreamWriter sw = new(fs);

                sw.Write(str);
                sw.Close();

                LOG.Inst.Info($@"Saved '{name}' Path: '{fullPath}'");
                return true;
            }
            catch (DriveNotFoundException ex)
            {
                LOG.Inst.Error($@"The specified drive was not found. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (UnauthorizedAccessException ex)
            {
                LOG.Inst.Error($@"No access! Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (FileNotFoundException ex)
            {
                LOG.Inst.Error($"File not found! Exception:{ex} Path:{fullPath}", MethodBase.GetCurrentMethod());
            }
            catch (IOException ex)
            {
                LOG.Inst.Error($@"An I/O error occurred while opening the file. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
            }
            catch (Exception ex)
            {
                LOG.Inst.Error($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'", MethodBase.GetCurrentMethod());
                throw new UnauthorizedAccessException($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'");
            }
            return false;
        }

        /// <summary>
        /// Checks to see if the specified name is a valid filename. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns><see langword="true"/> = Name is valid. <see langword="false"/> = Name is not valid.</returns>
        public bool IsValidFilename(string filename)
        {
            return !string.IsNullOrEmpty(filename) &&
              filename.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;             
        }

        /// <summary>
        /// Check if file exists.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool FileExist(string filename)
        {
            return File.Exists(Path.Combine(@$"{SavePath}/{filename}.json"));
        }
    }
}
