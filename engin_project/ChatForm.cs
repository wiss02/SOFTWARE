using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace engin_project
{
    public partial class ChatForm : Form
    {

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _chatUrl = "http://localhost:8000/chat";

        public ChatForm()
        {
            InitializeComponent();
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            rtbChat.BackColor = Color.FromArgb(72, 61, 139);
            rtbChat.ForeColor = Color.FromArgb(240, 240, 240);
            btnEnviar.Click += btnEnviar_Click;
            txtPregunta.KeyDown += txtPregunta_KeyDown;
        }

        private async void btnEnviar_Click(object sender, EventArgs e)
        {
            string pregunta = txtPregunta.Text.Trim();

            if (string.IsNullOrEmpty(pregunta))
                return;

            AgregarMensaje("Tú", pregunta, Color.FromArgb(135, 206, 235));

            txtPregunta.Clear();
            btnEnviar.Enabled = false;
            AgregarMensaje("Asistente", "Escribiendo...", Color.FromArgb(180, 180, 180));

            try
            {
                using (MultipartFormDataContent form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(pregunta), "pregunta");

                    HttpResponseMessage response = await _httpClient.PostAsync(_chatUrl, form);
                    string json = await response.Content.ReadAsStringAsync();

                    var resultado = JsonConvert.DeserializeObject<ChatResponse>(json);

                    EliminarUltimoMensaje();

                    if (resultado.success)
                        AgregarMensaje("Asistente", resultado.respuesta, Color.FromArgb(144, 238, 144));
                    else
                        AgregarMensaje("Asistente", "Ocurrió un error al obtener respuesta.", Color.FromArgb(255, 99, 71));
                }
            }
            catch (Exception ex)
            {
                EliminarUltimoMensaje();
                AgregarMensaje("Asistente", $"Error: {ex.Message}", Color.FromArgb(255, 99, 71));
            }
            finally
            {
                btnEnviar.Enabled = true;
                txtPregunta.Focus();
            }
        }

        private void txtPregunta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnEnviar_Click(sender, e);
            }
        }

        private void AgregarMensaje(string remitente, string mensaje, Color color)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;

            rtbChat.SelectionColor = color;
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Bold);
            rtbChat.AppendText($"{remitente}:\n");

            rtbChat.SelectionColor = Color.FromArgb(240, 240, 240);
            rtbChat.SelectionFont = new Font("Segoe UI", 10, FontStyle.Regular);
            rtbChat.AppendText($"{mensaje}\n\n");

            rtbChat.ScrollToCaret();
        }

        private void EliminarUltimoMensaje()
        {
            string texto = rtbChat.Text;
            int ultimoRemitente = texto.LastIndexOf("Asistente:\n");

            if (ultimoRemitente >= 0)
            {
                rtbChat.Select(ultimoRemitente, texto.Length - ultimoRemitente);
                rtbChat.SelectedText = "";
            }
        }

        public class ChatResponse
        {
            public bool success { get; set; }
            public string respuesta { get; set; }
            public string pregunta { get; set; }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
