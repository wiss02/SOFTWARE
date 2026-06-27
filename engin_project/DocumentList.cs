using engin_project.Helpers;
using engin_project.Models;
using engin_project.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace engin_project
{
    public partial class DocumentList : Form
    {
        private Panel mainPanel;
        private readonly DocumentoService documentService = new DocumentoService();
        private List<ListadoDocumento> listDocuments = new List<ListadoDocumento>();
        private DownloadDocument downloadDocument;

        public DocumentList(Panel panel)
        {
            InitializeComponent();
            mainPanel = panel;

            this.Load += DocumentList_Load;
            //dgvDocumentos.CellClick += dgvDocumentos_CellClick;
        }

        private void PersonalizarGrid()
        {
            if (dgvDocumentos.Columns.Contains("IdDocumento"))
                dgvDocumentos.Columns["IdDocumento"].Visible = false;

            if (dgvDocumentos.Columns.Contains("IdCategoria"))
                dgvDocumentos.Columns["IdCategoria"].Visible = false;

            if (dgvDocumentos.Columns.Contains("IdUsuario"))
                dgvDocumentos.Columns["IdUsuario"].Visible = false;

            if (dgvDocumentos.Columns.Contains("RutaArchivo"))
                dgvDocumentos.Columns["RutaArchivo"].Visible = false;
        }

        private void CargarDocumentos()
        {
            try
            {
                listDocuments = documentService.ListarDocumentosActivos();
                dgvDocumentos.DataSource = null;
                dgvDocumentos.DataSource = listDocuments;
                PersonalizarGrid();
                ButtonDataGrid.AgregarBotonesAccion(dgvDocumentos, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            CargarDocumentos();
            txtSearch.Clear();
        }

        private void DocumentList_Load(object sender, EventArgs e)
        {
            CargarDocumentos();
        }

        private void dgvDocumentos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (e.RowIndex >= dgvDocumentos.Rows.Count)
                return;

            if (dgvDocumentos.Rows[e.RowIndex].IsNewRow)
                return;

            object valor = dgvDocumentos.Rows[e.RowIndex]
                .Cells["IdDocumento"].Value;

            if (valor == null || valor == DBNull.Value)
                return;

            int idDocument = Convert.ToInt32(valor);

            string columna = dgvDocumentos.Columns[e.ColumnIndex].Name;

            if (columna == "Editar")
            {
                ListadoDocumento documentSelect =
                    (ListadoDocumento)dgvDocumentos.Rows[e.RowIndex].DataBoundItem;

                Documentos frm = new Documentos(documentSelect);

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
                    "¿Está seguro de eliminar este documento?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    bool eliminado =
                        documentService.EliminarDocumento(idDocument);

                    if (eliminado)
                    {
                        MessageBox.Show(
                            "Documento eliminado correctamente."
                        );

                        CargarDocumentos();
                    }
                    else
                    {
                        MessageBox.Show(
                            "No se pudo eliminar el documento."
                        );
                    }
                }
            }

            if (columna == "Descargar")
            {
                string rutaArchivo = dgvDocumentos.Rows[e.RowIndex]
                    .Cells["RutaArchivo"].Value?.ToString();

                if (string.IsNullOrEmpty(rutaArchivo))
                {
                    MessageBox.Show("La ruta del archivo es inválida.");
                    return;
                }

                if (!File.Exists(rutaArchivo))
                {
                    MessageBox.Show("El archivo no existe.");
                    return;
                }

                string nombreArchivo = Path.GetFileName(rutaArchivo);
                string extension = Path.GetExtension(rutaArchivo).ToLower();

                Dictionary<string, int> filtroIndex = new Dictionary<string, int>
                {
                    { ".pdf",  1 },
                    { ".jpg",  2 },
                    { ".jpeg", 3 },
                    { ".png",  4 }
                };

                using (SaveFileDialog save = new SaveFileDialog())
                {
                    save.FileName = nombreArchivo;
                    save.Filter =
                        "PDF (*.pdf)|*.pdf|" +
                        "Imagen JPG (*.jpg)|*.jpg|" +
                        "Imagen JPEG (*.jpeg)|*.jpeg|" +
                        "Imagen PNG (*.png)|*.png";

                    save.DefaultExt = extension.TrimStart('.');
                    save.FilterIndex = filtroIndex.ContainsKey(extension)
                        ? filtroIndex[extension]
                        : 1;

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        File.Copy(rutaArchivo, save.FileName, true);

                        DownloadDocument doc = new DownloadDocument
                        {
                            IdDocumento = idDocument,
                            IdUsuario = 1
                        };

                        bool descarga = documentService.DownloadDocumento(doc);

                        if (descarga)
                        {
                            MessageBox.Show("Documento descargado exitosamente...");
                        }
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

            var filtrados = listDocuments.Where(d =>
                (d.Codigo ?? "").ToLower().Contains(texto) ||
                (d.Titulo ?? "").ToLower().Contains(texto)
            ).ToList();

            dgvDocumentos.DataSource = null;
            dgvDocumentos.DataSource = filtrados;

            PersonalizarGrid();
            ButtonDataGrid.AgregarBotonesAccion(dgvDocumentos, true);
        }
    }
}
