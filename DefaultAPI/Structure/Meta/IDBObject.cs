using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OwlAPI.Structure.Meta
{
    /// <summary>
    /// Every object that can be "seen" from outside is an DBObject.
    /// 
    /// A DBObject has db specific attributes and also user defined attributes.
    /// 
    /// author: s1ck
    /// </summary>
    public interface IDBObject
    {
        #region Database-defined Properties

        String UUID { get; }

        #endregion


        #region User-defined Properties

        /// <summary>
        /// Returns all attributes for this graph associated object.
        /// </summary>
        IDictionary<String, Object> Attributes { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the attribute-value associated with the specified key.
        /// </summary>
        /// <param name="myKey">the key of the element to be added or returned</param>
        /// <returns></returns>
        Object this[String myKey] { get; set; }

        /// <summary>
        /// Removes the attribute-value with the specified key if it exists.
        /// </summary>
        /// <param name="myKey">the key of the element to remove</param>
        /// <returns></returns>
        bool RemoveAttribute(String myKey);

        #endregion
    }
}
