using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoOrdinario.Clases
{
    internal class JuegoPoker: IJuego
    {
        private IDealer dealer;
        private List<IJugador> jugadores;
        public IDealer Dealer => dealer;
        public bool JuegoTerminado 
        { 
            get; 
            private set; 
        }
        public JuegoPoker(IDealer dealer)
        {
            this.dealer = dealer;
            jugadores = new List<IJugador>();
        }
        public void AgregarJugador(IJugador jugador)
        {
            jugadores.Add(jugador);
        }
        public void IniciarJuego()
        {
            Console.WriteLine("El juego de póker está iniciando.");
            
        }
        public void JugarRonda()
        {
            Console.WriteLine("Playing a round of Poker.");
            dealer.BarajearDeck();
            foreach(var jugador in jugadores)
            {
                jugador.ObtenerCartas(dealer.RepartirCartas(2));
            }
            List<ICarta> cartasComunitarias = dealer.RepartirCartas(5);
            
            foreach (var jugador in jugadores)
            {
                jugador.RealizarJugada();
            }
        }
        public void MostrarGanador()
        {

            Console.WriteLine("Displaying the winner of the Poker game.");
            
        }
    }
}
