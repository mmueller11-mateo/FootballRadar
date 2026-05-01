using My.Framework.EventHandling;

namespace FootballRadar.EventHandling
{
	internal static class EventSerializer
	{
		public static string Serialize<TEvent>(TEvent eventData) where TEvent : IEvent
		{
			return System.Text.Json.JsonSerializer.Serialize(eventData);
		}

		public static IEvent Deserialize(string eventData, string eventTypeString)
		{
			var eventType = Type.GetType(eventTypeString);

			if (eventType is null)
			{
				throw new InvalidOperationException($"Type '{eventTypeString}' not found.");
			}

			return (IEvent)System.Text.Json.JsonSerializer.Deserialize(eventData, eventType)!;
		}
	}
}
