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
        FueraDeServicio = 'F'
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
                case EstadosHabitacion.FueraDeServicio:
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

        /// <summary>
        /// Monstrar todas las habitaciones
        /// </summary>
        /// <returns>listado de habitaciones</returns>
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

        /// <summary>
        /// Obtiene una habitacion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Habitacion BuscarHabitacionPorId(int id)
        {
            Habitacion laHabitacion = new Habitacion();

            try
            {
                //Query busqueda
                string query = @"SELECT * From Habitaciones.habitacion
                                WHERE id = @id";

                //Establecer la coneccion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer el valor del parametro
                sqlCommand.Parameters.AddWithValue("@id", id);

                using (SqlDataReader rdr = sqlCommand.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        laHabitacion.Id = Convert.ToInt32(rdr["id"]);
                        laHabitacion.Descripcion = rdr["descripcion"].ToString();
                        laHabitacion.Numero = Convert.ToInt32(rdr["numero"]);
                        laHabitacion.Estado = (EstadosHabitacion)Convert.ToChar(rdr["estado"].ToString().Substring(0, 1));
                    }
                }

                return laHabitacion;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                //Cerrar la conexio
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Modifica los datos de una habitacion
        /// </summary>
        /// <param name="habitacion">El id de la habitacion</param>
        public void ModificarHabitacion(Habitacion habitacion)
        {
            try
            {
                //Query de actualizacion
                string query = @"UPDATE Habitaciones.Habitacion
                                SET descripcion = @descripcion, numero = @numero, estado = @estado
                                WHERE id = @id";

                //Strablecer la conexion
                sqlConnection.Open();

                //Crear el comando SQL
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer los valores de los parametros
                sqlCommand.Parameters.AddWithValue("@id", habitacion.Id);
                sqlCommand.Parameters.AddWithValue("@descripcion", habitacion.Descripcion);
                sqlCommand.Parameters.AddWithValue("@numero", habitacion.Numero);
                sqlCommand.Parameters.AddWithValue("@estado", ObtenerEstado(habitacion.Estado));

                //Ejecutar el comando de actualizar
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //Cerrar conexcion
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Elimina una habitacion
        /// </summary>
        /// <param name="id">El id de la habitacion</param>
        public void EliminarHabitacion(int id)
        {
            try
            {
                //Query de eliminar
                string query = @"DELETE FROM Habitaciones.Habitacion
                                WHERE id = @id";

                //Establecer la conexion SQL
                sqlConnection.Open();

                //Establecer el valor del parametro
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                //Establecer el valor del parametro
                sqlCommand.Parameters.AddWithValue("@id", id);

                //Ejecutar el comando de eliminacion
                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                //CErrar conexion
                sqlConnection.Close();
            }
        }
    }
}
