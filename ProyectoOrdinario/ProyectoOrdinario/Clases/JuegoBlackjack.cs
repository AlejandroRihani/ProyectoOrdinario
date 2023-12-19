// BlackjackJuego class
using ProyectoOrdinario.Enumeradores;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoOrdinario.Clases
{
    public class BlackjackJuego : IJuego
    {
        private List<IJugador> jugadores;
        private Dealer dealer;
        private IDeckDeCartas deck;
        private bool juegoTerminado;

        public BlackjackJuego()
        {
            this.jugadores = new List<IJugador>();
            this.dealer = new Dealer();
            this.deck = new DeckDeCartas();
            this.juegoTerminado = false;
        }

        public IDealer Dealer => dealer;

        public bool JuegoTerminado => juegoTerminado;

        public void AgregarJugador(IJugador jugador)
        {
            jugadores.Add(jugador);
        }

        public void IniciarJuego()
        {
            Console.WriteLine("Iniciando juego de 21 Blackjack...");
            // Barajear el mazo antes de cada juego
            deck.BarajearDeck();
        }

        public void JugarRonda()
        {
            Console.WriteLine("Iniciando nueva ronda...");

            // Repartir las cartas iniciales
            foreach (var jugador in jugadores)
            {
                jugador.ObtenerCartas(dealer.RepartirCartas(2));
            }

            // Repartir las cartas iniciales para el crupier
            dealer.ObtenerCartas(dealer.RepartirCartas(2));

            // Mostrar las cartas iniciales
            MostrarEstadoJuego();

            // Permitir que cada jugador realice una jugada
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Turno del jugador {jugadores.IndexOf(jugador)}");
                jugador.RealizarJugada();
            }

            // El crupier realiza sus jugadas
            dealer.RealizarJugada();

            // Mostrar el estado final del juego
            MostrarEstadoJuego();

            // Determinar al ganador y finalizar la ronda
            MostrarGanador();
            juegoTerminado = true;
        }

        public void MostrarGanador()
        {
            Console.WriteLine("Determinando al ganador...");

            // Obtener la mano del crupier
            List<ICarta> manoCrupier = dealer.MostrarCartas();
            int valorManoCrupier = CalcularValorMano(manoCrupier);

            // Calcular el valor de la mano de cada jugador
            Dictionary<IJugador, int> valoresManosJugadores = new Dictionary<IJugador, int>();
            foreach (var jugador in jugadores)
            {
                List<ICarta> manoJugador = jugador.MostrarCartas();
                int valorManoJugador = CalcularValorMano(manoJugador);
                valoresManosJugadores.Add(jugador, valorManoJugador);
            }

            // Determinar al ganador
            IJugador ganador = null;
            int valorGanador = 0;
            //kvp significa key value pair (par de valor y llave)
            foreach (var kvp in valoresManosJugadores)
            {
                if (kvp.Value <= 21 && kvp.Value > valorGanador)
                {
                    ganador = kvp.Key;
                    valorGanador = kvp.Value;
                }
            }

            // Mostrar el resultado
            if (valorGanador > valorManoCrupier && valorGanador <= 21)
            {
                Console.WriteLine($"El ganador es el jugador {jugadores.IndexOf(ganador)} con un valor de mano de {valorGanador}.");
            }
            else
            {
                Console.WriteLine("El crupier gana.");
            }
        }

        private int CalcularValorMano(List<ICarta> mano)
        {
            int valorMano = 0;
            int ases = 0;

            foreach (var carta in mano)
            {
                if (carta.Valor == ValoresCartasEnum.As)
                {
                    ases++;
                }
                else
                {
                    valorMano += ObtenerValorCarta(carta);
                }
            }

            // Manejar los ases
            for (int i = 0; i < ases; i++)
            {
                if (valorMano + 11 <= 21)
                {
                    valorMano += 11;
                }
                else
                {
                    valorMano += 1;
                }
            }

            return valorMano;
        }

        private int ObtenerValorCarta(ICarta carta)
        {
            switch (carta.Valor)
            {
                case ValoresCartasEnum.Diez:
                case ValoresCartasEnum.Jota:
                case ValoresCartasEnum.Reina:
                case ValoresCartasEnum.Rey:
                    return 10;

                default:
                    return (int)carta.Valor;
            }
        }

        private void MostrarEstadoJuego()
        {
            Console.WriteLine("\nEstado actual del juego:");

            // Mostrar las cartas de cada jugador
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Cartas del jugador {jugadores.IndexOf(jugador)}:");
                foreach (var carta in jugador.MostrarCartas())
                {
                    Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
                }
                Console.WriteLine();
            }

            // Mostrar las cartas del crupier
            Console.WriteLine("Cartas del crupier:");
            foreach (var carta in dealer.MostrarCartas())
            {
                Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
            }

            Console.WriteLine();
        }
    }
}
