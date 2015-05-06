using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using GrinGlobal.Core;
using System.Diagnostics;

namespace GrinGlobal.Search.Engine {

	internal class DataFileProcessor<T> where T : IExternalSortable<T>, new() {

		/// <summary>
		/// Given the BinaryReader, returns all items that are considered to be grouped with the first one from the given offset
		/// </summary>
		/// <param name="rdr"></param>
		/// <param name="fileOffset"></param>
		/// <returns></returns>
		internal IEnumerable<T> GetGroupOfDataItems(BinaryReader rdr, long location) {

			if (rdr.BaseStream.Length > location) {

				rdr.BaseStream.Position = location;
				int groupCount = rdr.ReadInt32();
				int i = 0;

				while (rdr.BaseStream.Position < rdr.BaseStream.Length && i < groupCount) {

					T item = new T();
					item.Read(rdr, false);

					yield return item;
					i++;
				}
			}
		}

		/// <summary>
		/// Is called once per row found in the DataReader.  Caller must return a list of items that are to be written to disk via a BinaryWriter (can return null or a list of length 0)
		/// </summary>
		/// <param name="idr"></param>
		/// <param name="additionalData">This is simply the object that was passed in to the method that eventually calls this callback -- allows a caller to have information "tag along" with the callback</param>
		/// <returns></returns>
		public delegate List<T> ProcessRowCallback(IDataReader idr, object additionalData);

        /// <summary>
        /// Is called once per id found.  Caller must return a list of items that are to be written to disk via a BinaryWriter (can return null or a list of length 0)
        /// </summary>
        /// <param name="idr"></param>
        /// <param name="additionalData">This is simply the object that was passed in to the method that eventually calls this callback -- allows a caller to have information "tag along" with the callback</param>
        /// <returns></returns>
        public delegate List<T> ProcessIDCallback(int primaryKeyID, object additionalData);

        /// <summary>
        /// Is called once per hit found.  Caller must return a list of items that are to be written to disk via a BinaryWriter (can return null or a list of length 0)
        /// </summary>
        /// <param name="idr"></param>
        /// <param name="additionalData">This is simply the object that was passed in to the method that eventually calls this callback -- allows a caller to have information "tag along" with the callback</param>
        /// <returns></returns>
        public delegate List<T> ProcessHitCallback(Hit hit, object additionalData);

		/// <summary>
		/// Is called when the final sorted data is being written to the merged file.  Since merging removes the key used for sorting from the output, this is the only change to link up an item with its groups' location in the data file.
		/// </summary>
		/// <param name="lastItemInGroup">The last item in the current group.  Mainly so the grouping key can be grabbed from it one last time before the value is lost forever.</param>
		/// <param name="dataFileLocation">The location in the file of the start of this group.  Always points to the group count header.  This header is simply a 4-byte signed int value followed immediately by the actual data.</param>
		/// <param name="groupCount">Number of items in the group</param>
		/// <param name="additionalData">This is simply the object that was passed in to the method that eventually calls this callback -- allows a caller to have information "tag along" with the callback</param>
		public delegate void ProcessGroupSortedCallback(T lastItemInGroup, long dataFileLocation, int groupCount, object additionalData);

		int _tempFileCount = 0;

