using System;

namespace RezeptVerwaltung.DataAccess.Exceptions
{
	class NumberFormatException: Exception
	{
		public NumberFormatException(string message) : base(message){}
	}
}
