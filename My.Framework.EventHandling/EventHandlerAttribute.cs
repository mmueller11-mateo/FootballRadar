namespace My.Framework.EventHandling
{
    [AttributeUsage(validOn: AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class EventHandlerAttribute : Attribute
    {
        public EventDispatchType DispatchType { get; set; }
    }
}
