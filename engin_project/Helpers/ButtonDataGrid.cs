using System.Windows.Forms;

namespace engin_project.Helpers
{
    internal static class ButtonDataGrid
    {
        public static void AgregarBotonesAccion(DataGridView dgv, bool btnDowload = false)
        {
            if (!dgv.Columns.Contains("Editar"))
            {
                DataGridViewButtonColumn btnEditar = new DataGridViewButtonColumn();

                btnEditar.Name = "Editar";
                btnEditar.HeaderText = "Editar";
                btnEditar.Text = "Editar";
                btnEditar.UseColumnTextForButtonValue = true;

                dgv.Columns.Add(btnEditar);
            }

            if (!dgv.Columns.Contains("Eliminar"))
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();

                btnEliminar.Name = "Eliminar";
                btnEliminar.HeaderText = "Eliminar";
                btnEliminar.Text = "Eliminar";
                btnEliminar.UseColumnTextForButtonValue = true;

                dgv.Columns.Add(btnEliminar);
            }

            if (btnDowload && !dgv.Columns.Contains("Descargar"))
            {
                DataGridViewButtonColumn btnDownload = new DataGridViewButtonColumn();

                btnDownload.Name = "Descargar";
                btnDownload.HeaderText = "Descargar";
                btnDownload.Text = "Descargar";
                btnDownload.UseColumnTextForButtonValue = true;

                dgv.Columns.Add(btnDownload);
            }

            dgv.Columns["Editar"].DisplayIndex = dgv.Columns.Count - 2;
            dgv.Columns["Eliminar"].DisplayIndex = dgv.Columns.Count - 1;

            if (btnDowload && dgv.Columns.Contains("Descargar"))
            {
                dgv.Columns["Editar"].DisplayIndex = dgv.Columns.Count - 3;
                dgv.Columns["Eliminar"].DisplayIndex = dgv.Columns.Count - 2;
                dgv.Columns["Descargar"].DisplayIndex = dgv.Columns.Count - 1;
            }
        }
    }
}