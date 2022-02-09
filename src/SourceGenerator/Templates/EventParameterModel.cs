namespace Thor.Generator.Templates
{
    internal class EventParameterModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsFirst { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public EventParameterModel Clone()
        {
            return (EventParameterModel) MemberwiseClone();
        }
    }
}
