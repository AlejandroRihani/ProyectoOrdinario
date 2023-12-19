using ProyectoOrdinario.Enumeradores;
using ProyectoOrdinario.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProyectoOrdinario.Clases
{
    public class JuegoPoker : IJuego
    {
        private List<IJugador> jugadores;
        private IDealer dealer;
        private IDeckDeCartas deck;
        private bool juegoTerminado;

        public JuegoPoker()
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
            Console.WriteLine("Iniciando juego de Poker...");
            // Barajear el mazo antes de cada juego
            deck.BarajearDeck();
        }

        public void JugarRonda()
        {
            Console.WriteLine("Iniciando nueva ronda...");

            // Repartir cinco cartas a cada jugador
            foreach (var jugador in jugadores)
            {
                jugador.ObtenerCartas(dealer.RepartirCartas(5));
            }

            // Mostrar las cartas iniciales
            MostrarEstadoJuego();

            // Permitir que cada jugador realice un intercambio de cartas
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Turno del jugador número {jugadores.IndexOf(jugador)}");
                jugador.RealizarJugada();
            }

            // Mostrar el estado final del juego
            MostrarEstadoJuego();

            // Determinar al ganador y finalizar la ronda
            MostrarGanador();
            juegoTerminado = true;
        }

        public void MostrarGanador()
        {
            Console.WriteLine("Determinando al ganador...");

            IJugador ganador = null;
            int maxHandRank = 0;

            foreach (var jugador in jugadores)
            {
                int handRank = EvaluarMano(jugador.MostrarCartas());

                if (handRank > maxHandRank)
                {
                    maxHandRank = handRank;
                    ganador = jugador;
                }
            }

            if (ganador != null)
            {
                Console.WriteLine($"Ganador: {jugadores.IndexOf(ganador)} Mano: {TraducirRankMano(maxHandRank)}");
                //MostrarManoJugador(ganador);
            }
            else
            {
                Console.WriteLine("Es un empate.");
            }
        }

        private int EvaluarMano(List<ICarta> mano)
        {
            // Order the cards by value and count occurrences of each value
            var orderedCards = mano.OrderBy(carta => (int)carta.Valor).ToList();
            var valueCounts = orderedCards.GroupBy(carta => carta.Valor).ToDictionary(grupo => grupo.Key, grupo => grupo.Count());

            // Check for specific combinations in decreasing order of rank
            if (EsEscaleraReal(orderedCards)) return 9;
            if (EsEscaleraColor(orderedCards)) return 8;
            if (EsPoker(valueCounts)) return 7;
            if (EsFullHouse(valueCounts)) return 6;
            if (EsColor(mano)) return 5;
            if (EsEscalera(orderedCards)) return 4;
            if (EsTrio(valueCounts)) return 3;
            if (EsDoblePar(valueCounts)) return 2;
            if (EsPar(valueCounts)) return 1;
            return 0;
            
        }

        private string TraducirRankMano(int rank)
        {
            switch (rank)
            {
                case 9:
                    return "Escalera Real";
                case 8:
                    return "Escalera de Color";
                case 7:
                    return "Poker";
                case 6:
                    return "Full House";
                case 5:
                    return "Color";
                case 4:
                    return "Escalera";
                case 3:
                    return "Trío";
                case 2:
                    return "Doble Par";
                case 1:
                    return "Par";
                default:
                    return "Carta Alta";
            }
        }

        private bool EsEscaleraReal(List<ICarta> mano)
        {
            if (mano.All(carta => carta.Valor >= ValoresCartasEnum.Diez) && EsEscaleraColor(mano))
            {
                return true;
            }
            return false;
        }

        private bool EsEscaleraColor(List<ICarta> mano)
        {
            return EsColor(mano) && EsEscalera(mano);
        }

        private bool EsPoker(Dictionary<ValoresCartasEnum, int> valueCounts)
        {
            return valueCounts.ContainsValue(4); // Check for four cards of the same value
        }

        private bool EsFullHouse(Dictionary<ValoresCartasEnum, int> valueCounts)
        {
            return valueCounts.ContainsValue(3) && valueCounts.ContainsValue(2); // Check for three cards of one value and two cards of another
        }


        private bool EsColor(List<ICarta> mano)
        {
            return mano.All(carta => carta.Figura == mano.First().Figura);
        }

        private bool EsEscalera(List<ICarta> mano)
        {
            for (int i = 0; i < mano.Count - 1; i++)
            {
                if ((int)mano[i + 1].Valor - (int)mano[i].Valor != 1)
                {
                    return false;
                }
            }
            return true;
        }
        private bool EsTrio(Dictionary<ValoresCartasEnum, int> valueCounts)
        {
            return valueCounts.ContainsValue(3); // Check for three cards of the same value
        }

        private bool EsDoblePar(Dictionary<ValoresCartasEnum, int> valueCounts)
        {
            int cantidadPares = valueCounts.Count(pair => pair.Value == 2);
            return cantidadPares >= 2; // Check for at least two pairs
        }

        private bool EsPar(Dictionary<ValoresCartasEnum, int> valueCounts)
        {
            return valueCounts.ContainsValue(2); // Check for two cards of the same value
        }
        private void MostrarEstadoJuego()
        {
            Console.WriteLine("\nEstado actual del juego:");

            // Mostrar las cartas de cada jugador
            foreach (var jugador in jugadores)
            {
                Console.WriteLine($"Cartas de {jugador.GetType().Name}:");
                foreach (var carta in jugador.MostrarCartas())
                {
                    Console.WriteLine($"   {carta.Valor} de {carta.Figura}");
                }
                Console.WriteLine();
            }
        }
        private void MostrarManoJugador(IJugador jugador)
        {
            foreach(var carta in jugador.MostrarCartas())
            {
                Console.WriteLine($"{carta.Valor} de {carta.Figura}");
            }        
        }
    }
}