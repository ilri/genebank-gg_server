using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace GrinGlobal.Core {

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer {
        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;
        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        private SortOrder OrderOfSort;
        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private IComparer ObjectCompare;



        private class DateTimeComparer : IComparer {
            public int Compare(object val1, object val2) {
                return DateTime.Compare(Toolkit.ToDateTime(val1, DateTime.MinValue), Toolkit.ToDateTime(val2, DateTime.MinValue));
            }
        }

        private class DecimalComparer : IComparer {
            public int Compare(object val1, object val2) {
                return Decimal.Compare(Toolkit.ToDecimal(val1.ToString().Replace("$", "").Replace(",", ""), Decimal.MinValue), Toolkit.ToDecimal(val2.ToString().Replace("$", "").Replace(",", ""), Decimal.MinValue));
            }
        }

        public void DetermineComparer(ListView lv) {
            if (lv.Items.Count == 0) {
                return;
            }

            var val1 = lv.Items[0].SubItems[ColumnToSort].Text;

            var dt1 = Toolkit.ToDateTime(val1, DateTime.MinValue);
            if (dt1 > DateTime.MinValue) {
                // compare as dates
                ObjectCompare = new DateTimeComparer();
            } else {
                var dec1 = Toolkit.ToDecimal(val1.Replace("$", "").Replace(",", ""), Decimal.MinValue);
                if (dec1 > Decimal.MinValue) {
                    // compare as numbers
                    ObjectCompare = new DecimalComparer();
                } else {
                    // assume it's text
                    ObjectCompare = new CaseInsensitiveComparer();
                }
            }
        }


        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter() {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the sort order to 'none'
            OrderOfSort = SortOrder.None;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y) {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            var val1 = listviewX.SubItems[ColumnToSort].Text;
            var val2 = listviewY.SubItems[ColumnToSort].Text;

            compareResult = ObjectCompare.Compare(val1, val2);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending) {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            } else if (OrderOfSort == SortOrder.Descending) {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            } else {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn {
            set {
                ColumnToSort = value;
            }
            get {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order {
            set {
                OrderOfSort = value;
            }
            get {
                return OrderOfSort;
            }
        }

    }
}
