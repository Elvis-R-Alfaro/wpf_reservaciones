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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _26_reservaciones
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class IniciarSesion : Window
    {
        //Objeti de tipo usuarios para implementar su funcionalidad
        private Usuario usuario = new Usuario();
        public IniciarSesion()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Implementar la busqueda del usuario desde la clase
                Usuario elUsuario = usuario.BuscarUsuario(txtUsername.Text);

                //Verificar si el usuario existe
                if (elUsuario.Username == string.Empty)
                    MessageBox.Show("El usuario no existe en nuestro sistema");
                else
                {
                    //verificar que la contrasela ingresada es igual a la contraseña
                    //almaceanr en la bas de datos
                    if(elUsuario.Password == pwbPassword.Password)
                    {
                        MessageBox.Show("!Bienvenido al sistema de reservaciones¡");
                    }
                    else
                    {
                        MessageBox.Show("La contraseña no es correcta, Por favor verificala");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un erroe al momento de realzar la consulta...");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
