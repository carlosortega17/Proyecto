using System;
using System.Windows.Forms;
using Proyecto.Controllers; // Seccion de controladores personalizados.

namespace Proyecto
{
    public partial class FrmMain : Form
    {
        DatabaseManager dbmanager;
        public FrmMain()
        {
            InitializeComponent();
            dbmanager = new DatabaseManager();
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
            }
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            if(tbxCommand.Text.Length > 0) ExecuteCommand(tbxCommand.Text);
        }

        private void lbxDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            dbmanager.Seleccion = lbxDatabases.Items[lbxDatabases.SelectedIndex].ToString();
            MessageBox.Show("La base de datos actual es "+dbmanager.Seleccion, "Base de datos en uso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
