using System;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace Logger
{
    public class LogWriter
    {
        #region Variables
        /// <summary>
        /// (String) Path to the folder where the file is saved.
        /// </summary>
        public string Path { get { return _path; } }
        /// <summary>
        /// (String) Name of the file without .log extension.
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// (String) If an error occurs in the WriteLog method (returning false) it will be placed here.
        /// </summary>
        public string ErrorMessage { get { return errormsg; } }

        private string _path;
        private string _name;
        private string errormsg;
        #endregion

        #region Enumerations
        /// <summary>
        /// Enumerations for the type of logging that will be written.
        /// </summary>
        public enum LOG_TYPE
        {
            WARNING,
            VERBOSE,
            DEBUG,
            ERROR,
            INFO,
            CONFIG
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the logger
        /// </summary>
        /// <param name="sPath">Path to the folder where the log file will be written. Throws DirectoryNotFoundException if the path is invalid.</param>
        /// <param name="fName">Name of the log file (a .log extension will be appended automatically--Does not check for invalid filename characters).</param>
        public LogWriter(string sPath, string fName)
        {
            // Validation
            if (!(Directory.Exists(sPath) || string.IsNullOrEmpty(sPath)))
                throw new DirectoryNotFoundException();
            _path = sPath;
            if (!(string.IsNullOrEmpty(fName)))
                _name = fName + ".log";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Writes data to the log file specified in the constructor.
        /// </summary>
        /// <param name="logtype">Used from the LOG_TYPE enumeration to determine whether the log is verbose, info, error, etc...</param>
        /// <param name="message">This is the data that will be written to the log.</param>
        /// <param name="code_local">Optional. This is the location of the code where the information is being logged.</param>
        /// <returns>True/False depending on if the write is successful.</returns>
        public bool WriteLog(LOG_TYPE logtype, string message, [CallerFilePath]string codeName=null, [CallerMemberName]string code_local=null)
        {
            string full_path = _path + "\\" + _name;
            string codefile = codeName.Split('\\').Last();
            try
            {
                StreamWriter log_writer = new StreamWriter(full_path, true);
                //Date [LOG_TYPE]   Message (Code location)
                if (logtype == LOG_TYPE.VERBOSE)
                {
                    Console.WriteLine($"Out>[{logtype}]\t{message}\t({codefile}->{code_local})");
                    log_writer.WriteLine($"{System.DateTime.Now}[{logtype}]\t{message}\t({codefile}->{code_local})");
                }
                else
                {
                    log_writer.WriteLine($"{System.DateTime.Now}[{logtype}]\t{message}\t({codefile}->{code_local})");
                }
                log_writer.Close();

                return true;
            }
            catch (Exception err)
            {
                errormsg = "[Log>>WriteLog]: " + err.Message;
                return false;
            }
        }
        #endregion Methods
    }
}