		/// <summary>
		/// Given the DataReader, pulls all data into a temp file.  Used when we want to log data but not sort it yet (as handling chunking of data is done outside this class)
		/// </summary>
		/// <param name="idr"></param>
        /// <param name="maxTempFileSizeInMB"></param>
		/// <param name="targetFilePath"></param>
		/// <param name="rowCallback"></param>
        /// <param name="additionalData"></param>
		internal void WriteToTempFile(IDataReader idr, int maxTempFileSizeInMB, string targetFilePath, ProcessRowCallback rowCallback, object additionalData) {
			long maxBytes = maxTempFileSizeInMB * 1024 * 1024;

			BinaryWriter bw = null;
			try {
                if (idr.Read()) {
                    bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));
                    do {
                        List<T> items = rowCallback(idr, additionalData);
                        foreach (T item in items) {
                            item.Write(bw, true);
                        }

                        if (bw.BaseStream.Position >= maxBytes) {
                            // we've hit the file size limit.
                            // close current temp file, open a new one
                            bw.Close();
                            _tempFileCount++;
                            bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));

                        }
                    } while (idr.Read());
                }
			} finally {
				if (bw != null){
					bw.Close();
				}
			}
		}

        /// <summary>
        /// Given the list of hits, pulls all data into a temp file.  Used when we want to log data but not sort it yet (as handling chunking of data is done outside this class)
        /// </summary>
        /// <param name="idr"></param>
        /// <param name="maxTempFileSizeInMB"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="rowCallback"></param>
        /// <param name="additionalData"></param>
        internal void WriteToTempFile(List<Hit> hits, int maxTempFileSizeInMB, string targetFilePath, ProcessHitCallback hitCallback, object additionalData) {
            long maxBytes = maxTempFileSizeInMB * 1024 * 1024;

            BinaryWriter bw = null;
            try {
                if (hits.Count > 0) {
                    bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));
                    foreach (var hit in hits) {
                        List<T> items = hitCallback(hit, additionalData);
                        foreach (T item in items) {
                            item.Write(bw, true);
                        }

                        if (bw.BaseStream.Position >= maxBytes) {
                            // we've hit the file size limit.
                            // close current temp file, open a new one
                            bw.Close();
                            _tempFileCount++;
                            bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));

                        }
                    }
                }
            } finally {
                if (bw != null) {
                    bw.Close();
                }
            }
        }


        /// <summary>
        /// Given the list of primary key ids, pulls all data into a temp file.  Used when we want to log data but not sort it yet (as handling chunking of data is done outside this class)
        /// </summary>
        /// <param name="idr"></param>
        /// <param name="maxTempFileSizeInMB"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="rowCallback"></param>
        /// <param name="additionalData"></param>
        internal void WriteToTempFile(List<int> primaryKeyIDs, int maxTempFileSizeInMB, string targetFilePath, ProcessIDCallback idCallback, object additionalData) {
            long maxBytes = maxTempFileSizeInMB * 1024 * 1024;

            BinaryWriter bw = null;
            try {
                if (primaryKeyIDs.Count > 0) {
                    bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));
                    foreach (int id in primaryKeyIDs) {
                        List<T> items = idCallback(id, additionalData);
                        foreach (T item in items) {
                            item.Write(bw, true);
                        }

                        if (bw.BaseStream.Position >= maxBytes) {
                            // we've hit the file size limit.
                            // close current temp file, open a new one
                            bw.Close();
                            _tempFileCount++;
                            bw = new BinaryWriter(new FileStream(targetFilePath + ".temp_" + _tempFileCount, FileMode.Append, FileAccess.Write));

                        }
                    }
                }
            } finally {
                if (bw != null) {
                    bw.Close();
                }
            }
        }

		/// <summary>
		/// Sorts the pre-existing temp files and merges them into one data file.  Must be called after one or more calls to WriteToTempFile.
		/// </summary>
		/// <param name="targetFilePath"></param>
		internal void SortAndMergeTempFiles(string targetFilePath, bool retainSortKeyAndSuppressGroupCountHeader, ProcessGroupSortedCallback groupSortedCallback, object additionalData) {

			// due to how WriteToTempFile increments the temp file count,
			// and the fact that this is the only method that inspects the _tempFileCount variable 
			// we have to tack an extra one on the end...
			_tempFileCount++;

			sortTempFiles(_tempFileCount, targetFilePath);

			mergeSortedFiles(_tempFileCount, targetFilePath, retainSortKeyAndSuppressGroupCountHeader, groupSortedCallback, additionalData);

			_tempFileCount = 0;
		}

		/// <summary>
		/// Reads from DataReader and sorts data in one fell swoop.  The temporary files required by this method are written to disk during execution and deleted upon successful completion.
		/// </summary>
		/// <param name="idr">The DataReader to pull data from</param>
		/// <param name="maxSortSizeInMB">The maximum size, in megabytes, of data that is to be sorted in RAM at one time.
		/// <param name="targetFilePath">Full or partial path to file to output</param>
		/// <param name="rowCallback">A delegate for the caller to implement that is called for each row pulled from the given IDataReader object.</param>
		/// <param name="retainSortKeyAndSuppressGroupCountHeader">If true, no group count is written before each group and every item has the grouping key written with it (i.e. the value is repeated in every item).  If false, the group count is written and the grouping key is not written at all.</param>
		/// <param name="groupSortedCallback">A delegate for the caller to implement that is called after all the data has been written to file and sorted.  It returns the last item of a group, so the caller may do additional processing before the sort key is lost forever.</param>
		/// <param name="additionalData">An object that is passed back in each of the delegate calls -- allows caller to pass information into the callback.</param>
		internal void Sort(IDataReader idr, int maxSortSizeInMB, string targetFilePath, ProcessRowCallback rowCallback, bool retainSortKeyAndSuppressGroupCountHeader, ProcessGroupSortedCallback groupSortedCallback, object additionalData) {

			// first slurp data from DataReader into a set of 1 or more temp files
			int fileCount = writeTempFiles(idr, maxSortSizeInMB, rowCallback, targetFilePath, additionalData);

			// sort each temp file
			sortTempFiles(fileCount, targetFilePath);

			// merge sorted files
			mergeSortedFiles(fileCount, targetFilePath, retainSortKeyAndSuppressGroupCountHeader, groupSortedCallback, additionalData);

			// all done!

		}

		private int writeTempFiles(IDataReader idr, long maxFileSizeInMB, ProcessRowCallback rowCallback, string targetFilePath, object additionalData) {
			BinaryWriter bw = null;
			string tempFile = Toolkit.ResolveFilePath(targetFilePath + ".temp_0", true);
			int fileCount = 1;

			long maxFileSizeInBytes = maxFileSizeInMB * 1024 * 1024;

			try {
				// tell them they need to process the current row and write whatever associated data they want to the binary file
                // don't needlessly open/close the file
                if (idr.Read()) {
                    bw = new BinaryWriter(new FileStream(tempFile, FileMode.Create, FileAccess.Write));
                    do {
					    List<T> items = rowCallback(idr, additionalData);
					    if (items != null && items.Count > 0) {
						    foreach (T item in items) {
							    item.Write(bw, true);
							    if (bw.BaseStream.Position >= maxFileSizeInBytes) {
								    // we've hit the maximum file size (as defined by the caller)
								    // write this file, open a new one
								    bw.Close();
								    tempFile = Toolkit.ResolveFilePath(targetFilePath + ".temp_" + fileCount, true);
								    fileCount++;
								    bw = new BinaryWriter(new FileStream(tempFile, FileMode.Create, FileAccess.Write));
							    }
						    }
					    }
                    } while (idr.Read());
				}
			} finally {
				if (bw != null) {
					bw.Close();
				}
			}
			return fileCount;
		}

		private void sortTempFiles(int fileCount, string targetFilePath) {

			// now, we have a bunch of temp files.
			// process each one by sorting the entire file (which is why we generate multiple temp files -- to keep them small for sorting in RAM)
			for (int i=0; i < fileCount; i++) {
				string tempFile = Toolkit.ResolveFilePath(targetFilePath + ".temp_" + i, false);
				string sortedFile = Toolkit.ResolveFilePath(targetFilePath + ".sort_" + i, true);
				if (!File.Exists(tempFile)) {
					// make sure fileCount isn't bogus
					break;
				}
				using (BinaryReader rdr = new BinaryReader(new FileStream(tempFile, FileMode.Open, FileAccess.Read))) {
					using (BinaryWriter wtr = new BinaryWriter(new FileStream(sortedFile, FileMode.Create, FileAccess.Write))) {

						// read the entire temp file into a list of items
						List<T> items = new List<T>();
						while (rdr.BaseStream.Position < rdr.BaseStream.Length) {
							// read the object from file
							T item = new T();
							item.Read(rdr, true);
							items.Add(item);
						}

						if (items.Count > 0) {
							// sort the list of items
							IEnumerable<T> sorted = new T().Sort(items);

							// write the sorted list back out
							foreach (T item in sorted) {
								item.Write(wtr, true);
							}


							// sorting is typically very RAM intensive, 
							// so we orphan the possibly large list that was the source for the sort, 
							// then force a collection
							items.Clear();
							items = null;
							sorted = null;
							GC.Collect();
						}

					}
				}
				// we get here, we no longer need the current temp file (all its data is in the corresponding sorted file)
				File.Delete(tempFile);
			}

		}

		private void mergeSortedFiles(int fileCount, string targetFilePath, bool retainSortKeyAndSuppressGroupCountHeader, ProcessGroupSortedCallback groupSortedCallback, object additionalData) {

			string baseFileName = Toolkit.ResolveFilePath(targetFilePath, false);

			// We can use this optimization only when there's only 1 sorted file and caller doesn't expect callbacks on each group iteration
			if (fileCount == 1 && groupSortedCallback == null) {
				// there's only one sorted file -- no need to merge.
				// simply rename it (aka Move) as it's already sorted
				File.Move(baseFileName + ".sort_0", baseFileName);
				return;
			}


			BinaryReader[] rdrs = new BinaryReader[fileCount];
			List<int> fileIndexes = new List<int>();
			T[] items = new T[fileCount];
			try {

				// initialize reading from all the sorted files at once
				for (int i=0; i < fileCount; i++) {

					if (!File.Exists(baseFileName + ".sort_" + i)) {
						// check for bogus file count
						break;
					}

					rdrs[i] = new BinaryReader(new FileStream(baseFileName + ".sort_" + i, FileMode.Open, FileAccess.Read));

                    if (rdrs[i].BaseStream.Length == 0) {
                        // this sort file is empty! do not add it to the list to process.
                    } else {
                        // read first object from each stream
                        items[i] = new T();
                        items[i].Read(rdrs[i], true);

                        // remember our list of file offsets 
                        // we do this so that when the file is done processing, 
                        // we can pull it out of the list of files to process
                        fileIndexes.Add(i);

                    }


				}

				// initialize the final, merged file for writing
				using (BinaryWriter wtr = new BinaryWriter(new FileStream(baseFileName, FileMode.Create, FileAccess.Write))) {

					int fileWithSmallestItem = -1;
					T previousItem = default(T);
					long previousLocation = -1;
					int groupedItemCount = -1;

					while (fileIndexes.Count > 0) {

						// find minimum object from the front of each file
						foreach (int fileIndex in fileIndexes) {
							if (fileWithSmallestItem == -1 || items[fileIndex].CompareTo(items[fileWithSmallestItem]) < 0) {
								// current item is smaller than our current smallest one 
								// (or it's the first one and there is no current 'smallest' one)
								// assign it as the smallest one.
								fileWithSmallestItem = fileIndex;
							}
						}

						if (!retainSortKeyAndSuppressGroupCountHeader) {


							groupedItemCount++;
							
							// if this item is in a different grouping, write out the previous group's count,
							// make a placeholder for this groups count, and continue writing out the items
							if (previousLocation < 0 || !items[fileWithSmallestItem].IsInSameGroup(previousItem)) {


								if (previousLocation > -1){
									// we're starting a new group.
									// write the count to the previous location
									wtr.BaseStream.Position = previousLocation;
									wtr.Write(groupedItemCount);

									// give caller a chance to link to the right location in the data file (skip this on the first iteration, it's bogus)
									if (groupSortedCallback != null) {
										groupSortedCallback(previousItem, previousLocation, groupedItemCount, additionalData);
									}
								} else {
									// first time through.
									// don't do the 'normal' processing, don't call the callback

								}

								groupedItemCount = 0;

								// reposition to the end of the file, remember the location, write the placeholder for the count
								wtr.BaseStream.Position = wtr.BaseStream.Length;
								previousLocation = wtr.BaseStream.Position;
								wtr.Write(int.MinValue);
								// and write one instance of the sort key (instead of writing the same value for every item)
								//items[fileWithSmallestItem].WriteOnlySortKey(wtr);
							}
						}

						// we now know the smallest item.
						// copy it to our output stream 
                        items[fileWithSmallestItem].Write(wtr, retainSortKeyAndSuppressGroupCountHeader);

						previousItem = items[fileWithSmallestItem];

						// then read the next one from its file.
						if (rdrs[fileWithSmallestItem].BaseStream.Position == rdrs[fileWithSmallestItem].BaseStream.Length) {
							// we're at the end of this sorted file.
							// remove this file from the mix and mark the smallest offset as though it's starting fresh
							fileIndexes.Remove(fileWithSmallestItem);
							fileWithSmallestItem = -1;
						} else {
							// the smallest one's file still has data. just read it in.
							items[fileWithSmallestItem].Read(rdrs[fileWithSmallestItem], true);
						}

					}

					if (!retainSortKeyAndSuppressGroupCountHeader) {
						if (previousLocation < wtr.BaseStream.Position && previousLocation > -1) {
							// need to write out the count of the last group (and its sort key)
							wtr.BaseStream.Position = previousLocation;
							wtr.Write(groupedItemCount);
							//previousItem.WriteOnlySortKey(wtr);

							// give caller a chance to link to the right location in the data file
							if (groupSortedCallback != null) {
								groupSortedCallback(previousItem, previousLocation, groupedItemCount, additionalData);
							}

						}
					}

					// make sure the data is written out
					wtr.Flush();

				}



				// we get here, the merge worked.
				// we're just cleaning up.
				for (int i=0; i < fileCount; i++) {
					// initialize reading from all the sorted files at once
					if (rdrs[i] != null) {
						rdrs[i].Close();
						rdrs[i] = null;
					}

					// delete the sort files, we merged their data into one big file
					string sortFileName = Toolkit.ResolveFilePath(baseFileName + ".sort_" + i, false);
					if (File.Exists(sortFileName)) {
						File.Delete(sortFileName);
					}
				}
            } catch (Exception ex){
                Debug.WriteLine(ex.Message);
                throw;

			} finally {
				// even if the merge failed, be sure to close the associated readers so subsequent runs don't bomb
				for (int i=0; i < fileCount; i++) {
					if (rdrs[i] != null) {
						rdrs[i].Close();
						rdrs[i] = null;
					}
				}
			}
		}
	}
}
