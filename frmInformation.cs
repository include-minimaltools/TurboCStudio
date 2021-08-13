using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsCAssistant
{
    public partial class frmInformation : XtraForm
    {
        public frmInformation()
        {
            InitializeComponent();
        }

        private void frmInformation_Load(object sender, EventArgs e)
        {
            meInformation.Text = @"Turbo C Studio es un asistente para la creación de formularios y un entorno virtual gráfico en MS-DOS mediante el cual se pretende simular una versión reciente del sistema operativo de WINDOWS 10, también funciona como instalador de la máquina virtual que simula MS-DOS ""DOSBOX"" y del lenguaje de programación utilizado 'Turbo C 2.0', además anexa la funcionalidad de instalar un editor de texto de Microsoft llamado Visual Studio Code.

Turbo C Studio trabaja en conjunto a los programas antes mencionado para facilitar la programación en el lenguaje de Turbo C 2.0, aunque se sabe de que seguirá siendo aparatosa. Turbo C Studio abrirá diversos programas puestos en tu carpeta de BIN directamente en DOSBOX sin necesidad de estar buscando el archivo dentro del Entorno de Turbo C y te dará la opción de abrir dicho código en Visual Studio Code

La principal función de Turbo C Studio es generar un archivo C (.c) con el cual se pueda ejecutar una simulación básica del sistema operativo Windows 10 con la finalidad de que se puedan anexar programas al sistema con funcionalidades separadas.

Para la creación de formularios, Turbo C Studio te generará un archivo C (.c) el cual te guardará en la carpeta BIN de tu directorio, este archivo tendrá un código de preestablecido para poder visualizar en el modo gráfico un ""Form"" parecido al ya visto en Visual Studio con Visual Basic y C#, esto pretende ahorrar tiempo al programador al comenzar a realizar un proyecto y que se centre en las funcionalidades que formarán parte del formulario y no en las virtudes básicas

En dado caso se quiera generar un archivo.h se eliminarán ciertas funcionalidades y se introducirá en la carpeta de ""Include"", permitiendo así que en archivo Windows.c se ejecute con una simple función, habiendo claro, incluido la librería.

Los códigos generados por default se irán mejorando con diversas versiones del asistente, procure mantener actualizado el programa desde nuestro canal de discord. Si eres de primer año, esta aplicación será de mucha ayuda para ti y tus iniciaciones en el desactualizado Turbo C 2.0, te servirá para guiarte en el tema de funciones, tipos de datos, estructura de datos, punteros, modo gráfico, etc..Te invitamos cordialmente a utilizar el programa para tu beneficio, sin embargo, el plagio del código dado con el archivo c de Windows sería muy evidente en caso que lo quieras entregar como tuyo propio, así que te recomendamos que no lo hagas.

                                                                                                                            -Dev. Gabriel Alejandro Ortiz Amador.
                                                                                                                            -Discord >>>>>>>>> Aliz#9397.";
            btnOk.Focus();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}