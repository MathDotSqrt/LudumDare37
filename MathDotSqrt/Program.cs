﻿using MathDotSqrt.Sqrt3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathDotSqrt {
	public class Program {
		
		public static void Main(string[] args) {
			GameComponent component = new GameComponent(1000, 800);  
			component.Run(); 
		}
	}
}
