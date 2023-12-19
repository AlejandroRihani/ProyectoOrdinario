using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoOrdinario.Clases
{
    public class JugadorPoker : IJugador
    {
        private List<ICarta> mano;

        public JugadorPoker()
        {
            mano = new List<ICarta>();
        }

        public void RealizarJugada()
        {
            
            Console.WriteLine("Player makes a move.");
        }

        public void ObtenerCartas(List<ICarta> cartas)
        {

            mano.AddRange(cartas);
        }

        public ICarta DevolverCarta(int indiceCarta)
        {
            var carta = mano[indiceCarta];
            mano.RemoveAt(indiceCarta);
            return carta;
        }

        public List<ICarta> DevolverTodasLasCartas()
        {
            var todasLasCartas = new List<ICarta>(mano);
            mano.Clear();
            return todasLasCartas;
        }

        public List<ICarta> MostrarCartas()
        {
            return new List<ICarta>(mano);
        }

        public ICarta MostrarCarta(int indiceCarta)
        {
            return mano[indiceCarta];
        }
    }
}
