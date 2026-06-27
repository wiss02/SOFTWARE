using engin_project.Models;
using engin_project.Services;
using System;
using System.Windows.Forms;


namespace engin_project
{    
    public partial class Usuarios : Form
    {
        private UsuarioService usuarioService = new UsuarioService();
        private UsuarioListado usuarioEditar;
        public Usuarios()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }


        public Usuarios(UsuarioListado usuario)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            usuarioEditar = usuario;

            txtName.Text = usuario.Nombres;
            txtLastName.Text = usuario.Apellidos;
            txtUser.Text = usuario.Usuario;
            txtEmail.Text = usuario.Correo;
            txtPwd.Text = usuario.Contrasenia;
            
        }

        private void LimpiarFormulario()
        {
            txtName.Clear();
            txtLastName.Clear();
            txtUser.Clear();
            txtPwd.Clear();
            txtEmail.Clear();
            cmbRol.SelectedIndex = -1;
            txtName.Focus();
        }

        private void Usuarios_Load(object sender, EventArgs e)
        {
            CargarRoles();
            if (usuarioEditar != null)
            {
                cmbRol.SelectedValue = usuarioEditar.IdRol;
            }
        }

        private void CargarRoles()
        {
            try
            {
                var listaRoles = usuarioService.ObtenerRoles();

                cmbRol.DataSource = listaRoles;
                cmbRol.DisplayMember = "Nombre";
                cmbRol.ValueMember = "IdRol";
                cmbRol.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRol.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un rol.");
                    return;
                }

                bool resultado; 

                if (usuarioEditar == null)
                {
                    resultado = usuarioService.InsertarUsuario(
                        Convert.ToInt32(cmbRol.SelectedValue),
                        txtName.Text.Trim(),
                        txtLastName.Text.Trim(),
                        txtUser.Text.Trim(),
                        txtPwd.Text.Trim(),
                        txtEmail.Text.Trim()
                    );

                    if (resultado)
                    {
                        MessageBox.Show("Usuario registrado correctamente.");
                        LimpiarFormulario();
                    }
                }
                else
                {
                    resultado = usuarioService.ActualizarUsuario(
                        usuarioEditar.IdUsuario,
                        Convert.ToInt32(cmbRol.SelectedValue),
                        txtName.Text.Trim(),
                        txtLastName.Text.Trim(),
                        txtUser.Text.Trim(),
                        txtPwd.Text.Trim(),
                        txtEmail.Text.Trim()
                    );

                    if (resultado)
                    {
                        MessageBox.Show("Usuario actualizado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
