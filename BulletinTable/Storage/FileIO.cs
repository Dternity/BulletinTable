namespace BulletinTable.Storage
{
    public class FileIO
    {
        public static readonly FileIO _IO = new FileIO();
        public string SavePath = AppContext.BaseDirectory + "Data";


        /// <summary>
        /// Saves a specified string as a .json file. 
        /// </summary>
        /// <param name="str">The text to be written.</param>
        /// <param name="name">Name of the file.</param>
        /// <returns>Error = <see langword="false"/>. 
        /// Success = <see langword="true"/>.</returns>
        /// <exception cref="DriveNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public bool SaveString(string str, string name)
        {
            var fullPath = @$"{SavePath}/{name}.json";

            if (str == null || name == null)
            {
                Console.WriteLine(@"'str' or 'name' is/was null - FileIO");
                return false;
            }

            if (File.Exists(fullPath))
            {
                Console.WriteLine(@$"A file with the name '{name}' already exist at '{fullPath}'.");
                return false;
            }

            FileInfo fi = new(@$"{SavePath}/{name}.json");
            try
            {
                using FileStream fs = fi.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                using StreamWriter sw = new(fs);

                sw.Write(str);
                sw.Close();

                Console.WriteLine($@"Saved '{name}' Path: '{fullPath}'");
                return true;
            }
            catch (DriveNotFoundException ex)
            {
                Console.WriteLine($@"The specified drive was not found. Exception: '{ex}' Path: '{fullPath}'");
                throw new DriveNotFoundException($@"The specified drive was not found. Exception: '{ex}' Path: '{fullPath}'");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($@"No access! Exception: '{ex}' Path: '{fullPath}'");
                throw new UnauthorizedAccessException($@"No access! Exception: '{ex}' Path: '{fullPath}'");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found! Exception:{ex} Path:{fullPath}");
                throw new FileNotFoundException(@$"File not found! Exception:{ex} Path:{fullPath}");
            }
            catch (IOException ex) 
            {
                Console.WriteLine($@"An I/O error occurred while opening the file. Exception: '{ex}' Path: '{fullPath}'");
                throw new IOException($@"An I/O error occurred while opening the file. Exception: '{ex}' Path: '{fullPath}'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'");
                throw new UnauthorizedAccessException($@"An unhandled exception occurred. Exception: '{ex}' Path: '{fullPath}'");
            }
        }
    }
}
