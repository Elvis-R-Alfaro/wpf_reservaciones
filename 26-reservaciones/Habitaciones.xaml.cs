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
            habitacion.Id = Convert.ToInt32(lbHabitaciones.SelectedValue);
        }

        private void ObtenerFormularioDEsdeObjeto()
        {
            txtDescripcion.Text = habitacion.Descripcion;
            txtNumeroHabitacion.Text = habitacion.Numero.ToString();
            cmbEstado.SelectedValue = habitacion.Estado;
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

        private void OcultarBotonesnOperaciones(Visibility oculto)
        {
            btnAgregar.Visibility = oculto;
            btnModificar.Visibility = oculto;
            btnEliminar.Visibility = oculto;
            btnRegresar.Visibility = oculto;

        }

        private bool VerificarValores()
        {
            if (txtDescripcion.Text == string.Empty || txtNumeroHabitacion.Text == string.Empty)
            {
                MessageBox.Show("Por favor ingresa todos los valores en las cajas de texto");
                return false;
            }
            else if (cmbEstado.SelectedValue == null)
            {
                MessageBox.Show("Por favor selecciona el estado de la habitacion");
                return false;
            }
            return true;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            //Verificar que se ingresaron los valores requeridos
            if(VerificarValores())
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

        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            if(lbHabitaciones.SelectedValue == null)
                MessageBox.Show("Por favor selecciona una habitacion desde el lisbox");
            else
            {
                try
                {
                    //Obteber la informacion de la habitacion
                    habitacion = habitacion.BuscarHabitacionPorId(Convert.ToInt32(lbHabitaciones.SelectedValue));

                    //Llnar los valores del formulario
                    ObtenerFormularioDEsdeObjeto();

                    //Ocultar los botones de operaciones
                    OcultarBotonesnOperaciones(Visibility.Hidden);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al momento de modificar la informacion");
                }
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //Monstrar los botones de operaciones
            OcultarBotonesnOperaciones(Visibility.Visible);

            LimpiarFormulario();
        }

        private void btAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (VerificarValores())
            {
                try
                {
                    //Obtener los valores para la habitacion desde el formulario
                    ObtenerValoresFormulario();

                    //Actualizar los valores en la base de datos
                    habitacion.ModificarHabitacion(habitacion);

                    //Actualizar el lisbox de habitaciones
                    ObtenerHabitaciones();

                    //Mensaje de actualizacion realizada
                    MessageBox.Show("Habitacion modificada correctamente");

                    //monstrar los btones de operaciones
                    OcultarBotonesnOperaciones(Visibility.Visible);

                    //Limpiar formulario
                    LimpiarFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al momento de actualizar la habitacion....");
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //actualizar el listbox
                    ObtenerHabitaciones();
                }
            }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            //Cerrar el formulario
            this.Close();
        }
    }
}
