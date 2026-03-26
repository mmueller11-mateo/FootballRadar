namespace FootballRadar.Business.Entities
{
    /// <summary>
    /// Represents a cloneable object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICloneable<out T>
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        T Clone();
    }
}
