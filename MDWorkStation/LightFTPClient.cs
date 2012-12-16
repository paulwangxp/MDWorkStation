using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace MDWorkStation
{

    namespace LightFTP
    {
        public class FtpClient
        {

            public class FtpException : Exception
            {
                public FtpException(string message) : base(message) { }
                public FtpException(string message, Exception innerException) : base(message, innerException) { }
            }

            private static int BUFFER_SIZE = 512;
            private static Encoding ASCII = Encoding.ASCII;

            public System.Windows.Forms.ListView lstMessage;

            private bool verboseDebugging = true; //false;

            // defaults
            private string server = "localhost";
            private string remotePath = ".";
            private string username = "anonymous";
            private string password = "anonymous@anonymous.net";
            private string message = null;
            private string result = null;
            private int m_iServerOS;

            private int port = 21;
            private int bytes = 0;
            private int resultCode = 0;

            private bool loggedin = false;
            private bool binMode = false;

            private Byte[] buffer = new Byte[BUFFER_SIZE];
            private Byte[] receiveBuffer = new Byte[BUFFER_SIZE * 2];
            private Socket clientSocket = null;

            private int timeoutSeconds = 10;

            /// <summary>
            /// Default contructor
            /// </summary>
            public FtpClient()
            {
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="server"></param>
            /// <param name="username"></param>
            /// <param name="password"></param>
            public FtpClient(string server, string username, string password)
            {
                this.server = server;
                this.username = username;
                this.password = password;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="server"></param>
            /// <param name="username"></param>
            /// <param name="password"></param>
            /// <param name="timeoutSeconds"></param>
            /// <param name="port"></param>
            public FtpClient(string server, string username, string password, int timeoutSeconds, int port)
            {
                this.server = server;
                this.username = username;
                this.password = password;
                this.timeoutSeconds = timeoutSeconds;
                this.port = port;
            }

            /// <summary>
            /// Display Server OS
            /// </summary>
            public int ServerOS
            {
                get { return m_iServerOS; }
            }


            /// <summary>
            /// Display all communications to the debug log
            /// </summary>
            public bool VerboseDebugging
            {
                get
                {
                    return this.verboseDebugging;
                }
                set
                {
                    this.verboseDebugging = value;
                }
            }
            /// <summary>
            /// Remote server port. Typically TCP 21
            /// </summary>
            public int Port
            {
                get
                {
                    return this.port;
                }
                set
                {
                    this.port = value;
                }
            }
            /// <summary>
            /// Timeout waiting for a response from server, in seconds.
            /// </summary>
            public int Timeout
            {
                get
                {
                    return this.timeoutSeconds;
                }
                set
                {
                    this.timeoutSeconds = value;
                }
            }
            /// <summary>
            /// Gets and Sets the name of the FTP server.
            /// </summary>
            /// <returns></returns>
            public string Server
            {
                get
                {
                    return this.server;
                }
                set
                {
                    this.server = value;
                }
            }
            /// <summary>
            /// Gets and Sets the port number.
            /// </summary>
            /// <returns></returns>
            public int RemotePort
            {
                get
                {
                    return this.port;
                }
                set
                {
                    this.port = value;
                }
            }
            /// <summary>
            /// GetS and Sets the remote directory.
            /// </summary>
            public string RemotePath
            {
                get
                {
                    return this.remotePath;
                }
                set
                {
                    this.remotePath = value;
                }

            }
            /// <summary>
            /// Gets and Sets the username.
            /// </summary>
            public string Username
            {
                get
                {
                    return this.username;
                }
                set
                {
                    this.username = value;
                }
            }
            /// <summary>
            /// Gets and Set the password.
            /// </summary>
            public string Password
            {
                get
                {
                    return this.password;
                }
                set
                {
                    this.password = value;
                }
            }

            /// <summary>
            /// If the value of mode is true, set binary mode for downloads, else, Ascii mode.
            /// </summary>
            public bool BinaryMode
            {
                get
                {
                    return this.binMode;
                }
                set
                {
                    if (this.binMode == value) return;

                    if (value)
                        sendCommand("TYPE I");

                    else
                        sendCommand("TYPE A");

                    if (this.resultCode != 200) FireException(result.Substring(4));
                }
            }
            /// <summary>
            /// Login to the remote server.
            /// </summary>
            public void Login()
            {
                if (this.loggedin) this.Close();

                showMessage("Opening connection to " + this.server, false);

                //IPAddress addr = null;
                IPEndPoint ep = null;

                try
                {
                    this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ep = new IPEndPoint(IPAddress.Parse(this.server), this.port);

                    //this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //addr = Dns.Resolve(this.server).AddressList[0];
                    //ep = new IPEndPoint(addr, this.port);
                    this.clientSocket.Connect(ep);
                }
                catch (Exception ex)
                {
                    // doubtfull
                    if (this.clientSocket != null && this.clientSocket.Connected) this.clientSocket.Close();

                    FireException("Couldn't connect to remote server", ex);
                }

                this.readResponse();

                if (this.resultCode != 220)
                {
                    this.Close();
                    FireException(this.result.Substring(4));
                }

                /// <summary> 
                /// Sends a user name
                /// </summary>
                /// <remarks>Reply codes: 230 530 500 501 421 331 332</remarks>
                this.sendCommand("USER " + username);

                if (!(this.resultCode == 331 || this.resultCode == 230))
                {
                    this.cleanup();
                    FireException(this.result.Substring(4));
                }

                if (this.resultCode != 230)
                {
                    /// <summary>
                    /// send the user's password
                    /// </summary>
                    /// <remarks>Reply codes: 230 202 530 500 501 503 421 332</remarks>
                    this.sendCommand("PASS " + password);

                    if (!(this.resultCode == 230 || this.resultCode == 202))
                    {
                        this.cleanup();
                        FireException(this.result.Substring(4));
                    }
                }

                this.loggedin = true;

                showMessage("Connected to " + this.server, false);


                /// <summary>
                /// Finds out the type of operating system at the server.
                /// </summary>
                /// <remarks>Reply codes: 215 500 501 502 421</remarks>
                this.sendCommand("SYST\r\n");
                if (!(this.resultCode == 215))
                {
                    FireException(this.result.Substring(4));
                }

                string lstr_temp = this.result.Remove(0, 4).Substring(0, 4);
                if (lstr_temp == "UNIX")
                {
                    m_iServerOS = 1;
                }
                else if (lstr_temp == "Wind")
                {
                    m_iServerOS = 2;
                }
                else
                {
                    m_iServerOS = 3;
                    /*	Currently not supported */
                    this.cleanup();
                    FireException("This version of FTP Explorer supports browsing only on Windows and Unix based FTP Services. FTP browsing on other FTP services will be enabled in future versions.");
                }


                this.ChangeDir(this.remotePath);
            }

            /// <summary>
            /// Close the FTP connection.
            /// </summary>
            public void Close()
            {
                showMessage("Closing connection to " + this.server, false);

                if (this.clientSocket != null)
                {
                    /// <summary>
                    /// This command terminates a USER and if file transfer is not in progress, the server closes the control connection. 
                    /// </summary>
                    /// <remarks>Reply codes: 221 500</remarks>
                    this.sendCommand("QUIT");
                }

                this.cleanup();
            }

            /// <summary>
            /// Return a string array containing the remote directory's file list.
            /// </summary>
            /// <returns></returns>
            public string[] GetFileList()
            {
                return this.GetFileList("*.*");
            }

            /// <summary>
            /// Return a string array containing the remote directory's file list.
            /// </summary>
            /// <param name="mask"></param>
            /// <returns></returns>
            public string[] GetFileList(string mask)
            {
                if (!this.loggedin) this.Login();

                Socket cSocket = createDataSocket();

                /// <summary>
                /// Causes a list to be sent from the server to the passive DTP. If 
                /// the pathname specifies a directory or other group of files, the server should 
                /// transfer a list of files in the specified directory. If the pathname specifies 
                /// a file then the server should send current information on the file. A null 
                /// argument implies the user's current working or default directory.
                /// </summary>
                /// <remarks>Reply codes: 125 150 226 250 425 426 451 450 500 501 502 421 530</remarks>
                this.sendCommand("LIST " + mask);

                if (!(this.resultCode == 150 || this.resultCode == 125 || this.resultCode == 250 || this.resultCode == 226))
                    FireException(this.result.Substring(4));

                this.message = "";

                string l_strOutput = "", l_strTemp = "";
                int l_iRetval = 0;

                receiveBuffer.Initialize();
                l_strTemp = "";
                l_strOutput = "";

                Thread.Sleep(700);

                for (; (l_iRetval = cSocket.Receive(receiveBuffer)) > 0; )
                {
                    l_strTemp = Encoding.ASCII.GetString(receiveBuffer, 0, l_iRetval);
                    l_strOutput += l_strTemp;
                    if (cSocket.Available == 0)
                    {
                        break;
                    }
                }

                this.message = l_strOutput;
                string[] msg = this.message.Replace("\r", "").Split('\n');

                cSocket.Close();

                if (this.message.IndexOf("No such file or directory") != -1)
                    msg = new string[] { };

                //			this.readResponse();
                //
                //			if ( this.resultCode != 226 )
                //				msg = new string[]{};
                //			//	FireException(result.Substring(4));

                return msg;
            }

            /// <summary>
            /// Return the size of a file.
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public long GetFileSize(string fileName)
            {
                if (!this.loggedin) this.Login();

                this.sendCommand("SIZE " + fileName);
                long size = 0;

                if (this.resultCode == 213)
                    size = long.Parse(this.result.Substring(4));

                else
                    FireException(this.result.Substring(4));

                return size;
            }


            /// <summary>
            /// Download a file to the Assembly's local directory,
            /// keeping the same file name.
            /// </summary>
            /// <param name="remFileName"></param>
            public void Download(string remFileName)
            {
                this.Download(remFileName, "", false);
            }

            /// <summary>
            /// Download a remote file to the Assembly's local directory,
            /// keeping the same file name, and set the resume flag.
            /// </summary>
            /// <param name="remFileName"></param>
            /// <param name="resume"></param>
            public void Download(string remFileName, Boolean resume)
            {
                this.Download(remFileName, "", resume);
            }

            /// <summary>
            /// Download a remote file to a local file name which can include
            /// a path. The local file name will be created or overwritten,
            /// but the path must exist.
            /// </summary>
            /// <param name="remFileName"></param>
            /// <param name="locFileName"></param>
            public void Download(string remFileName, string locFileName)
            {
                this.Download(remFileName, locFileName, false);
            }

            /// <summary>
            /// Download a remote file to a local file name which can include
            /// a path, and set the resume flag. The local file name will be
            /// created or overwritten, but the path must exist.
            /// </summary>
            /// <param name="remFileName"></param>
            /// <param name="locFileName"></param>
            /// <param name="resume"></param>
            public void Download(string remFileName, string locFileName, Boolean resume)
            {
                if (!this.loggedin) this.Login();

                this.BinaryMode = true;

                showMessage("Downloading file " + remFileName + " from " + server + "/" + remotePath, false);

                if (locFileName.Equals(""))
                {
                    locFileName = remFileName;
                }

                FileStream output = null;

                if (!File.Exists(locFileName))
                    output = File.Create(locFileName);

                else
                    output = new FileStream(locFileName, FileMode.Open);

                Socket cSocket = createDataSocket();

                long offset = 0;

                if (resume)
                {
                    offset = output.Length;

                    if (offset > 0)
                    {

                        /// <summary>
                        /// The argument field represents the server marker at which file transfer is to be
                        /// restarted. This command does not cause file transfer but skips over the file to
                        /// the specified data checkpoint.
                        /// </summary>
                        /// <remarks>Reply codes: 500 501 502 421 530 350</remarks>
                        this.sendCommand("REST " + offset);
                        if (this.resultCode != 350)
                        {
                            //Server dosnt support resuming
                            offset = 0;
                            showMessage("Resuming not supported:" + result.Substring(4), false);
                        }
                        else
                        {
                            showMessage("Resuming at offset " + offset, false);
                            output.Seek(offset, SeekOrigin.Begin);
                        }
                    }
                }

                /// <summary>
                /// Causes the server-DTP to transfer a copy of the file, specified 
                /// in the pathname, to the server- or user-DTP at the other end of the data 
                /// connection. The status and contents of the file at the server site shall be unaffected.
                /// </summary>
                /// <remarks>Reply codes: 125 150 110 226 250 425 426 451 450 550 500 501 421 530</remarks>
                this.sendCommand("RETR " + remFileName);

                /*	125,150,110,250,226 (success) */
                if (this.resultCode != 150 && this.resultCode != 125 && this.resultCode != 110 && this.resultCode != 250 && this.resultCode != 226)
                {
                    FireException(this.result.Substring(4));
                }

                DateTime timeout = DateTime.Now.AddSeconds(this.timeoutSeconds);

                while (timeout > DateTime.Now)
                {
                    this.bytes = cSocket.Receive(buffer, buffer.Length, 0);
                    output.Write(this.buffer, 0, this.bytes);

                    if (this.bytes <= 0)
                    {
                        break;
                    }
                }

                output.Close();

                if (cSocket.Connected) cSocket.Close();

                this.readResponse();

                if (this.resultCode != 226 && this.resultCode != 250)
                    FireException(this.result.Substring(4));
            }


            /// <summary>
            /// Upload a file.
            /// </summary>
            /// <param name="fileName"></param>
            public void Upload(string fileName)
            {
                this.Upload(fileName, false);
            }


            /// <summary>
            /// Upload a file and set the resume flag.
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="resume"></param>
            public void Upload(string fileName, bool resume)
            {
                if (!this.loggedin) this.Login();

                Socket cSocket = null;
                long offset = 0;

                if (resume)
                {
                    try
                    {
                        this.BinaryMode = true;

                        offset = GetFileSize(Path.GetFileName(fileName));
                    }
                    catch (Exception)
                    {
                        // file not exist
                        offset = 0;
                    }
                }

                // open stream to read file
                FileStream input = new FileStream(fileName, FileMode.Open, System.IO.FileAccess.Read);

                if (resume && input.Length < offset)
                {
                    // different file size
                    showMessage("Overwriting " + fileName, false);
                    offset = 0;
                }
                else if (resume && input.Length == offset)
                {
                    // file done
                    input.Close();
                    showMessage("Skipping completed " + fileName + " - turn resume off to not detect.", false);
                    return;
                }

                // dont create untill we know that we need it
                cSocket = this.createDataSocket();

                if (offset > 0)
                {
                    /// <summary>
                    /// The argument field represents the server marker at which file transfer is to be
                    /// restarted. This command does not cause file transfer but skips over the file to
                    /// the specified data checkpoint.
                    /// </summary>
                    /// <remarks>Reply codes: 500 501 502 421 530 350</remarks>
                    this.sendCommand("REST " + offset);
                    if (this.resultCode != 350)
                    {
                        showMessage("Resuming not supported", false);
                        offset = 0;
                    }
                }

                /// <summary>
                /// Causes the server-DTP to accept the data transferred via the data connection 
                /// and to store the data as a file at the server site. If the file specified in 
                /// the pathname exists at the server site, then its contents shall be replaced 
                /// by the data being transferred. A new file is created at the server site if 
                /// the file specified in the pathname does not already exist.
                /// </summary>
                /// <remarks>Reply codes: 125 150 110 226 250 425 426 451 551 552 532 450 452 553 500 501 421 530</remarks>
                this.sendCommand("STOR " + Path.GetFileName(fileName));

                if (this.resultCode != 125 && this.resultCode != 150)
                    FireException(result.Substring(4));

                if (offset != 0)
                {
                    showMessage("Resuming at offset " + offset, false);

                    input.Seek(offset, SeekOrigin.Begin);
                }

                showMessage("Uploading file " + fileName + " to " + remotePath, false);

                while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cSocket.Send(buffer, bytes, 0);
                }

                input.Close();

                if (cSocket.Connected)
                {
                    cSocket.Close();
                }

                this.readResponse();

                if (this.resultCode != 226 && this.resultCode != 250)
                    FireException(this.result.Substring(4));
            }

            /// <summary>
            /// Upload a directory and its file contents
            /// </summary>
            /// <param name="path"></param>
            /// <param name="recurse">Whether to recurse sub directories</param>
            public void UploadDirectory(string path, bool recurse)
            {
                this.UploadDirectory(path, recurse, "*.*");
            }

            /// <summary>
            /// Upload a directory and its file contents
            /// </summary>
            /// <param name="path"></param>
            /// <param name="recurse">Whether to recurse sub directories</param>
            /// <param name="mask">Only upload files of the given mask - everything is '*.*'</param>
            public void UploadDirectory(string path, bool recurse, string mask)
            {
                string[] dirs = path.Replace("/", @"\").Split('\\');
                string rootDir = dirs[dirs.Length - 1];

                // make the root dir if it doed not exist
                try
                {
                    // try to retrieve files
                    this.GetFileList(rootDir);
                }
                catch
                {
                    // if receive an error
                    this.MakeDir(rootDir);
                }

                //			if ( this.GetFileList(rootDir).Length < 1 ) this.MakeDir(rootDir);

                this.ChangeDir(rootDir);

                foreach (string file in Directory.GetFiles(path, mask))
                {
                    this.Upload(file, true);
                }
                if (recurse)
                {
                    foreach (string directory in Directory.GetDirectories(path))
                    {
                        this.UploadDirectory(directory, recurse, mask);
                    }
                }

                this.ChangeDir("..");
            }

            /// <summary>
            /// Delete a file from the remote FTP server.
            /// </summary>
            /// <param name="fileName"></param>
            public void DeleteFile(string fileName)
            {
                if (!this.loggedin) this.Login();

                /// <summary>
                /// Causes the file specified in the pathname to be deleted at the server site
                /// </summary>
                /// <remarks>Reply codes: 250 450 550 500 501 502 421 530</remarks>
                this.sendCommand("DELE " + fileName);

                if (this.resultCode != 250) FireException(this.result.Substring(4));

                showMessage("Deleted file " + fileName, false);
            }

            /// <summary>
            /// Rename a file on the remote FTP server.
            /// </summary>
            /// <param name="oldFileName"></param>
            /// <param name="newFileName"></param>
            /// <param name="overwrite">setting to false will throw exception if it exists</param>
            public void RenameFile(string oldFileName, string newFileName, bool overwrite)
            {
                if (!this.loggedin) this.Login();

                /// <summary>
                /// Specifies the old pathname of the file which is to be renamed. This 
                /// command must be immediately followed by a "rename to" command specifying the new 
                /// file pathname.
                /// </summary>
                /// <remarks>Reply codes: 450 550 500 501 502 421 530 350</remarks>
                this.sendCommand("RNFR " + oldFileName);

                if (this.resultCode != 350)
                    FireException(this.result.Substring(4));

                //			if ( !overwrite && this.GetFileList(newFileName).Length > 0 ) 
                //				FireException("File already exists");

                // verify if newFileName exists
                if (!overwrite)
                {
                    this.sendCommand("SIZE " + newFileName);
                    if (this.resultCode == 213)
                        FireException("File already exists");
                }

                /// <summary>
                /// Specifies the new pathname of the file specified in the immediately 
                /// preceding "rename from" command. Together the two commands cause a file to be renamed.
                /// </summary>
                /// <remarks>Reply codes: 250 532 553 500 501 502 503 421 530</remarks>
                this.sendCommand("RNTO " + newFileName);

                if (this.resultCode != 250)
                    FireException(this.result.Substring(4));

                showMessage("Renamed file " + oldFileName + " to " + newFileName, false);
            }

            /// <summary>
            /// Create a directory on the remote FTP server.
            /// </summary>
            /// <param name="dirName"></param>
            public void MakeDir(string dirName)
            {
                if (!this.loggedin) this.Login();

                /// <summary>
                /// Causes the directory specified in the pathname to be created as 
                /// a directory (if the pathname is absolute) or as a subdirectory of the current 
                /// working directory (if the pathname is relative).
                /// </summary>
                /// <remarks>Reply codes: 257 500 501 502 421 530 550</remarks>
                this.sendCommand("MKD " + dirName);

                if (this.resultCode != 250 && this.resultCode != 257 && this.resultCode != 550/*add by paul*/) FireException(this.result.Substring(4));

                showMessage("Created directory " + dirName, false);
            }

            /// <summary>
            /// Delete a directory on the remote FTP server.
            /// </summary>
            /// <param name="dirName"></param>
            public void RemoveDir(string dirName)
            {
                if (!this.loggedin) this.Login();

                /// <summary>
                /// Causes the directory specified in the pathname to be removed as 
                /// a directory (if the pathname is absolute) or as a subdirectory of the current 
                /// working directory (if the pathname is relative).
                /// </summary>
                /// <remarks>Reply codes: 250 500 501 502 421 530 550</remarks>
                this.sendCommand("RMD " + dirName);

                if (this.resultCode != 250) FireException(this.result.Substring(4));

                showMessage("Removed directory " + dirName, false);
            }

            /// <summary>
            /// Change the current working directory on the remote FTP server.
            /// </summary>
            /// <param name="dirName"></param>
            public string ChangeDir(string dirName)
            {
                if (dirName == null || dirName.Equals(".") || dirName.Length == 0)
                {
                    return "";
                }

                if (!this.loggedin) this.Login();

                if (dirName == this.remotePath)//add by paul
                    return this.remotePath;

                /// <summary>
                /// Changes the current directory
                /// </summary>
                /// <remarks>Reply codes: 250 500 501 502 421 530 550</remarks>
                this.sendCommand("CWD " + dirName);

                //if ( ( this.resultCode != 250 ) && ( this.resultCode != 150 ) )

                if ((this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 530) || (this.resultCode == 550))
                    FireException(result.Substring(4));

                /// <summary>
                /// Causes the name of the current working directory to be returned in the reply.
                /// </summary>
                /// <remarks>Reply codes: 257 500 501 502 421 550</remarks>
                this.sendCommand("PWD");

                if ((this.resultCode == 500) || (this.resultCode == 501) || (this.resultCode == 502) || (this.resultCode == 421) || (this.resultCode == 550))
                    FireException(result.Substring(4));

                // gonna have to do better than this....
                this.remotePath = this.message.Split('"')[1];

                showMessage("Current directory is " + this.remotePath, false);
                return this.remotePath;
            }

            /// <summary>
            /// 
            /// </summary>
            private void readResponse()
            {
                this.message = "";
                this.result = this.readLine();

                if (this.result.Length > 3)
                    this.resultCode = int.Parse(this.result.Substring(0, 3));
                else
                    this.result = null;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            private string readLine()
            {
                string l_strOutput = "", l_strTemp = "";
                int l_iRetval = 0;

                receiveBuffer.Initialize();
                l_strTemp = "";
                l_strOutput = "";

                Thread.Sleep(700);

                /* Receive data in a loop until the FTP server sends it */
                for (; (l_iRetval = clientSocket.Receive(receiveBuffer)) > 0; )
                {
                    l_strTemp = Encoding.ASCII.GetString(receiveBuffer, 0, l_iRetval);
                    l_strOutput += l_strTemp;
                    if (clientSocket.Available == 0)
                    {
                        break;
                    }
                }
                this.message = l_strOutput;

                string[] msg = this.message.Split('\n');

                if (this.message.Length > 2)
                    this.message = msg[msg.Length - 2];

                else
                    this.message = msg[0];


                if (this.message.Length > 4 && !this.message.Substring(3, 1).Equals(" "))
                    return this.readLine();

                if (this.verboseDebugging)
                {
                    for (int i = 0; i < msg.Length - 1; i++)
                    {
                        showMessage(msg[i], false);
                    }
                }

                return message;
            }

            /// <summary>
            /// sendCommand
            /// </summary>
            /// <param name="command"></param>
            private void sendCommand(String command)
            {
                int l_iRetval = 0;

                if (this.verboseDebugging)
                {
                    if (command.IndexOf("PASS ") >= 0)
                    {
                        // don't show password in message area
                        // show only *
                        showMessage("PASS [*** hidden ***]", false);
                    }
                    else
                    {
                        showMessage(command, false);
                    }
                }

                try
                {
                    Byte[] cmdBytes = Encoding.ASCII.GetBytes((command.Trim() + "\r\n").ToCharArray());
                    l_iRetval = clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
                    this.readResponse();
                }
                catch (Exception ex)
                {
                    throw new IOException(ex.Message);
                }
            }

            /// <summary>
            /// when doing data transfers, we need to open another socket for it.
            /// </summary>
            /// <returns>Connected socket</returns>
            private Socket createDataSocket()
            {
                /// <summary>
                /// Requests the server-DTP to "listen" on a data port (which is not
                /// its default data port) and to wait for a connection rather than initiate one
                /// upon receipt of a transfer command. The response to this command includes the 
                /// host and port address this server is listening on. 
                /// </summary>
                /// <remarks>Reply codes: 227 500 501 502 421 530</remarks>
                this.sendCommand("PASV");

                if (this.resultCode != 227) FireException(this.result.Substring(4));

                int index1 = this.result.IndexOf('(');
                int index2 = this.result.IndexOf(')');

                string ipData = this.result.Substring(index1 + 1, index2 - index1 - 1);

                int[] parts = new int[6];

                int len = ipData.Length;
                int partCount = 0;
                string buf = "";

                for (int i = 0; i < len && partCount <= 6; i++)
                {
                    char ch = char.Parse(ipData.Substring(i, 1));

                    if (char.IsDigit(ch))
                        buf += ch;

                    else if (ch != ',')
                        FireException("Malformed PASV result: " + result);

                    if (ch == ',' || i + 1 == len)
                    {
                        try
                        {
                            parts[partCount++] = int.Parse(buf);
                            buf = "";
                        }
                        catch (Exception ex)
                        {
                            FireException("Malformed PASV result (not supported?): " + this.result, ex);
                        }
                    }
                }

                string ipAddress = parts[0] + "." + parts[1] + "." + parts[2] + "." + parts[3];

                int port = (parts[4] << 8) + parts[5];

                Socket socket = null;
                IPEndPoint ep = null;

                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ep = new IPEndPoint(IPAddress.Parse(this.server), port);

                    //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //ep = new IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);
                    socket.Connect(ep);
                }
                catch (Exception ex)
                {
                    // doubtfull....
                    if (socket != null && socket.Connected) socket.Close();

                    FireException("Can't connect to remote server", ex);
                }

                return socket;
            }

            /// <summary>
            /// Always release those sockets.
            /// </summary>
            private void cleanup()
            {
                if (this.clientSocket != null)
                {
                    this.clientSocket.Close();
                    this.clientSocket = null;
                }
                this.loggedin = false;
            }

            /// <summary>
            /// Destuctor
            /// </summary>
            ~FtpClient()
            {
                this.cleanup();
            }

            private void FireException(string message, Exception innerException)
            {
                showMessage(message, true);
                throw new FtpException(message, innerException);
            }

            private void FireException(string message)
            {
                showMessage(message, true);
                throw new FtpException(message);
            }

            private void showMessage(string message, Boolean error)
            {
                if (error)
                    LogManager.WriteErrorLog(message);
                else
                    LogManager.WriteLog(message);
                

                return;

                //System.Windows.Forms.ListViewItem lvItem = new System.Windows.Forms.ListViewItem(message.Replace("\r", "").Replace("\n", ""), 0);

                //if (error)
                //{
                //    lvItem.Font = new System.Drawing.Font(lvItem.Font, lvItem.Font.Style | System.Drawing.FontStyle.Bold);
                //    lvItem.ForeColor = System.Drawing.Color.Red;
                //}
                //lstMessage.Items.Add(lvItem);
                //lstMessage.EnsureVisible(lstMessage.Items.Count - 1);
                //lstMessage.Invalidate();
                //lstMessage.Update();
            }


        }
    }
}
