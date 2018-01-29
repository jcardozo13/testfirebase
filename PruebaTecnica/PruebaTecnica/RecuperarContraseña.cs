using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebaTecnica
{
    public partial class RecuperarContraseña : Form
    {
        public RecuperarContraseña()
        {
            InitializeComponent();
        }

        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            reset();
        }

        private async void reset()
        {
            try
            {
                FirebaseConfig config = new FirebaseConfig(GLOBAL.apiKey);

                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(config);

                await authProvider.SendPasswordResetEmailAsync(txtCorreo.Text.Trim());
                MessageBox.Show("verifique su correo electronico", "Recuperar contraseña", MessageBoxButtons.OK, MessageBoxIcon.Information);        
                this.Close();
            }
            catch (FirebaseAuthException e)
            {
                MessageBox.Show("Ocurrio un error: " + e.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
