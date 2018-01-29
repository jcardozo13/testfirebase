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
    public partial class RegistrarVehiculo : Form
    {
        public RegistrarVehiculo()
        {
            InitializeComponent();

        }

        public accion tipoAccion;
        public cars vh;
        public int count;
        public enum accion
        {
            guardar =1,
            actualizar =2
        }

        public string token;
        public string localId;

        public void cargarDatos()
        {
             txtTipo.Text = vh.classType;
             txtNombre.Text = vh.name;
             txtPuertas.Text= vh.doors.ToString();
             txtTrasmision.Text= vh.transmission;
             ckHas.Checked= vh.hasAC;
             txtPrecio.Text= vh.price.ToString();
             txtSeats.Text =  vh.seats;
             txtImagen.Text = vh.imageUrl;
             txtluggage.Text = vh.luggage.ToString();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (tipoAccion == accion.guardar)
            {
                Guardar();
            }
            else
            {
                Actualizar();
            }
        }

        private async void Guardar()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtPuertas.Text) && !string.IsNullOrEmpty(txtPrecio.Text) && !string.IsNullOrEmpty(txtluggage.Text))
                {
                    vh = new cars();
                    setDatos();

                    FirebaseClient client = new FirebaseClient(GLOBAL.baseUrl, new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token)
                    });
                    var result = await client.Child("users/" + localId + "/cars").PostAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vh));
                    MessageBox.Show("Guardado correctamente!", "Vehiculos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Diligencie los datos solicitados.", "Vehiculos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNombre.Focus();
                }
            }
            catch (FirebaseAuthException e)
            {
                MessageBox.Show("Ocurrio un error al intentar registrar un vehiculo error: " + e.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           

        }

        private void setDatos()
        {
            vh.classType = txtTipo.Text.Trim();
            vh.name = txtNombre.Text.Trim();
            vh.doors = int.Parse(txtPuertas.Text);
            vh.transmission = txtTrasmision.Text.Trim();
            vh.hasAC = ckHas.Checked;
            vh.price = int.Parse(txtPrecio.Text);
            vh.seats = txtSeats.Text.Trim();
            vh.imageUrl = txtImagen.Text.Trim();
            vh.luggage = int.Parse(txtluggage.Text);
            if (tipoAccion == accion.guardar)
            {
                vh.id = "car" + (count + 1);
            }              
        }
        private async void Actualizar()
        {
            setDatos();

             FirebaseClient client = new FirebaseClient(GLOBAL.baseUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token)
            });
            await client.Child("users/" + localId + "/cars").PutAsync(Newtonsoft.Json.JsonConvert.SerializeObject(vh));
            MessageBox.Show("Actualizado correctamente!", "Vehiculos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }
        private void RegistrarVehiculo_Load(object sender, EventArgs e)
        {
            if (tipoAccion == accion.actualizar)
            {
                btnGuardar.Text = "Actualizar";
            }
        }

        private void txt_KeyPress (object sender, KeyPressEventArgs e) 
        {

            TextBox b = (TextBox)sender;
            
            if (Char.IsDigit(e.KeyChar))
            {              
                e.Handled = false;
            }
            else
                        if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
            {
                e.Handled = false;
            }
            else
            {
                //el resto de teclas pulsadas se desactivan 
                e.Handled = true;
            }
        }

        private void txtluggage_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_KeyPress(sender, e);
        }

        private void txtPuertas_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_KeyPress(sender, e);
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            txt_KeyPress(sender, e);
        }
    }
}
