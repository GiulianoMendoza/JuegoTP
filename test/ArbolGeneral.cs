using System;
using System.Collections.Generic;

namespace DeepSpace
{
	public class ArbolGeneral<T>
	{
		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato) {
			this.dato = dato;
		}
	
		public T getDatoRaiz() {
			return this.dato;
		}
		
		public void setDatoRaiz(T dato){
			this.dato = dato;
		}
	
		public List<ArbolGeneral<T>> getHijos() {
			return hijos;
		}
	
		public void agregarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Add(hijo);
		}
	
		public void eliminarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Remove(hijo); 
		}
	
		public bool esHoja() {
			return this.getHijos().Count == 0;
		}
		
		public int nivel(ArbolGeneral<T> valor){
			Cola<ArbolGeneral<T>> c = new Cola<ArbolGeneral<T>>();
			ArbolGeneral<T> arbolAux;
			
			int nivel = 0;
			
			bool corte = false;
			
			c.encolar(this);
			c.encolar(null);
			
			while(!c.esVacia() && corte == false){
				arbolAux = c.desencolar(); //empiezo a desencolar el arbol en el aux para no eliminar de la cola real.
				
				if(arbolAux == null){
					if(!c.esVacia()){
						nivel++;
						c.encolar(null);
					}						
				}
				else{
					if(arbolAux.Equals(valor)){
						corte = true;
					}
				
					foreach(var hijo in arbolAux.hijos)
						c.encolar(hijo);
				}
			}
			return nivel;
		}			
	}
}
