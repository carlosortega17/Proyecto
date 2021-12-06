using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Proyecto.Controllers
{
    class DatabaseManager
    {
        private const string ayuda = 
            "crea base NOMBRE - Crea una base de datos, el 3er parametro es el nombre\r\n\r\n"+
            "crea tabla NOMBRE (NOMBRE TIPO LONGITUD) - Crea una tabla con una estructura, " +
            "para separar las  columnas se utiliza la coma y la lungitud en decimales puede " +
            "llevar el numero de longitud y su respectiva cantidad de caracters a mostrar separado por un espacio\r\n\r\n"+
            "muestra bases - Muestra las bases de datos creadas\r\n\r\n"+
            "muestra tablas - Muestra las tablas en una base de datos, esto solo funciona al seleccionar una base de datos\r\n\r\n"+
            "borra base NOMBRE - Borra una base de datos\r\n\r\n"+
            "borra tabla NOMBRE - Borra una tabla en una base de datos, debes seleccionar una base de datos para realizar este comando";
        private string SELECCION = null;
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

        public void deleteTable(string tbname)
        {
            try
            {
                string path = DATABASES_PATH + Seleccion + "\\" + tbname+".struct";
                string table = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
                if (File.Exists(path))
                {
                    File.Delete(path);
                    if(File.Exists(table)) File.Delete(table); // Se usa de esta forma ya que no se genera el archivo .table hasta que se agreguen datos
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError: " + ex.Message + "\n\nStackTrace: " + ex.StackTrace);
            }
        }

        public void showTables(TextBox tbx)
        {
            try
            {
                if (Seleccion != null)
                {
                    string path = DATABASES_PATH + Seleccion;
                    if (Directory.Exists(path))
                    {
                        tbx.AppendText("Tablas de la base de datos " + Seleccion + "\r\n");
                        foreach (string file in Directory.GetFiles(path))
                        {
                            string name = Path.GetFileName(file);
                            if (name.Contains(".struct"))
                            {
                                string tname = name.Split(new string[] { ".struct" }, StringSplitOptions.RemoveEmptyEntries)[0];
                                tbx.AppendText("\t- " + tname + "\r\n");
                                tbx.AppendText("Estructura\r\n");
                                tableStruct(readTableStruct(tname), tbx);
                            }
                        }
                    }
                }
                else throw new Exception("Debes seleccionar una base de datos primero");
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

        public void deleteColumn(string tbname, string col)
        {
            try
            {
                if (Seleccion != null)
                {
                    string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".struct";
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(fs);
                    // TABLE
                    string name = reader.ReadString();
                    Int32 columns = reader.ReadInt32();
                    int pointer = -1, counter = 0;
                    ArrayList columns_ = new ArrayList();
                    while (counter < columns)
                    {
                        columns_.Add(reader.ReadString());
                        if (columns_[counter].ToString().Split(' ')[0].Equals(col)) pointer = counter;
                        counter += 1;
                    }
                    reader.Close();
                    fs.Close();
                    // Removemos la columna
                    columns_.RemoveAt(pointer);
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                    BinaryWriter binaryWriter = new BinaryWriter(fs);
                    binaryWriter.Write(name);
                    binaryWriter.Write(columns - 1);
                    foreach (string col_ in columns_)
                    {
                        binaryWriter.Write(col_);
                    }
                    binaryWriter.Flush();
                    binaryWriter.Close();
                    fs.Close();
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine("\n\nError: " + ex.Message + "\n\nStackTrace: " + ex.StackTrace);
            }
        }

        public void addColumn(string struct_)
        {
            try
            {
                if (Seleccion != null)
                {
                    if (struct_.Contains("(") && struct_.Contains(")"))
                    {
                        string[] table = struct_.Split(new char[2] { '(', ')' });
                        string[] command = table[0].Split(' ');
                        string[] columns__ = table[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                        string path = DATABASES_PATH + Seleccion + "\\" + command[2] + ".struct";
                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                        BinaryReader reader = new BinaryReader(fs);
                        // TABLE
                        string name = reader.ReadString();
                        Int32 columns = reader.ReadInt32();
                        int counter = 0, rep = 0;
                        ArrayList columns_ = new ArrayList();
                        while (counter < columns)
                        {
                            columns_.Add(reader.ReadString());
                            counter += 1;
                        }
                        reader.Close();
                        fs.Close();
                        if (rep != 0) throw new Exception("Error, la columna ya existe");
                        // Agregamos las columnas
                        foreach(string col_ in columns__)
                        {
                            columns_.Add(col_);
                        }
                        // verificamos que no existan columnas repetidas
                        string[] temp = new string[columns_.Count];
                        for(int i=0;i<columns_.Count;i++)
                        {
                            temp[i] = columns_[i].ToString();
                        }

                        tableStruct(temp);

                        fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                        BinaryWriter binaryWriter = new BinaryWriter(fs);
                        binaryWriter.Write(name);
                        binaryWriter.Write(columns + 1);
                        foreach (string col_ in columns_)
                        {
                            binaryWriter.Write(col_);
                        }
                        binaryWriter.Flush();
                        binaryWriter.Close();
                        fs.Close();
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

        // Metodos utilizados para las tablas y sus datos
        private string[] readTableStruct(string tbname)
        {
            string[] columns = null;
            if (Seleccion != null)
            {
                string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".struct";
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fs);
                string name = reader.ReadString();
                Int32 numCols = reader.ReadInt32();
                columns = new string[numCols];
                int c = 0;
                while (fs.Position != fs.Length)
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
            string[] types = { "entero", "caracter", "decimal", "fecha" }; // Tipos de datos
            string[] names = new string[colums.Length];
            for (int i = 0; i < names.Length; i++)
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
            if (repetidos > 0) throw new Exception("Nombres de columnas repetidos " + repetidos);

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
                    //Console.WriteLine("Name: {0}\nType: {1}\nSize: {2}", name, type, size);
                }
                else
                {
                    int size = int.Parse(col[2]);
                    int point = 0;
                    if (type == types[2]) point = int.Parse(col[3]);
                    //Console.WriteLine("Name: {0}\nType: {1}\nSize: {2}\nPoint: {3}", name, type, size, point);
                }
            }
        }

        private void tableStruct(string[] colums, TextBox console)
        {
            string[] types = { "entero", "caracter", "decimal", "fecha" }; // Tipos de datos
            string[] names = new string[colums.Length];
            for (int i = 0; i < names.Length; i++)
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
            if (repetidos > 0) throw new Exception("Nombres de columnas repetidos " + repetidos);

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
                    console.AppendText(string.Format("Nombre: {0} | Tipo: {1} | Tamaño: {2}\r\n", name, type, size));
                }
                else
                {
                    int size = int.Parse(col[2]);
                    int point = 0;
                    if (type == types[2]) point = int.Parse(col[3]);
                    console.AppendText(string.Format("Nombre: {0} | Tipo: {1} | Tamaño: {2} | Punto decimal: {3}\r\n", name, type, size, point));
                }
            }
        }

        private void addRowTable(string tbname, string struct_)
        {
            FileStream fs = null;
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            if (!File.Exists(path)) fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            else fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            string[] cols = readTableStruct(tbname);
            // Deconstruccion de la estructur a de la fila
            string[] body = struct_.Split(new char[2] { '(', ')' });
            string[] command = body[0].Split(' ');
            string[] data = body[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            int c = 0;
            foreach (string col in cols)
            {
                string[] str = col.Split(' ');
                if (str.Length == 4)
                {
                    if (str[1].Equals("decimal"))
                    {
                        if (data[c].Length <= int.Parse(str[2])) binaryWriter.Write(double.Parse(data[c]));
                    }
                }
                else
                {
                    if (str[1].Equals("entero"))
                    {
                        if (data[c].Length <= int.Parse(str[2])) binaryWriter.Write(Int32.Parse(data[c]));
                    }
                    else if (str[1].Equals("caracter"))
                    {
                        if (data[c].Length <= int.Parse(str[2])) binaryWriter.Write(data[c]);
                    }
                    else if (str[1].Equals("fecha"))
                    {
                        DateTime dt;
                        if (DateTime.TryParse(data[c], out dt))
                        {
                            if (data[c].Length <= int.Parse(str[2])) binaryWriter.Write(data[c]);
                        }
                    }
                }
                c += 1;
            }
            fs.Close();
        }

        private void viewAllData(string tbname, TextBox console)
        {
            FileStream fs = null;
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            if (File.Exists(path))
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fs);
                string[] cols = readTableStruct(tbname);
                while (fs.Position != fs.Length)
                {
                    int c = 0;
                    foreach (string col in cols)
                    {
                        string[] str = col.Split(' ');
                        if (str.Length == 4)
                        {
                            if (str[1].Equals("decimal"))
                            {
                                string zero = "";
                                for(int t=0;t<int.Parse(str[3]); t++) zero += "0";
                                console.AppendText(string.Format("{0} = {1:"+zero+"}\t", str[0], binaryReader.ReadDouble()));
                            }
                        }
                        else
                        {
                            if (str[1].Equals("entero"))
                            {
                                console.AppendText(string.Format("{0} = {1}\t", str[0], binaryReader.ReadInt32()));
                            }
                            else if (str[1].Equals("caracter"))
                            {
                                console.AppendText(string.Format("{0} = {1}\t", str[0], binaryReader.ReadString()));
                            }
                            else if (str[1].Equals("fecha"))
                            {
                                console.AppendText(string.Format("{0} = {1}\t", str[0], binaryReader.ReadString()));
                            }
                        }
                        c += 1;
                    }
                    console.AppendText("\r\n");
                }
                fs.Close();
            }
        }

        private ArrayList readAllDataTable(string tbname)
        {
            ArrayList tempData = new ArrayList();
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fs);
            string[] cols = readTableStruct(tbname);
            while (fs.Position != fs.Length)
            {
                int c = 0;
                ArrayList temp = new ArrayList();
                foreach (string col in cols)
                {
                    string[] str = col.Split(' ');
                    ArrayList data_ = new ArrayList();
                    data_.Add(str[0]);
                    if (str.Length == 4)
                    {
                        if (str[1].Equals("decimal"))
                        {
                            double value_ = binaryReader.ReadDouble();
                            data_.Add(value_);
                        }
                    }
                    else
                    {
                        if (str[1].Equals("entero"))
                        {
                            Int32 value_ = binaryReader.ReadInt32();
                            data_.Add(value_);
                        }
                        else if (str[1].Equals("caracter"))
                        {
                            string value_ = binaryReader.ReadString();
                            data_.Add(value_);
                        }
                        else if (str[1].Equals("fecha"))
                        {
                            string value_ = binaryReader.ReadString();
                            data_.Add(value_);
                        }
                    }
                    temp.Add(data_);
                    c += 1;
                }
                tempData.Add(temp);
            }
            binaryReader.Close();
            fs.Close();
            return tempData;
        }

        private void deleteRecord(string tbname, string struct_)
        {
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            FileStream fs = null;
            if (File.Exists(path))
            {
                ArrayList tempData = readAllDataTable(tbname);
                int index = -1;
                int rep = 0;
                string[] cmd = struct_.Split('=');
                string value = cmd[1];
                string column = cmd[0].Split(' ')[2];
                do
                {
                    index = -1;
                    for (int i = 0; i < tempData.Count; i++)
                    {
                        var tmp = (ArrayList)tempData[i];
                        for (int j = 0; j < tmp.Count; j++)
                        {
                            var col_ = (ArrayList)tmp[j];
                            Console.WriteLine(col_[1].GetType().Name);
                            if (col_[1].GetType().Name == "String")
                            {
                                
                                if (col_[0].ToString().Equals(column) && col_[1].ToString().Equals(value))
                                {
                                    index = i;
                                }
                            }
                            else if (col_[1].GetType().Name == "Int32")
                            {

                                if (col_[0].ToString().Equals(column) && Int32.Parse(col_[1].ToString()) == Int32.Parse(value))
                                {
                                    index = i;
                                }
                            }
                            else if (col_[1].GetType().Name == "Double")
                            {

                                if (col_[0].ToString().Equals(column) && Double.Parse(col_[1].ToString()) == Double.Parse(value))
                                {
                                    index = i;
                                }
                            }
                        }
                    }
                    if (index != -1) { tempData.RemoveAt(index); rep += 1; }
                } while (index != -1);
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                BinaryWriter binaryWriter = new BinaryWriter(fs);
                for (int i = 0; i < tempData.Count; i++)
                {
                    var tmp = (ArrayList)tempData[i];
                    for (int j = 0; j < tmp.Count; j++)
                    {
                        var col_ = (ArrayList)tmp[j];

                        if (col_[1].GetType().Name == "String") binaryWriter.Write(col_[1].ToString());
                        else if (col_[1].GetType().Name == "Int32") binaryWriter.Write(Int32.Parse(col_[1].ToString()));
                        else if (col_[1].GetType().Name == "Double") binaryWriter.Write(Double.Parse(col_[1].ToString()));
                    }
                }
                binaryWriter.Close();
                fs.Close();
            }
        }

        private void modifyRecord(string tbname, string struct_)
        {
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            string[] table = struct_.Split(new char[2] { '(', ')' });
            string[] command = table[0].Split(' ');
            string[] columns = table[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            string[] condition = table[2].Split('=');
            string target = condition[0].Split(' ')[2];
            string value = condition[1];
            ArrayList tempData = readAllDataTable(tbname);

            FileStream fs = null;
            int index = -1;
            int rp = 0;
            do
            {
                for (int i = 0; i < tempData.Count; i++)
                {
                    var tmp = (ArrayList)tempData[i];
                    for (int j = 0; j < tmp.Count; j++)
                    {
                        var col_ = (ArrayList)tmp[j];
                        if (col_[1].GetType().Name == "String")
                        {
                            if (col_[0].ToString().Equals(target) && col_[1].ToString().Equals(value))
                            {
                                index = i;
                            }
                        }
                        else if (col_[1].GetType().Name == "Int32")
                        {
                            if (col_[0].ToString().Equals(target) && Int32.Parse(col_[1].ToString()) == Int32.Parse(value))
                            {
                                index = i;
                            }
                        }
                        else if (col_[1].GetType().Name == "Double")
                        {
                            if (col_[0].ToString().Equals(target) && Double.Parse(col_[1].ToString()) == Double.Parse(value))
                            {
                                index = i;
                            }
                        }
                    }
                }
                if (index != -1)
                {
                    var temp = (ArrayList)tempData[index];
                    for (int i = 0; i < temp.Count; i++)
                    {
                        foreach (string column in columns)
                        {
                            var col_ = (ArrayList)temp[i];
                            Console.WriteLine("{0} : {1}", col_[0], col_[1]);
                            string[] cmd = column.Split('=');
                            string col = cmd[0];
                            string val = cmd[1];
                            if (col_[0].ToString() == col)
                            {
                                col_[1] = val;
                            }
                        }
                    }
                }
                rp += 1;
            } while (rp < tempData.Count);
            fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            BinaryWriter binaryWriter = new BinaryWriter(fs);
            for (int i = 0; i < tempData.Count; i++)
            {
                var tmp = (ArrayList)tempData[i];
                for (int j = 0; j < tmp.Count; j++)
                {
                    var col_ = (ArrayList)tmp[j];
                    if (col_[1].GetType().Name == "String") binaryWriter.Write(col_[1].ToString());
                    else if (col_[1].GetType().Name == "Int32") binaryWriter.Write(Int32.Parse(col_[1].ToString()));
                    else if (col_[1].GetType().Name == "Double") binaryWriter.Write(Double.Parse(col_[1].ToString()));
                }
            }
            binaryWriter.Close();
            fs.Close();
        }

        private void viewAllDataPerColumns(string tbname, string struct_, TextBox console)
        {
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            if (File.Exists(path))
            {
                string[] table = struct_.Split(new char[2] { '(', ')' });
                string[] command = table[0].Split(' ');
                string[] columns = table[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                ArrayList tempData = readAllDataTable(tbname);
                foreach (ArrayList row in tempData)
                {
                    for (int i = 0; i < row.Count; i++)
                    {
                        ArrayList colInfo = (ArrayList)row[i];
                        for (int t = 0; t < columns.Length; t++)
                        {
                            if (colInfo[0].ToString() == columns[t])
                            {
                                console.AppendText(string.Format("{0} = {1}\t", colInfo[0], colInfo[1]));
                            }
                        }
                    }
                    console.AppendText("\r\n");
                }
            }
        }

        private void viewAllDataPerValue(string tbname, string struct_, TextBox console)
        {
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            if (File.Exists(path))
            {
                string[] table = struct_.Split(new char[2] { '(', ')' });
                string[] command = table[0].Split(' ');
                string[] columns = table[1].Split(new string[1] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                ArrayList tempData = readAllDataTable(tbname);
                foreach (ArrayList row in tempData)
                {
                    bool coincide = false;
                    for (int i = 0; i < row.Count; i++)
                    {
                        
                        ArrayList colInfo = (ArrayList)row[i];
                        for (int t = 0; t < columns.Length; t++)
                        {
                            if (colInfo[1].ToString() == columns[t].Split('=')[1])
                            {
                                coincide = true;
                            }
                        }
                        
                    }
                    for (int i = 0; i < row.Count; i++)
                    {
                        ArrayList colInfo = (ArrayList)row[i];
                        if (coincide)
                        {
                            console.AppendText(string.Format("{0} = {1}\t", colInfo[0], colInfo[1]));
                        }
                    }
                    if(coincide) console.AppendText("\r\n");
                }
            }
        }

        private void viewPerColAndValue(string tbname, string struct_, TextBox console)
        {
            string path = DATABASES_PATH + Seleccion + "\\" + tbname + ".table";
            if (File.Exists(path))
            {
                string[] table = struct_.Split(new char[2] { '(', ')' });
                string[] columns = table[1].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                string[] target = table[3].Split('=');
                string column_ = target[0];
                string value = target[1];
                ArrayList tempData = readAllDataTable(tbname);
                foreach (ArrayList row in tempData)
                {
                    bool coincide = false;
                    for (int i = 0; i < row.Count; i++)
                    {
                        ArrayList colInfo = (ArrayList)row[i];
                        for (int t = 0; t < columns.Length; t++)
                        {
                            if (colInfo[1].ToString() == value)
                            {
                                coincide = true;
                            }
                        }
                    }
                    for (int i = 0; i < row.Count; i++)
                    {
                        ArrayList colInfo = (ArrayList)row[i];
                        if (coincide)
                        {
                            for (int t = 0; t < columns.Length; t++)
                            {
                                if (colInfo[0].ToString() == columns[t])
                                {
                                    console.AppendText(string.Format("{0} = {1}\t", colInfo[0], colInfo[1]));
                                }
                            }
                        }
                    }
                    if (coincide) console.AppendText("\r\n");
                }
            }
        }

        public void execute(string cmd, TextBox console)
        {
            string[] commands = cmd.Split(' ');
            try
            {

                if (commands.Length > 0)
                {
                    if (commands[0].Equals("crea")) // Comandos que inician con crea
                    {
                        if (commands[1].Equals("base") && commands.Length == 3) // Para crear base de datos
                        {
                            createDatabase(commands[2]);
                            console.AppendText(string.Format("Se creo la base de datos {0}\r\n", commands[2]));
                        }
                        else if (commands[1].Equals("tabla") && commands.Length > 2) // Para crear tablas
                        {
                            if (Seleccion != null)
                            {
                                try
                                {
                                    createTable(cmd);
                                    console.AppendText(string.Format("Se creo la tabla {0}\r\n", commands[2]));
                                }
                                catch (Exception ex)
                                {
                                    console.AppendText(string.Format("{0}\r\n", ex.Message));
                                }
                            }
                            else
                            {
                                console.AppendText("No se selecciono ninguna base de datos\r\n");
                            }
                        }
                        else if (commands[1].Equals("campo") && commands.Length > 2)
                        {
                            addColumn(cmd);
                            console.AppendText(string.Format("Se creo la columna \r\n"));
                        }
                    }
                    else if (commands[0].Equals("borra"))
                    {
                        if (commands[1].Equals("base") && commands.Length == 3)
                        {
                            deleteDatabase(commands[2]);
                            console.AppendText(string.Format("Se elimino la base de datos {0}\r\n", commands[2]));
                        }
                        else if (commands[1].Equals("tabla") && commands.Length == 3)
                        {
                            try
                            {
                                deleteTable(commands[2]);
                                console.AppendText(string.Format("Se elimino la tabla {0}\r\n", commands[2]));
                            }
                            catch (Exception ex)
                            {
                                console.AppendText(string.Format("{0}\r\n", ex.Message));
                            }
                        }
                        else if (commands[1].Equals("campo") && commands.Length == 4)
                        {
                            deleteColumn(commands[2], commands[3]);
                            console.AppendText("Se elimino la columna "+commands[3]+" de la tabla "+commands[2]+"\r\n");
                        }
                    }
                    else if (commands[0].Equals("muestra"))
                    {
                        if (commands[1].Equals("bases") && commands.Length == 2)
                        {
                            console.AppendText("Bases de datos\r\n");
                            foreach (var dir in listDatabase())
                            {
                                console.AppendText("\t- " + dir.Name + "\r\n");
                            }
                        }
                        else if (commands[1].Equals("tablas") && commands.Length == 2)
                        {
                            try
                            {
                                showTables(console);
                            }
                            catch (Exception ex)
                            {
                                console.AppendText(string.Format("{0}\r\n", ex.Message));
                            }
                        }
                    }
                    else if (commands[0].Equals("lista"))
                    {
                        Console.WriteLine(commands[1]);
                        if (commands[1].Equals("*"))
                        {
                            if (commands.Length == 3) viewAllData(commands[2], console);
                            else viewAllDataPerValue(commands[2], cmd, console);
                        }
                        else if (commands[1].Contains("("))
                        {
                            string[] params_ = cmd.Split(new char[] { '(', ')' });
                            string table_name = params_[2].Split(' ')[1];
                            viewPerColAndValue(table_name, cmd, console);
                        }
                        else viewAllDataPerColumns(commands[1], cmd, console);
                    }
                    else if (commands[0].Equals("elimina"))
                    {
                        try
                        {
                            deleteRecord(commands[1], cmd);
                            console.AppendText("Se elimino correctamente la fila en " + commands[1] + "\r\n");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (commands[0].Equals("inserta"))
                    {
                        try
                        {
                            addRowTable(commands[1], cmd);
                            console.AppendText("Se inserto correctamente una fila en " + commands[1] + "\r\n");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (commands[0].Equals("modifica"))
                    {
                        try
                        {
                            modifyRecord(commands[1], cmd);
                            console.AppendText("Se modifico correctamente la tabla " + commands[1] + "\r\n");
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (commands[0].Equals("usa"))
                    {
                        if (commands[1].Equals("base") && commands.Length == 3)
                        {
                            Seleccion = commands[2];
                            console.AppendText(string.Format("Se selecciono la base de datos {0}\r\n", Seleccion));
                        }
                    }
                    else if (commands[0].Equals("describir"))
                    {
                        readTableStruct(commands[1]);
                    }
                    // Comandos para controlar la "Consola"
                    else if (commands[0].StartsWith("salir"))
                    {
                        Application.Exit(); // Cerrar el programa
                    }
                    else if (commands[0].StartsWith("ayuda"))
                    {
                        console.AppendText("\r\n"); // Agragamos un salto de linea
                        console.AppendText(ayuda + "\r\n"); // Mostramos el texto de ayuda
                    }
                    else if (commands[0].StartsWith("limpia"))
                    {
                        console.Clear(); // Limpiamos la "consola"
                    }
                    else
                    {
                        console.AppendText("Comando no encontrado, pruebe con: ayuda\r\n");
                    }

                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
