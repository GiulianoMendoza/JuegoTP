using System;
using System.Collections.Generic;

namespace DeepSpace
{
	class Estrategia
	{		
		public String Consulta1(ArbolGeneral<Planeta> arbol) {
			
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			
			int distancia = 0;
			
			c.encolar(arbol); 
			while(!c.esVacia()){ 
				arbolAux = c.desencolar(); 
				
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()){ 
					distancia = arbol.nivel(arbolAux); 
				}
				
				foreach(var hijo in arbolAux.getHijos()){ 
					c.encolar(hijo);				
				}				
			}
			return "Consulta 1: La distancia entre el planeta del Bot y la raíz es: " + distancia;
		}		
		
		public String Consulta2(ArbolGeneral<Planeta> arbol) {
			
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			
			string lista = "Descendientes del Bot: ";
			bool existe = false;
				
			c.encolar(arbol);
			while(!c.esVacia()){
				
				arbolAux = c.desencolar();
				
				if(existe){ 
					lista = lista + arbolAux.getDatoRaiz().Poblacion() + " ";
				}
				
				if(arbolAux.getDatoRaiz().EsPlanetaDeLaIA()){ 
					existe = true; 
					while(!c.esVacia()){ 
						c.desencolar(); 
					}
				}
				foreach(var hijo in arbolAux.getHijos()){
					c.encolar(hijo);
				}
			}
			return "Consulta 2: " + lista;
		}

