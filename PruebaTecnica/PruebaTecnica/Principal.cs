using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database.Query;
using Firebase.Database;

namespace PruebaTecnica
{
    public partial class Principal : Form
    {
        public Principal()
        {
            InitializeComponent();
        }

       
        public string localId { get; set; }
        public string email { get; set; }
        public string token;
        private int count;

        private void registrarUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegistroUsuario fr = new RegistroUsuario();
            fr.Show();
        }

        private void recuperarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecuperarContraseña fr = new RecuperarContraseña();
            fr.Show();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            cargarVehiculos();
        }
        private async void  cargarVehiculos()
        {
            FirebaseClient client = new FirebaseClient(GLOBAL.baseUrl, new FirebaseOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(token)
            });
            var result = await client.Child("users/" + localId + "/cars").OnceAsync<cars>();

            List<cars> carlist = new List<cars>();

            foreach (var car in result)
            {
                cars caradd = new cars();
                caradd.classType = car.Object.classType;
                caradd.doors = car.Object.doors;
                caradd.hasAC = car.Object.hasAC;
                caradd.id = car.Key.ToString();
                caradd.name = car.Object.name;
                caradd.transmission = car.Object.transmission;
                caradd.price = car.Object.price;
                caradd.imageUrl = car.Object.imageUrl;
                caradd.seats = car.Object.seats;
                caradd.luggage = car.Object.luggage;
                carlist.Add(caradd);
            }

            count = carlist.Count();
            dataGridView1.DataSource = carlist;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.ColumnIndex == 0 && senderGrid.Columns[e.ColumnIndex].Name == "eliminar")
            {
                string idVehiculo = senderGrid.Rows[e.RowIndex].Cells[5].Value.ToString();
                eliminarVehiculo(idVehiculo);

            } if(e.ColumnIndex == 1 && senderGrid.Columns[e.ColumnIndex].Name == "edit")
            {
                cars vh = new cars();
                vh.id = senderGrid.Rows[e.RowIndex].Cells["id"].Value.ToString();
                if (senderGrid.Rows[e.RowIndex].Cells["Type"].Value != null)
                {
                    vh.classType = senderGrid.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                }             
                vh.name = senderGrid.Rows[e.RowIndex].Cells["name"].Value.ToString();
                vh.transmission = senderGrid.Rows[e.RowIndex].Cells["transmission"].Value.ToString();
                vh.hasAC = (bool)senderGrid.Rows[e.RowIndex].Cells["hasAC"].Value;
                vh.doors = (int)senderGrid.Rows[e.RowIndex].Cells["doors"].Value;
                vh.seats = senderGrid.Rows[e.RowIndex].Cells["seats"].Value.ToString();
                vh.imageUrl = senderGrid.Rows[e.RowIndex].Cells["imageUrl"].Value.ToString();
                vh.price = (int)senderGrid.Rows[e.RowIndex].Cells["price"].Value;
                vh.luggage = (int)senderGrid.Rows[e.RowIndex].Cells["luggage"].Value;
                actualizarVehiculo(vh);
            }

        }
        private async void eliminarVehiculo(string id)
        {
            if (MessageBox.Show("Seguro desea eliminar el vehiculo?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                FirebaseClient client = new FirebaseClient(GLOBAL.baseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(token)
                });
                await client.Child("users/" + localId + "/cars").Child(id).DeleteAsync();
                cargarVehiculos();
            }
            
        }
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoVehiculo();
        }

        private void actualizarVehiculo(cars vh)
        {
            RegistrarVehiculo fr = new RegistrarVehiculo();
            fr.vh = vh;
            fr.token = token;
            fr.localId = localId;
            fr.tipoAccion = RegistrarVehiculo.accion.actualizar;
            fr.cargarDatos();
            fr.Show();
        }
        private void NuevoVehiculo()
        {
            RegistrarVehiculo fr = new RegistrarVehiculo();
            cargarVehiculos();
            fr.token = token;
            fr.localId = localId;
            fr.tipoAccion = RegistrarVehiculo.accion.guardar;
            fr.count = count;
            fr.Show();
            
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarVehiculos();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login fr = new Login();
            fr.Show();
        }
    }

}
