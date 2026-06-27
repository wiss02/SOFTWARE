using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace engin_project
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void AbrirFormulario(Form frm)
        {
            MainPanel.SuspendLayout();

            MainPanel.Controls.Clear();

            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            frm.Visible = false;

            MainPanel.Controls.Add(frm);

            frm.Show();
            frm.Visible = true;

            MainPanel.ResumeLayout();
        }
        
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new Usuarios());
        }

        private void btnListUser_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new UsuarioList(MainPanel));
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new Documentos());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new DocumentList(MainPanel));
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            ChatForm formChat = new ChatForm();
            formChat.ShowDialog();
        }
    }
}
