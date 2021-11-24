using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proyecto.Controllers
{
    class DatabaseManager
    {
        private string SELECCION = "";
        private string DATABASES_PATH = AppDomain.CurrentDomain.BaseDirectory + "\\databases\\";

        public void createDatabase(string dbname)
        {
            try
            {
                if (!Directory.Exists(DATABASES_PATH)) Directory.CreateDirectory(DATABASES_PATH);
                string path = DATABASES_PATH + dbname;
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string file_path = path + "\\" + dbname + ".dsrc";
                
                Encoding encoding = Encoding.UTF8;
                BinaryWriter binaryWriter = new BinaryWriter(new FileStream(path + "\\" + dbname + ".dsrc", FileMode.OpenOrCreate), encoding);
                binaryWriter.Close();
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError Message: "+ex.Message+"\n\nStackTrace: "+ex.StackTrace);
            }
        }

        public string Seleccion
        {
            set{ SELECCION = value; }
            get{ return SELECCION; }
        }

        public DirectoryInfo[] listDatabase()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(DATABASES_PATH);
            return dirInfo.GetDirectories();
        }

        public void execute(string cmd, TextBox console)
        {
            string[] commands = cmd.Split(' ');
            if(commands.Length > 0)
            {
                if(commands[0].Equals("crear"))
                {
                    if(commands[1].Equals("base") && commands.Length == 3)
                    {
                        createDatabase(commands[2]);
                        console.AppendText(string.Format("Se creo la base de datos {0}\r\n", commands[2]));
                    }
                }
            }
        }
    }
}
