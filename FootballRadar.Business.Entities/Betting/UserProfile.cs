namespace FootballRadar.Business.Entities.Betting
{
	public class UserProfile
	{
		public Guid UserId { get; set; }

		/// <summary>
		/// A dictionary of event type names and a boolean indicating whether the user wants to receive notifications for that event type.
		/// </summary>
		public Dictionary<string, bool> EventNotificationPreferences { get; set; } = [];

		// todo define reputation and stats
	}
}
