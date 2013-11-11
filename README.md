Directory-Snapshot-Difference-Csharp
====================================

Utility which can save snapshot of some directory(all sub-directories and files with sizes) and later compare that snapshot with current state of same(or some other) directory. Written in c#, even javascript(nodejs) would be better(multiplatform).

### Usage

#### Compare snapshots (and optional save current snapshot)
dirsnap -c snapshot_file_path [diff_file_path] [--sortbysize] [-s file_path_to_search [snapshot_file_path]]

#### Save snapshot
dirsnap -s directory_path [snapshot_file_path]

		* snapshot_file_path = path where is old snapshot for comparing
		* diff_file_path = path where difference will be saved. if not specified path to directoty is inside snapshot file(same directory will be compared)
		* --sortbysize = sort by size, otherwise differences will be sorted by name
		* directory_path = directory that will be examined
		* snapshot_file_path = file where current snapshot will be saved
		If -c switch is used -s can be ommited. In that case snapshot directory is read from snapshot_file_path
