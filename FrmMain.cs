using System;
using System.Windows.Forms;
using Proyecto.Controllers; // Seccion de controladores personalizados.

namespace Proyecto
{
    public partial class FrmMain : Form
    {
        FrmCrearDB crearDB;
        DatabaseManager dbmanager;
        public FrmMain()
        {
            InitializeComponent();
            dbmanager = new DatabaseManager();
            crearDB = new FrmCrearDB();
            LoadDatabasesList();
        }

        private void LoadDatabasesList()
        {
            lbxDatabases.Items.Clear();
            var list = dbmanager.listDatabase();
            for (int i = 0; i < list.Length; i++)
            {
                lbxDatabases.Items.Add(list[i].Name);
            }
        }

        private void ExecuteCommand(string command)
        {
            dbmanager.execute(command, tbxConsole);
            LoadDatabasesList();
        }

        private void Test()
        {
            
            dbmanager.createDatabase("Ejemplo");
            var list = dbmanager.listDatabase();
            for (int i = 0; i < list.Length; i++)
            {
                Console.WriteLine(list[i].Name);
            }
        }

        private void tbxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (tbxCommand.Text.Length > 0) ExecuteCommand(tbxCommand.Text);
                tbxCommand.Clear();
            }
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            if(tbxCommand.Text.Length > 0) ExecuteCommand(tbxCommand.Text);
            tbxCommand.Clear();
        }

        private void lbxDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbmanager.Seleccion = lbxDatabases.Items[lbxDatabases.SelectedIndex].ToString();
            tbxConsole.AppendText("La base de datos actual es: " + dbmanager.Seleccion + "\r\n");
        }

        private void nuevaBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(crearDB.ShowDialog() == DialogResult.OK)
            {
                dbmanager.createDatabase(crearDB.tbxNombre.Text);
                LoadDatabasesList();
            }
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FrmAyuda().ShowDialog(this);
        }
    }
}
