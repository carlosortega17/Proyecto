using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto
{
    public partial class FrmCrearDB : Form
    {
        public FrmCrearDB()
        {
            InitializeComponent();
        }

        private void FrmCrearDB_Load(object sender, EventArgs e)
        {
            tbxNombre.Clear();
        }
    }
}
