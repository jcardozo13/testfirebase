using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Auth;
using Firebase.Database;

namespace PruebaTecnica
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            Sign();
        }
        private void Sign()
        {
            string email = null;
            string pass = null;
            if (!string.IsNullOrEmpty(txtCorreo.Text) && !string.IsNullOrEmpty(txtContraseña.Text))
            {
                email = txtCorreo.Text.Trim();
                pass = txtContraseña.Text.Trim();

                Autenticar(email, pass);
                                               
            }
            else
            {
                txtCorreo.Focus();
                MessageBox.Show("Los datos solicitados son obligatorios.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }      

        }

        private async void Autenticar(string email, string pass)
        {
            try
            {
                FirebaseConfig config = new FirebaseConfig(GLOBAL.apiKey);

                FirebaseAuthProvider authProvider = new FirebaseAuthProvider(config);

                FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, pass);
                
                this.Hide();
                Principal main = new Principal();
                main.localId = auth.User.LocalId;
                main.token = auth.FirebaseToken;
                main.email = email;
                main.Show();
               
            }
            catch (FirebaseAuthException e)
            {
                MessageBox.Show("Ocurrio un error: " + e.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }

        }

     }
}
