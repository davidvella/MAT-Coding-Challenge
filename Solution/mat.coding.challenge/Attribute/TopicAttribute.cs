namespace mat.coding.challenge.Attribute
{
    /// <summary>
    /// This attribute is used to represent a topic name.
    /// </summary>
    public class TopicAttribute : System.Attribute
    {
        #region Properties

        /// <summary>
        /// Holds the stringvalue for the topic name.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public TopicAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

    }
}
