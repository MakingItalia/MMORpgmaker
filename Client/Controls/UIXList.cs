using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


/* UIX SDK BY Thejuster
 * see my other project on pierotofy.it and Github
 * */


namespace UIXControls
{

    public class UIXList<T> : List<T> where T : Controls
    {

        /// <summary>
        /// Add Controls to list
        /// </summary>
        /// <param name="t">Controls</param>
        public new void Add(T t)
        {
            string[] nm = t.GetType().ToString().Split('.');
            ((Controls)t).Name = nm[nm.Length-1] + base.Count;
            base.Add(t);
        }

        /// <summary>
        /// Get type of Control
        /// </summary>
        /// <param name="t">Control</param>
        /// <returns>Type</returns>
        internal object GetType(T t)
        {
            return (object)t.GetType();
        }


        /// <summary>
        /// Read list based on file
        /// </summary>
        /// <param name="Forms">Form Name</param>
        /// <returns>Compiled List</returns>
        internal UIXList<T> Read(string Forms)
        {
            StreamReader sr = new StreamReader(Environment.CurrentDirectory + "/Forms/" + Forms + ".dat");
            //ToDo: Da Implementare

            return this;
        }


    }

}
