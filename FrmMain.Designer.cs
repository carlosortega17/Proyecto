
namespace Proyecto
{
    partial class FrmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.mnsMenu = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevaBaseDeDatosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbxConsole = new System.Windows.Forms.TextBox();
            this.btnCommand = new System.Windows.Forms.Button();
            this.tbxCommand = new System.Windows.Forms.TextBox();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lbxDatabases = new System.Windows.Forms.ListBox();
            this.mnsMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnsMenu
            // 
            this.mnsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.mnsMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsMenu.Name = "mnsMenu";
            this.mnsMenu.Size = new System.Drawing.Size(784, 24);
            this.mnsMenu.TabIndex = 0;
            this.mnsMenu.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevaBaseDeDatosToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.archivoToolStripMenuItem.Text = "Opciones";
            // 
            // nuevaBaseDeDatosToolStripMenuItem
            // 
            this.nuevaBaseDeDatosToolStripMenuItem.Name = "nuevaBaseDeDatosToolStripMenuItem";
            this.nuevaBaseDeDatosToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.nuevaBaseDeDatosToolStripMenuItem.Text = "Nueva base de datos";
            this.nuevaBaseDeDatosToolStripMenuItem.Click += new System.EventHandler(this.nuevaBaseDeDatosToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            this.ayudaToolStripMenuItem.Click += new System.EventHandler(this.ayudaToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.tbxConsole);
            this.panel1.Controls.Add(this.btnCommand);
            this.panel1.Controls.Add(this.tbxCommand);
            this.panel1.Controls.Add(this.pnlLeft);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 537);
            this.panel1.TabIndex = 1;
            // 
            // tbxConsole
            // 
            this.tbxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxConsole.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbxConsole.ForeColor = System.Drawing.Color.Lime;
            this.tbxConsole.Location = new System.Drawing.Point(206, 30);
            this.tbxConsole.Multiline = true;
            this.tbxConsole.Name = "tbxConsole";
            this.tbxConsole.ReadOnly = true;
            this.tbxConsole.Size = new System.Drawing.Size(566, 495);
            this.tbxConsole.TabIndex = 3;
            // 
            // btnCommand
            // 
            this.btnCommand.Location = new System.Drawing.Point(697, 3);
            this.btnCommand.Name = "btnCommand";
            this.btnCommand.Size = new System.Drawing.Size(75, 23);
            this.btnCommand.TabIndex = 2;
            this.btnCommand.Text = "Executar";
            this.btnCommand.UseVisualStyleBackColor = true;
            this.btnCommand.Click += new System.EventHandler(this.btnCommand_Click);
            // 
            // tbxCommand
            // 
            this.tbxCommand.Location = new System.Drawing.Point(206, 3);
            this.tbxCommand.Name = "tbxCommand";
            this.tbxCommand.Size = new System.Drawing.Size(485, 21);
            this.tbxCommand.TabIndex = 1;
            this.tbxCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxCommand_KeyDown);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.lbxDatabases);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(200, 537);
            this.pnlLeft.TabIndex = 0;
            // 
            // lbxDatabases
            // 
            this.lbxDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxDatabases.FormattingEnabled = true;
            this.lbxDatabases.ItemHeight = 16;
            this.lbxDatabases.Location = new System.Drawing.Point(0, 0);
            this.lbxDatabases.Name = "lbxDatabases";
            this.lbxDatabases.Size = new System.Drawing.Size(200, 537);
            this.lbxDatabases.TabIndex = 0;
            this.lbxDatabases.SelectedIndexChanged += new System.EventHandler(this.lbxDatabases_SelectedIndexChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mnsMenu);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsMenu;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.mnsMenu.ResumeLayout(false);
            this.mnsMenu.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnsMenu;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem nuevaBaseDeDatosToolStripMenuItem;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.ListBox lbxDatabases;
        private System.Windows.Forms.Button btnCommand;
        private System.Windows.Forms.TextBox tbxCommand;
        private System.Windows.Forms.TextBox tbxConsole;
    }
}