		public String Consulta3(ArbolGeneral<Planeta> arbol) {
			
			Cola<ArbolGeneral<Planeta>> c = new Cola<ArbolGeneral<Planeta>>();
			ArbolGeneral<Planeta> arbolAux;
			
			int nivel = 0;
			int poblacionTotal = 0;
			int contador = 0;
			int promedio = 0;
			
			string texto = "Consulta 3: ";
			
			c.encolar(arbol);
			c.encolar(null);
			
			texto = texto + "Nivel: " + nivel;  
			texto = texto + " | PoblacionTotal: "+ arbol.getDatoRaiz().Poblacion();
			texto = texto + " | Promedio: " + arbol.getDatoRaiz().Poblacion();
			
			while(!c.esVacia()){ 
				arbolAux = c.desencolar(); 
				
				if(arbolAux == null){ 
					if(!c.esVacia()){ 
						nivel++; 
						texto = texto + "\n                   Nivel: " + nivel;
						texto = texto + " | PoblacionTotal: " + poblacionTotal;
						texto = texto + " | Promedio: " + promedio;
						c.encolar(null);
						poblacionTotal = 0;
						promedio = 0;
						contador = 0;
					}						
				}
				else{
					foreach(var hijo in arbolAux.getHijos()){ 
						contador++;  
						poblacionTotal = poblacionTotal + hijo.getDatoRaiz().Poblacion(); 
						c.encolar(hijo);
					}
					if(arbolAux.getHijos().Count > 0){  
						promedio = poblacionTotal / contador; 
					}
				}
			}
			return texto;
		}
		
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol) {
			
			List<Planeta> Ataque = CaminoEstrategiaBot(arbol);
		
			List<Planeta> CaminoHaciaBot = CalcularCaminoHaciaBot(arbol);
			
			List<Planeta> CaminoHaciaJugador = CalcularCaminoHaciaJugador(arbol);
			
			List<Planeta> CaminoBotHaciaJugador = CalcularCaminoBotHaciaJugador(CaminoHaciaBot,CaminoHaciaJugador);
			
			if(CaminoBotHaciaJugador[1].EsPlanetaDelJugador()){ 
				
				CaminoBotHaciaJugador = CalcularEstrategia(Ataque,CaminoHaciaJugador); 
				Ataque = CalcularCaminoBotHaciaJugador(CaminoHaciaBot,CaminoHaciaJugador); 
				
				if(Ataque[0].Poblacion() > Ataque[1].Poblacion()){ 
					CaminoBotHaciaJugador = CalcularCaminoBotHaciaJugador(CaminoHaciaBot,CaminoHaciaJugador); 
				}
			}
			
			Movimiento ataque = new Movimiento(CaminoBotHaciaJugador[0],CaminoBotHaciaJugador[1]); 
			
			return ataque; 
		}
		
		
		//Para llegar al jugador
		private List<Planeta> CalcularCaminoHaciaBot(ArbolGeneral<Planeta> arbol){
			
			List<Planeta> CaminoHaciaBot = new List<Planeta>();
			
			_CalcularCaminoHaciaBot(arbol, CaminoHaciaBot);
			
			return CaminoHaciaBot;
		}
		
		private bool _CalcularCaminoHaciaBot(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaBot) {
			
			bool caminoHallado = false; 
			
			CaminoHaciaBot.Add(arbol.getDatoRaiz());
			
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA()){ 
				caminoHallado = true;
			} 
			else{
				foreach(var hijo in arbol.getHijos()){
					caminoHallado = _CalcularCaminoHaciaBot(hijo, CaminoHaciaBot); 
					
					if(caminoHallado){ 
						return true;
					}
					CaminoHaciaBot.RemoveAt(CaminoHaciaBot.Count - 1); 
				}				
			}
			return caminoHallado; 
		}
		
		private List<Planeta> CalcularCaminoHaciaJugador(ArbolGeneral<Planeta> arbol){
			
			List<Planeta> CaminoHaciaJugador = new List<Planeta>();
			
			_CalcularCaminoHaciaJugador(arbol, CaminoHaciaJugador);
			
			return CaminoHaciaJugador;
		}
		
		private bool _CalcularCaminoHaciaJugador(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaJugador) {
			
			bool caminoHallado = false;
			
			CaminoHaciaJugador.Add(arbol.getDatoRaiz());
			
			if(arbol.getDatoRaiz().EsPlanetaDelJugador()){
				caminoHallado = true;
			}
			else{  
				foreach(var hijo in arbol.getHijos()){
					caminoHallado = _CalcularCaminoHaciaJugador(hijo, CaminoHaciaJugador);
						
					if(caminoHallado){	
						return true;
					}
					CaminoHaciaJugador.RemoveAt(CaminoHaciaJugador.Count - 1); 
				}				
			}
			return caminoHallado; 
		}
		
		private List<Planeta> CalcularCaminoBotHaciaJugador(List<Planeta> CaminoHaciaBot, List<Planeta> CaminoHaciaJugador){
			
			List<Planeta> PlanetasBot = new List<Planeta>();
			
			List<Planeta> PlanetasNeutro = new List<Planeta>();
			
			List<Planeta> PlanetasJugador = new List<Planeta>();
			
			
			List<Planeta> AncestrosComun = new List<Planeta>();
			
			List<Planeta> CaminoBotHaciaJugador = new List<Planeta>();
			
			Planeta ancestroComun;
			
			bool existe = false;
			//bucle para hallar los ancestro comunes entre los caminos del bot y del jugador.
			for(int i = 0; i < CaminoHaciaBot.Count && i < CaminoHaciaJugador.Count; i++){
				if(CaminoHaciaBot[i] == CaminoHaciaJugador[i]){
					AncestrosComun.Add(CaminoHaciaBot[i]);
				}
			}
			ancestroComun = AncestrosComun[AncestrosComun.Count - 1];
			//bucle para construir el camino del bot hacia el jugador 
			for(int i = CaminoHaciaBot.Count - 1; i >= 0; i--){
				CaminoBotHaciaJugador.Add(CaminoHaciaBot[i]);
				if(CaminoHaciaBot[i] == ancestroComun){
					break;
				}				
			}
			foreach(var x in CaminoHaciaJugador){
				if(existe){
					CaminoBotHaciaJugador.Add(x);
				}
				
				if(x == ancestroComun){
					existe = true;
				}
			}
			//clasificar los planetas en sus respectivas listas
			foreach(var x in CaminoBotHaciaJugador){
				if(x.EsPlanetaDeLaIA()){
					PlanetasBot.Add(x);
				}
				if(x.EsPlanetaDelJugador()){
					PlanetasJugador.Add(x);
				}
				if(x.EsPlanetaNeutral()){
					PlanetasNeutro.Add(x);
				}
			}
			//limpiar el camino y construir uno nuevo 
			CaminoBotHaciaJugador.Clear();
			
			CaminoBotHaciaJugador.Add(PlanetasBot[PlanetasBot.Count - 1]);
			
			foreach(var x in PlanetasNeutro){
				CaminoBotHaciaJugador.Add(x);
			}
			
			CaminoBotHaciaJugador.Add(PlanetasJugador[0]);
			
			return CaminoBotHaciaJugador;
		}	
		
		
		//Estrategia de ayuda
		private void ListaDeBot(ArbolGeneral<Planeta> arbol, List<Planeta> Ataque) {
			
			if(arbol.getDatoRaiz().EsPlanetaDeLaIA()){ 
				Ataque.Add(arbol.getDatoRaiz()); 
			}
			
			foreach(var hijo in arbol.getHijos()){ 
				ListaDeBot(hijo, Ataque); 
			}
		}
		
		private void OrdenarPorIntercambio(ref List<Planeta> datos)
		{
			int n = datos.Count;
			for(int i = 0; i < (n - 1); i++)
			{
				for(int j = i + 1; j < n; j++){
					if(datos[i].Poblacion() > datos[j].Poblacion()){ //si el planeta en la posicion i es mayor que en j se realiza el intercambio de posiciones de los planetas
						Planeta swap = datos[i];
						datos[i] = datos[j];
						datos[j] = swap; //aca hago un swap, como el intercambio de la heap
					}
				}
			}
		}
		
		private List<Planeta> CaminoEstrategiaBot(ArbolGeneral<Planeta> arbol){
			
			List<Planeta> Ataque = new List<Planeta>(); 
			
			List<Planeta> CaminoHaciaBot = new List<Planeta>();
	
			ListaDeBot(arbol,Ataque);
			
			OrdenarPorIntercambio(ref Ataque); 
			
			Planeta Maximo = Ataque[Ataque.Count - 1]; 
			 
			_CaminoEstrategiaBot(arbol, CaminoHaciaBot, Maximo); 
			
			return CaminoHaciaBot;
		}
		
		private bool _CaminoEstrategiaBot(ArbolGeneral<Planeta> arbol, List<Planeta> CaminoHaciaBot, Planeta Maximo) {
			
			bool caminoHallado = false;
			
			CaminoHaciaBot.Add(arbol.getDatoRaiz());
			
			if(arbol.getDatoRaiz() == Maximo){
				caminoHallado = true;
			}
			else{
				foreach(var hijo in arbol.getHijos()){
					caminoHallado = _CaminoEstrategiaBot(hijo, CaminoHaciaBot, Maximo); 
					
					if(caminoHallado){ 
						break;
					}
					CaminoHaciaBot.RemoveAt(CaminoHaciaBot.Count - 1);
				}				
			}
			return caminoHallado;
		}
		
		private List<Planeta> CalcularEstrategia(List<Planeta> Ataque, List<Planeta> CaminoHaciaJugador){
			
			List<Planeta> AncestrosComun = new List<Planeta>();
			
			List<Planeta> CaminoBotHaciaJugador = new List<Planeta>();
			
			Planeta ancestroComun;
			
			bool existe = false;
			
			for(int i = 0; i < Ataque.Count && i < CaminoHaciaJugador.Count; i++){ 
				if(Ataque.Contains(CaminoHaciaJugador[i])){ 
					AncestrosComun.Add(CaminoHaciaJugador[i]); 
				}
			}
			ancestroComun = AncestrosComun[AncestrosComun.Count - 1]; 
			
			for(int i = Ataque.Count - 1; i >= 0; i--){
				CaminoBotHaciaJugador.Add(Ataque[i]);
				if(Ataque[i] == ancestroComun){
					break;
				}				
			}
			foreach(var x in CaminoHaciaJugador){
				if(existe){ 
					CaminoBotHaciaJugador.Add(x); 
				}
				
				if(x == ancestroComun){ 
					existe = true;
				}
			}
			return CaminoBotHaciaJugador;
		}
	}
}