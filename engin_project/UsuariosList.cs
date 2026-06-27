using engin_project.Models;
using engin_project.Services;
using engin_project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace engin_project
{
    public partial class UsuarioList : Form
    {

        private Panel mainPanel;
        private readonly UsuarioService usuarioService = new UsuarioService();
        private List<UsuarioListado> listaUsuarios = new List<UsuarioListado>();

        public UsuarioList(Panel panel)
        {
            InitializeComponent();

            mainPanel = panel;

            this.Load += UsuarioList_Load;
            dgvUsuarios.CellClick += dgvUsuarios_CellClick;
        }

        private void UsuarioList_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                listaUsuarios = usuarioService.ListarUsuariosActivos();
                dgvUsuarios.DataSource = null;
                dgvUsuarios.DataSource = listaUsuarios;
                PersonalizarGrid();
                ButtonDataGrid.AgregarBotonesAccion(dgvUsuarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void PersonalizarGrid()
        {                                               
            if (dgvUsuarios.Columns.Contains("IdUsuario"))
                dgvUsuarios.Columns["IdUsuario"].Visible = false;

            if (dgvUsuarios.Columns.Contains("IdRol"))
                dgvUsuarios.Columns["IdRol"].Visible = false;

            if (dgvUsuarios.Columns.Contains("Contrasenia"))
                dgvUsuarios.Columns["Contrasenia"].Visible = false;
        }

        

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            int idUsuario = Convert.ToInt32(
                dgvUsuarios.Rows[e.RowIndex].Cells["IdUsuario"].Value
            );

            string columna = dgvUsuarios.Columns[e.ColumnIndex].Name;

            if (columna == "Editar")
            {
                UsuarioListado usuarioSeleccionado = (UsuarioListado)dgvUsuarios.Rows[e.RowIndex].DataBoundItem;
                Usuarios frm = new Usuarios(usuarioSeleccionado);
                frm.TopLevel = false;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Dock = DockStyle.Fill;
                mainPanel.Controls.Clear();
                mainPanel.Controls.Add(frm);
                frm.Show();
            }

            if (columna == "Eliminar")
            {
                DialogResult result = MessageBox.Show(
                    "¿Está seguro de eliminar este usuario?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    bool eliminado = usuarioService.EliminarUsuario(idUsuario);

                    if (eliminado)
                    {
                        MessageBox.Show("Usuario eliminado correctamente.");                        
                        CargarUsuarios();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el usuario.");
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string texto = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(texto))
            {                
                return;
            }

            var filtrados = listaUsuarios.Where(u =>
                (u.Nombres ?? "").ToLower().Contains(texto) ||
                (u.Apellidos ?? "").ToLower().Contains(texto) ||
                (u.Usuario ?? "").ToLower().Contains(texto) ||
                (u.Correo ?? "").ToLower().Contains(texto) ||
                (u.Rol ?? "").ToLower().Contains(texto)
            ).ToList();

            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = filtrados;

            PersonalizarGrid();
            ButtonDataGrid.AgregarBotonesAccion(dgvUsuarios);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
            txtSearch.Clear();
        }
    }
}