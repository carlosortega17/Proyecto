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
                        tableStruct(columns);
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
                        else
                        {
                            throw new Exception(string.Format("La tabla {0} ya existe", command[2]));
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
                            try
                            {
                                createTable(cmd);
                                console.AppendText(string.Format("Se creo la tabla {0}\r\n", commands[2]));
                            }catch(Exception ex)
                            {
                                console.AppendText(string.Format("{0}\r\n", ex.Message));
                            }
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
                else if(commands[0].Equals("describir"))
                {
                    readTableStruct("ejemplo");
                }
                else
                {
                    console.AppendText("Comando no encontrado, pruebe con: ayuda\r\n");
                }
            }
        }

        // Metodos utilizados para las tablas y sus datos
        private string[] readTableStruct(string tbname)
        {
            string[] columns = null;
            if (Seleccion!=null)
            {
                string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".struct";
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fs);
                string name = reader.ReadString();
                Int32 numCols = reader.ReadInt32();
                columns = new string[numCols];
                int c = 0;
                while(fs.Position != fs.Length)
                {
                    columns[c] = reader.ReadString();
                    c += 1;
                }
                tableStruct(columns);
                reader.Close();
                fs.Close();
            }
            return columns;
        }

        private void tableStruct(string[] colums)
        {
            string[] types = {"entero", "caracter", "decimal", "fecha"}; // Tipos de datos
            string[] names = new string[colums.Length];
            for(int i = 0; i < names.Length; i++)
            {
                names[i] = colums[i].Split(' ')[0];
            }
            // Seccion para verificar si los nombres estan repetidos
            int repetidos = 0;
            for (int i = 0; i < names.Length; i++)
            {
                for (int j = 1; j < names.Length; j++)
                {
                    if (j != i) if (names[i] == names[j]) repetidos += 1;
                }
            }
            if (repetidos > 0) throw new Exception("Nombres de columnas repetidos "+repetidos);

            for (int i = 0; i < colums.Length; i++)
            {
                string[] col = colums[i].Split(' ');
                string name = col[0];
                string type = col[1];
                bool typePass = false;
                foreach (string t in types)
                {
                    if (t == type) typePass = true;
                }
                if (!typePass) throw new Exception("Error, el tipo de dato no corresponde con los tipos existentes");
                if (type == types[3])
                {
                    int size = 8;
                    Console.WriteLine("Name: {0}\nType: {1}\nSize: {2}", name, type, size);
                }
                else
                {
                    int size = int.Parse(col[2]);
                    int point = 0;
                    if (type == types[2]) point = int.Parse(col[3]);
                    Console.WriteLine("Name: {0}\nType: {1}\nSize: {2}\nPoint: {3}", name, type, size, point);
                }
            }
        }
    }
}
