using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _26_reservaciones
{
    /// <summary>
    /// Lógica de interacción para Habitaciones.xaml
    /// </summary>
    public partial class Habitaciones : Window
    {
        //Variable miembro
        private Habitacion habitacion = new Habitacion();
        private List<Habitacion> habitaciones;

        public Habitaciones()
        {
            InitializeComponent();

            //Llenar el combobox de estado de las habitacion
            cmbEstado.ItemsSource = Enum.GetValues(typeof(EstadosHabitacion));

            //Llenar el listbox de habitaciones
            ObtenerHabitaciones();
        }

        private void ObtenerValoresFormulario()
        {
            habitacion.Descripcion = txtDescripcion.Text;
            habitacion.Numero = Convert.ToInt32(txtNumeroHabitacion.Text);
            habitacion.Estado = (EstadosHabitacion)cmbEstado.SelectedValue;
        }

        private void LimpiarFormulario()
        {
            txtDescripcion.Text = string.Empty;
            txtNumeroHabitacion.Text = string.Empty;
            cmbEstado.SelectedValue = null;
        }

        private void ObtenerHabitaciones()
        {
            habitaciones = habitacion.MonstrarHabitaciones();
            lbHabitaciones.DisplayMemberPath = "Descripcion";
            lbHabitaciones.SelectedValuePath = "Id";
            lbHabitaciones.ItemsSource = habitaciones;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            //Verificar que se ingresaron los valores requeridos
            if (txtDescripcion.Text == string.Empty || txtNumeroHabitacion.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
            }
            else if (cmbEstado.SelectedValue == null)
                MessageBox.Show("Por favor selecciona el estado de la habitacion");
            else
            {
                try
                {
                    //Obtener los valores para la habitacion
                    ObtenerValoresFormulario();

                    //Insertar los datos de la habitacion
                    habitacion.CrearHabitacion(habitacion);

                    //Mensaje de inserccion exito
                    MessageBox.Show("Datos insertados correctamente");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de insertar la habitacion....");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    LimpiarFormulario();
                    ObtenerHabitaciones();
                }
            }
        }

    }
}
