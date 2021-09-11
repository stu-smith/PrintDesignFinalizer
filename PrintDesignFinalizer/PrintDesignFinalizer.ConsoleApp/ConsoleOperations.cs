using System;
using System.Drawing;
using Console = Colorful.Console;

namespace PrintDesignFinalizer.ConsoleApp
{
	public class ConsoleOperations : IConsoleOperations
	{
		public void Info(string message)
		{
			Console.WriteLine(message);
		}

		public void Fatal(string message)
		{
			Console.WriteLine(message, Color.Red);
			Environment.Exit(1);
		}
	}
}
