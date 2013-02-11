namespace Zeta.Extreme
{
	/// <summary>
	/// Расширения для последовательного API сессии
	/// </summary>
	public static class SerialAccessExtensions
	{
		/// <summary>
		/// Создает объект API последовательного синхронного доступа
		/// </summary>
		/// <param name="session"></param>
		/// <returns></returns>
		public static ISerialSession AsSerial(this ZexSession session) {
			return new SerialSession(session);
		}
	}
}
