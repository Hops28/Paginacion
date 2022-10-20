using Paginacion.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace Paginacion
{
    public partial class Paginacion : Form
    {
        // El tipo de animación que va a tener el movimiento de la presentación
        Transition Transicion = new Transition(new TransitionType_EaseInEaseOut(1000));

        public Paginacion()
        {
            InitializeComponent();

            // Luego de cargar los componentes se crea un hilo que va a ser
            // utilizado para usar un delay en paralelo con el flujo actual
            ThreadStart Delegado = new ThreadStart(MoverPresentacion);
            Thread Hilo = new Thread(Delegado);

            // Se comienza el hilo
            Hilo.Start();
        }

        void MoverPresentacion()
        {
            // Se espera por 4.5 segundos
            Thread.Sleep(4500);

            // Se realiza el movimiento al panel 'Presentacion'
            Transicion.add(Presentacion, "Left", 720);
            Transicion.run();
        }

        private void Cerrar_MouseHover(object sender, EventArgs e)
        {
            // Cambio de imagen al colocar el mouse
            Cerrar.Image = Resources.cancel2;
        }

        private void Cerrar_MouseLeave(object sender, EventArgs e)
        {
            // Cambio de imagen al quitar el mouse
            Cerrar.Image = Resources.cancel1;
        }

        private void Cerrar_Click(object sender, EventArgs e)
        {
            // Se cierra el programa
            Application.Exit();
        }

        private void txtNombreProceso_Click(object sender, EventArgs e)
        {
            // Si el campo tiene la palabra 'proceso', automáticamente se vacía
            if (txtNombreProceso.Text.Equals("Proceso"))
            {
                txtNombreProceso.Text = "";
                txtNombreProceso.ForeColor = Color.FromArgb(0, 0, 64, 100);
            }
        }

        private void txtNombreProceso_Leave(object sender, EventArgs e)
        {
            // Si el campo está vacío y pierde el foco, se rellena con un marcador
            if (txtNombreProceso.Text.Equals(""))
            {
                txtNombreProceso.Text = "Proceso";
                txtNombreProceso.ForeColor = Color.Silver;
            }
        }

        private void txtNombreProceso_KeyUp(object sender, KeyEventArgs e)
        {
            string txt = txtNombreProceso.Text;

            // El campo no tiene que estar vacío
            if (txt.Length > 0)
            {
                // Si el nombre empieza con espacio no se activarán los botones
                if (txt[0].Equals(' '))
                {
                    control_botones(false);
                }
                else // De lo contraro si se activarán
                {
                    // Si el campo no está vacío, los botones se activan
                    control_botones(true);
                }
            }
            else
            {
                // Se desactivan los botones si el campo está vacío
                control_botones(false);
            }
        }

        private void btnTerminarProceso_MouseLeave(object sender, EventArgs e)
        {
            btnTerminarProceso.ForeColor = Color.Black;
        }

        private void btnTerminarProceso_MouseEnter(object sender, EventArgs e)
        {
            btnTerminarProceso.ForeColor = Color.Navy;
        }

        private void tam1_Click(object sender, EventArgs e)
        {
            agregarProceso(1, txtNombreProceso.Text);
        }

        private void tam2_Click(object sender, EventArgs e)
        {
            agregarProceso(2, txtNombreProceso.Text);
        }

        private void tam3_Click(object sender, EventArgs e)
        {
            agregarProceso(3, txtNombreProceso.Text);
        }

        private void tam4_Click(object sender, EventArgs e)
        {
            agregarProceso(4, txtNombreProceso.Text);
        }

        private void tam5_Click(object sender, EventArgs e)
        {
            agregarProceso(5, txtNombreProceso.Text);
        }

        private void tam6_Click(object sender, EventArgs e)
        {
            agregarProceso(6, txtNombreProceso.Text);
        }

        private void tam7_Click(object sender, EventArgs e)
        {
            agregarProceso(7, txtNombreProceso.Text);
        }

        private void tam8_Click(object sender, EventArgs e)
        {
            agregarProceso(8, txtNombreProceso.Text);
        }

        private void cbProcesos_SelectedValueChanged(object sender, EventArgs e)
        {
            btnTerminarProceso.Enabled = true;
        }

        /*********************************************************************/
        /*************************FUNCIONES PROPIAS***************************/
        /*********************************************************************/

        // Listas para almacenar los procesos y páginas
        string[] MarcoPaginas = new string[8];

        // Lista para guardar los procesos en el marco
        List<Proceso> ProcesosEnMarco = new List<Proceso>();

        // Lista de tipo cola para guardar los procesos en espera
        Queue<Proceso> procesosEnEspera = new Queue<Proceso>();

        int contadorPaginasOcupadas = 0,
            contadorProcesosEnMarco = 0,
            contadorPaginasLibres = 8, i;

        private void btnTerminarProceso_Click(object sender, EventArgs e)
        {
            terminarProceso();
        }

        // Método que controla todos los botones
        public void control_botones(bool valorControl)
        {
            tam1.Enabled = valorControl;
            tam2.Enabled = valorControl;
            tam3.Enabled = valorControl;
            tam4.Enabled = valorControl;
            tam5.Enabled = valorControl;
            tam6.Enabled = valorControl;
            tam7.Enabled = valorControl;
            tam8.Enabled = valorControl;
        }

        public void agregarProceso (int size, string nombreProceso)
        {
            // Se valida si el nombre del proceso no se repite
            bool noSeRepite = true;

            // Ciclo para verificar si el nombre del proceso no se repite...
            // De lo contrario el ciclo se romperá y no se realizarán las demás instrucciones
            foreach (Proceso p in ProcesosEnMarco)
            {
                if (p.name.Equals(nombreProceso))
                {
                    noSeRepite = false;
                    break;
                }
            }

            // Se revisa en la cola por si acaso
            foreach (Proceso p in procesosEnEspera)
            {
                if (p.name.Equals(nombreProceso))
                {
                    noSeRepite = false;
                    break;
                }
            }

            if (!noSeRepite)
            {
                MessageBox.Show("EL nombre del proceso ya existe");
                imprimir();
            }

            // Si el tamaño se ajusta a las páginas ocupadas
            if (contadorPaginasLibres >= size && noSeRepite)
            {
                // Se añaden las páginas al marco
                for (i = contadorPaginasOcupadas; i < (contadorPaginasOcupadas + size); i++)
                {
                    MarcoPaginas[i] = nombreProceso;
                }

                // Se tiene que controlar el tamaño del marco
                contadorPaginasOcupadas += size;

                // Se actualiza la cantidad de páginas libres
                contadorPaginasLibres = 8 - contadorPaginasOcupadas;

                // Se añade a la lista de procesos en marco
                ProcesosEnMarco.Add(new Proceso(nombreProceso, size));

                // Y se contabilizan los procesos dentro del marco
                contadorProcesosEnMarco++;

                imprimir();
            }
            else if (!(contadorPaginasLibres >= size) && noSeRepite)
            {
                // Se añaden a la cola de espera
                procesosEnEspera.Enqueue(new Proceso(nombreProceso, size));

                imprimir();
            }

            if (contadorProcesosEnMarco > 0)
            {
                // Si hay procesos en marco se habilita el combobox con esos procesos
                cbProcesos.Enabled = true;

                imprimir();
            }
            else
            {
                // Si no hay procesos entonces estará inactivo
                cbProcesos.Enabled = false;
            }

            // Se verifica si hay otros procesos en espera que quepan en el marco
            if (procesosEnEspera.Count > 0)
            {
                if (contadorPaginasLibres >= procesosEnEspera.Peek().size)
                {
                    Proceso NuevoProceso = procesosEnEspera.Dequeue();

                    agregarProceso(NuevoProceso.size, NuevoProceso.name);
                }

                imprimir();
            }
        }

        public void terminarProceso ()
        {
            // Se obtiene el proceso que se quiere terminar
            string procesoATerminar = cbProcesos.SelectedItem.ToString();

            // Variable de iteración
            int i, orden = 0, paginasEliminadas = 0;

            // Se recorre la lista de los procesos en marco para quitarlo
            foreach (Proceso p in ProcesosEnMarco)
            {
                // Luego de encontrarlo
                if (p.name.Equals(procesoATerminar))
                {
                    // Se elimina de la lista
                    ProcesosEnMarco.Remove(p);

                    // Se eliminan las páginas en el marco
                    for (i = 0; i < contadorPaginasOcupadas; i++)
                    {
                        // Se reemplaza por un null
                        if (MarcoPaginas[i].Equals(procesoATerminar))
                        {
                            MarcoPaginas[i] = null;
                            paginasEliminadas++;
                        }
                    }

                    contadorPaginasOcupadas -= paginasEliminadas;
                    contadorPaginasLibres += paginasEliminadas;

                    // Se ordenan las páginas en el arreglo del marco
                    for (i = 0; i < 8; i++)
                    {
                        // Si es diferente a null entonces es uno de los procesos
                        // Así que se transfiere directo al inicio
                        if (MarcoPaginas[i] != null)
                        {
                            MarcoPaginas[orden] = MarcoPaginas[i];
                            orden++;
                        }
                    }

                    // Se limpian los sobrantes
                    for (i = contadorPaginasOcupadas; i < 8; i++)
                    {
                        // Si es diferente a null entonces es uno de los procesos
                        // Así que se transfiere directo al inicio
                        MarcoPaginas[i] = null;
                    }

                    contadorProcesosEnMarco--;
                    break; // Se interrumpe el ciclo
                }
            }

            // Se obtiene el primer proceso añadido a cola
            if (procesosEnEspera.Count > 0)
            {
                // Se guarda el proceso sacado de la cola
                Proceso NuevoProceso = procesosEnEspera.Dequeue();

                agregarProceso(NuevoProceso.size, NuevoProceso.name);
            }
        }

        public void imprimir ()
        {
            // Se vacía el campo de texto, se deshabilitan los botones
            // Y se vacía la lista
            txtNombreProceso.Text = "";
            control_botones(false);
            Marco.Items.Clear();
            ListaProcesos.Items.Clear();
            ListaProcesosEspera.Items.Clear();
            cbProcesos.Items.Clear();
            cbProcesos.Text = "";
            btnTerminarProceso.Enabled = false;

            // Se activa el combobox donde van a estar todos los procesos agregados y activos
            foreach (Proceso p in ProcesosEnMarco)
            {
                //string strProcesoEnMarco = p.name + " (" + p.size + ")";
                cbProcesos.Items.Add(p.name);
            }

            // Se imprimen en la lista
            for (i = 0; i < contadorPaginasOcupadas; i++)
            {
                Marco.Items.Add(MarcoPaginas[i]);
            }

            // Se imprimen los procesos
            foreach (Proceso p in ProcesosEnMarco)
            {
                string strProcesoEnMarco = p.name + " (" + p.size + ")";
                ListaProcesos.Items.Add(strProcesoEnMarco);
            }

            // Se muestran en la lista de procesos en espera
            foreach (Proceso w in procesosEnEspera)
            {
                string strProcesoEspera = w.name + " (" + w.size + ")";
                ListaProcesosEspera.Items.Add(strProcesoEspera);
            }

            lblPaginasOcupadas.Text = contadorProcesosEnMarco.ToString();
        }
    }
}
