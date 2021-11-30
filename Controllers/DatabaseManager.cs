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
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError Message: "+ex.Message+"\n\nStackTrace: "+ex.StackTrace);
            }
        }

        public void deleteDatabase(string dbname)
        {
            try
            {
                string path = DATABASES_PATH + dbname;
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError: " + ex.Message + "\n\nStackTrace: " + ex.StackTrace);
            }
        }

        public void createTable(string struct_)
        {
            try
            {
                if (Seleccion != null)
                {
                    if (struct_.Contains("(") && struct_.Contains(")"))
                    {
                        string[] table = struct_.Split(new char[2] { '(', ')' });
                        string[] command = table[0].Split(' ');
                        string[] columns = table[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        // Obtener el path para la estructura de la tabla
                        string path = DATABASES_PATH + Seleccion + "\\" + command[2] + ".struct";
                        if (!File.Exists(path))
                        {
                            FileStream table_ = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                            BinaryWriter tableWriter = new BinaryWriter(table_);
                            tableWriter.Write(command[2]);
                            tableWriter.Write(columns.Length);
                            int c = 0;
                            while (c < columns.Length)
                            {
                                tableWriter.Write(columns[c]);
                                c += 1;
                            }
                            tableWriter.Close();
                            table_.Close();
                        }
                    }
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError: " + ex.Message + "\n\nStackTrace: " + ex.StackTrace);
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
                if(commands[0].Equals("crea")) // Comandos que inician con crea
                {
                    if(commands[1].Equals("base") && commands.Length == 3) // Para crear base de datos
                    {
                        createDatabase(commands[2]);
                        console.AppendText(string.Format("Se creo la base de datos {0}\r\n", commands[2]));
                    }
                    else if(commands[1].Equals("tabla") && commands.Length > 2) // Para crear tablas
                    {
                        if(Seleccion!=null)
                        {
                            createTable(cmd);
                        }
                        else
                        {
                            console.AppendText("No se selecciono ninguna base de datos\r\n");
                        }
                    }
                }
                else if(commands[0].Equals("borra"))
                {
                    if(commands[1].Equals("base") && commands.Length == 3)
                    {
                        deleteDatabase(commands[2]);
                        console.AppendText(string.Format("Se elimino la base de datos {0}\r\n", commands[2]));
                    }
                }
                else if(commands[0].Equals("muestra"))
                {
                    if (commands[1].Equals("bases") && commands.Length == 2)
                    {
                        console.AppendText("Bases de datos\r\n");
                        foreach (var dir in listDatabase())
                        {
                            console.AppendText("\t- "+dir.Name + "\r\n");
                        }
                    }
                }
                else if(commands[0].Equals("usa"))
                {
                    if(commands[1].Equals("base") && commands.Length == 3)
                    {
                        Seleccion = commands[2];
                        console.AppendText(string.Format("Se selecciono la base de datos {0}\r\n", commands[2]));
                    }
                }
                else
                {
                    console.AppendText("Comando no encontrado, pruebe con: ayuda\r\n");
                }
            }
        }
    }
}
