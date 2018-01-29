using Firebase.Auth;
using Firebase.Database;
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
    public partial class RegistroUsuario : Form
    {
        public RegistroUsuario()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtContraseña.Text.Length >= 6)
            {
                if (!string.IsNullOrEmpty(txtCorreo.Text) && !string.IsNullOrEmpty(txtContraseña.Text))
                {
                    guardarUsuario();
                }
            }
            else
            {
                MessageBox.Show("la contraseña debe tener minimo 6 caracteres", "informacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void guardarUsuario()
        {
            try
            {
                FirebaseConfig config = new FirebaseConfig(GLOBAL.apiKey);

                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(config);

                FirebaseAuthLink auth = await authProvider.CreateUserWithEmailAndPasswordAsync(txtCorreo.Text, txtContraseña.Text);

                MessageBox.Show("Usuario registrado correctamente!", "Registro usuarios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                
            }
            catch (FirebaseAuthException e)
            {
                MessageBox.Show("Ocurrio un error: " + e.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }   
}
