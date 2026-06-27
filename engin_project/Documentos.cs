using engin_project.Models;
using engin_project.Services;
using System;
using System.IO;
using System.Windows.Forms;

namespace engin_project
{
    public partial class Documentos : Form
    {

        private string rutaArchivoSeleccionado = "";
        private DocumentoService documentService = new DocumentoService();
        private ListadoDocumento documentEditar;
        public Documentos()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        public Documentos(ListadoDocumento documento)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            documentEditar = documento;
            txtCodigo.Text = documento.Codigo;
            txtTitulo.Text = documento.Titulo;
            txtDescripcion.Text = documento.Descripcion;
            txtFilePath.Text = documento.RutaArchivo;
            dtFechaDoc.Value = Convert.ToDateTime(documento.FechaDocumento);
        }

        private void LimpiarFormulario()
        {
            txtTitulo.Clear();
            txtCodigo.Clear();
            txtDescripcion.Clear();

            txtFilePath.Clear();

            cmbCategoria.SelectedIndex = -1;

            dtFechaDoc.Value = DateTime.Today;

            rutaArchivoSeleccionado = "";

            txtTitulo.Focus();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                
                if (cmbCategoria.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.");
                    return;
                }                

                bool resultado;

                if (documentEditar == null)
                {
                    string rutaGuardada = GuardarArchivo();

                    if (rutaGuardada == "")
                    {
                        MessageBox.Show("El archivo no se guardó, intente nuevamente.");
                        return;
                    }

                    ListadoDocumento doc = new ListadoDocumento
                    {
                        IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                        IdUsuario = 1,
                        Titulo = txtTitulo.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Codigo = txtCodigo.Text.Trim(),
                        FechaDocumento = dtFechaDoc.Value.ToString("yyyy-MM-dd"),
                        RutaArchivo = rutaGuardada
                    };

                    resultado = documentService.InsertarDocumento(doc);

                    LLMService llm = new LLMService();

                    var response =
                        await llm.GenerarEmbeddings(
                            rutaGuardada                          
                        );

                    if (resultado)
                    {
                        MessageBox.Show("Documento registrado correctamente.");
                        LimpiarFormulario();
                    }
                }
                else
                {
                    string rutaArchivoFinal = documentEditar.RutaArchivo;

                    if (!string.IsNullOrWhiteSpace(rutaArchivoSeleccionado))
                    {
                        rutaArchivoFinal = GuardarArchivo();

                        if (rutaArchivoFinal == "")
                        {
                            MessageBox.Show("El archivo no se guardó, intente nuevamente.");
                            return;
                        }
                    }

                    ListadoDocumento doc = new ListadoDocumento
                    {
                        IdCategoria = Convert.ToInt32(cmbCategoria.SelectedValue),
                        IdUsuario = documentEditar.IdUsuario,
                        Titulo = txtTitulo.Text.Trim(),
                        Descripcion = txtDescripcion.Text.Trim(),
                        Codigo = txtCodigo.Text.Trim(),
                        FechaDocumento = dtFechaDoc.Value.ToString("yyyy-MM-dd"),
                        RutaArchivo = rutaArchivoFinal
                    };

                    resultado = documentService.ActualizarDocumento(documentEditar.IdDocumento, doc);

                    if (resultado)
                    {
                        MessageBox.Show("Documento actualizado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CargarCategoriasDocumentos()
        {
            try
            {
                var listaCategorias = documentService.ObtenerCategorias();

                cmbCategoria.DataSource = listaCategorias;
                cmbCategoria.DisplayMember = "Nombre";
                cmbCategoria.ValueMember = "IdCategoria";
                cmbCategoria.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar roles: " + ex.Message);
            }
        }

        private void Documentos_Load(object sender, EventArgs e)
        {
            CargarCategoriasDocumentos();
            if (documentEditar != null)
            {
                cmbCategoria.SelectedValue = documentEditar.IdCategoria;
            }
        }

        private string GuardarArchivo()
        {
            if (string.IsNullOrWhiteSpace(rutaArchivoSeleccionado))
            {
                MessageBox.Show("Debe seleccionar un archivo.");
                return "";
            }

            string carpetaDestino = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "engin_project",
                "files_received"
            );

            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            string nombreArchivo = Path.GetFileName(rutaArchivoSeleccionado);
            string rutaFinal = Path.Combine(carpetaDestino, nombreArchivo);

            File.Copy(rutaArchivoSeleccionado, rutaFinal, true);

            return rutaFinal;
        }


        private void seleccionarArchivo() {

            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Seleccionar archivo";
            ofd.Filter = "Archivos permitidos|*.pdf;*.png;*.jpg;*.jpeg|" + "PDF|*.pdf|" + "Imágenes|*.png;*.jpg;*.jpeg";

            ofd.Multiselect = false;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                rutaArchivoSeleccionado = ofd.FileName;
                txtFilePath.Text = rutaArchivoSeleccionado;
            }
        }
        private void btnFile_Click(object sender, EventArgs e)
        {
            seleccionarArchivo();
        }

        private async void btnSubidaRapida_Click(
            object sender,
            EventArgs e
        )
        {
            try
            {
                btnSubidaRapida.Enabled = false;
                string textoOriginal =
                    btnSubidaRapida.Text;

                btnSubidaRapida.Text =
                    "Procesando...";

                seleccionarArchivo();

                string ruta = GuardarArchivo();

                if (string.IsNullOrEmpty(ruta))
                    return;

                LLMService llm = new LLMService();

                var response =
                    await llm.EnviarDocumento(
                        ruta,
                        1,
                        1
                    );

                if (response.success)
                {
                    MessageBox.Show(
                        response.message
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Error: " +
                        response.message
                    );
                }

                btnSubidaRapida.Text =
                    textoOriginal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message
                );
            }
            finally
            {
                btnSubidaRapida.Enabled = true;
                btnSubidaRapida.Text =
                    "Subida rápida";
                LimpiarFormulario();
            }
        }
    }
}
