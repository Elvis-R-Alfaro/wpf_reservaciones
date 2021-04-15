using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Agrefar los namespace
using System.Data.SqlClient;
using System.Configuration;

namespace _26_reservaciones
{
    //Crear una variable que mantengaa los valoress para los estados de la habitacion
    public enum EstadosHabitacion
    {
        Ocupado = 'O',
        Disponible = 'D',
        Mantenimiento = 'M',
        FueraServicio = 'F'
    }
    class Habitacion
    {
        //Variable miembro
        private static string connectionString = ConfigurationManager.ConnectionStrings["_26_reservaciones.Properties.Settings.ReservacionesConexion"].ConnectionString;
        private SqlConnection sqlConnection = new SqlConnection(connectionString);

        //Propiedades

        public int Id { get; set; }

        public string Descripcion { get; set; }

        public int Numero { get; set; }
        public EstadosHabitacion Estado { get; set; }

        //Constructor
        public Habitacion() { }

        
        public Habitacion(string descripcion, int numero, EstadosHabitacion estado)
        {
            Descripcion = descripcion;
            Numero = numero;
            Estado = estado;
        }

        //Metodos

        /// <summary>
        /// Obtiene el estado de la habitacion  desde el enum estados
        /// </summary>
        /// <param name="estado">El valor denteo del enum</param>
        /// <returns>Estado valido dentro de la base de datos</returns>
        private string ObtenerEstado(EstadosHabitacion estado)
        {
            switch (estado)
            {
                case EstadosHabitacion.Ocupado:
                    return "OCUPADA";
                case EstadosHabitacion.Disponible:
                    return "DISPONIBLE";
                case EstadosHabitacion.Mantenimiento:
                    return "MANTENIMIENTO";
                case EstadosHabitacion.FueraServicio:
                    return "FUERADESERVICIO";
                default:
                    return "DISPONIBLE";
            }
        }

        /// <summary>
        /// Inserta la habitacion
        /// </summary>
        /// <param name="habitacion">la informacion de la habitacion</param>
        public void CrearHabitacion(Habitacion habitacion)
        {
            try
            {
                //Query de inserccion
                string query = @"INSERT INTO Habitaciones.Habitacion (descripcion, numero, estado)
                                VALUES (@descripcion, @numero, @estado)";

                //EStablcer la conexion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer los valores de los paramawtros
                sqlCommand.Parameters.AddWithValue("@descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue("@numero", habitacion.Numero);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(habitacion.Estado));

                //ejecutar el comando insertado
                sqlCommand.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
        }

        public List<Habitacion> MonstrarHabitaciones()
        {
            //Inicializar una lista vacia de habitaciones
            List<Habitacion> habitaciones = new List<Habitacion>();

            try
            {
                //Query de seleccion
                string query = @"SELECT id, descripcion
                                FROM habitaciones.habitacion";

                //Establcer la coneccion
                sqlConnection.Open();

                //Crear el comando sql
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Obtener los datos de las habitaciones
                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        habitaciones.Add(new Habitacion { Id = Convert.ToInt32(rdr["id"]), Descripcion = rdr["Descripcion"].ToString() });
                    }
                }
                return habitaciones;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                //Cerrar la conexion
                sqlConnection.Close();
            }
        }
    }
}
