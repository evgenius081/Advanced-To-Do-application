using System;

namespace ToDo.DomainModel.Attributes
{
    /// <summary>
    /// This attribute is used to represent a string value for a value in an enum.
    /// </summary>
    public class StringValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringValueAttribute"/> class.
        /// </summary>
        /// <param name="value">Value that will be used as attribute.</param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        /// <summary>
        /// Gets or sets the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }
    }
}
