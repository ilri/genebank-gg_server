using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrinGlobal.Core {
	public class Sorter<T> {

		private List<T> _items;
		private Comparer<T> _comparer;
		private T _overflow;

		private void swap(ref T item1, ref T item2) {
			T holder = item1;
			item1 = item2;
			item2 = holder;
		}

		private bool lessThan(T item1, T item2) {
			return _comparer.Compare(item1, item2) < 0;
		}

		public void MergeSort(List<T> items, Comparer<T> comparer) {
			if (items == null || items.Count < 1) {
				return;
			}

			_items = items;
			_comparer = comparer;
			mergeSort(0, items.Count);
		}

		private void mergeSort(int start, int end){

			int midpoint = end / 2;

			if (midpoint > start) {
				mergeSort(start, midpoint);
			}
			if (end > midpoint + 1) {
				mergeSort(midpoint + 1, end);
			}
			merge(start, midpoint, midpoint + 1, end);
		}

		private void merge(int leftStart, int leftEnd, int rightStart, int rightEnd) {
			int leftPos = leftStart;
			int rightPos = rightStart;
			T left = default(T);
			T right = default(T);
			while (leftPos < leftEnd && rightPos < rightEnd) {
				left = _items[leftPos];
				right = _items[rightPos];

				if (lessThan(left, right)){
					// left is smaller than right
					if (_overflow != null && lessThan(_overflow, left)) {
						// overflow is smallest
						swap(ref left, ref _overflow);
					} else {
						// left is smallest, but its pointer it always incremented anyway...
					}
				} else {
					// right is smaller than left

					if (_overflow != null && lessThan(_overflow, right)) {
						// overflow is smallest
						swap(ref left, ref _overflow);
					} else {
						// right is smallest
						swap(ref left, ref right);
						rightPos++;
					}
				}

				// and step pass the one we just moved
				leftPos++;
			}
		}

	}
}
